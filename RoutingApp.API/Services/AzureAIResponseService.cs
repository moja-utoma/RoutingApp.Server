using Azure;
using Azure.AI.OpenAI;
using Azure.AI.OpenAI.Chat;
using OpenAI.Assistants;
using OpenAI.Chat;
using RoutingApp.API.Controllers;
using System.ClientModel;
using System.Collections.Concurrent;
using System.Text;

namespace RoutingApp.API.Services
{
	public interface IAzureAIResponseService
	{
		Task<ChatResponse> ProcessMessageAsync(ChatRequest request);
		void ClearHistory(string conversationId);
		IReadOnlyList<ChatMessage> GetHistory(string conversationId);
		List<MessageDto> GetFormattedHistory(string conversationId);
	}
	public class AzureAIResponseService : IAzureAIResponseService
	{
#pragma warning disable OPENAI001
		private readonly AzureOpenAIClient _azureClient;
		private readonly ChatClient _chatClient;
		private readonly IConfiguration _config;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ConcurrentDictionary<string, List<ChatMessage>> _conversationHistories = new();
		private readonly string _systemPrompt = @"You are a helpful assistant for a logistics platform. 
Your role is to guide users through using the site, including navigating pages, creating entities, and starting simulations. 
You do not perform actions yourself—your job is to explain how users can do these tasks on their own.
Always provide clear, step-by-step instructions. If a user asks how to create something, explain where to go and what fields to fill. 
If they ask how to start a simulation, describe the prerequisites and the steps to trigger it. Use context from the FAQ and file-based instructions when available.
If the user's question relates to delivery points, warehouses, vehicles, or routes, refer to the correct page and explain the form fields based on the entity schema. 
If the route must be calculated before simulation, remind the user to click the 'Calculate' button and verify the map shows connected points.
Be concise, instructional, and proactive. Ask for missing information if needed, 
and always assume the user wants to understand how—not to have the assistant do it for them.";

		public AzureAIResponseService(IConfiguration config)
		{
			_config = config;
			var endpoint = new Uri(_config["AzureOpenAI:Endpoint"]);
			var key = new AzureKeyCredential(_config["AzureOpenAI:Key"]);
			_azureClient = new AzureOpenAIClient(endpoint, key);
			_chatClient = _azureClient.GetChatClient(_config["AzureOpenAI:DeploymentName"]);
		}

		public async Task<ChatResponse> ProcessMessageAsync(ChatRequest request)
		{
			var conversationId = string.IsNullOrWhiteSpace(request.ConversationId)
		? Guid.NewGuid().ToString()
		: request.ConversationId;

			var history = _conversationHistories.GetOrAdd(conversationId, _ => new List<ChatMessage> {
		ChatMessage.CreateSystemMessage(_systemPrompt)
	});

			history.Add(ChatMessage.CreateUserMessage(request.Message));

			var chatOptions = new ChatCompletionOptions();

			var searchEndpoint = _config["AzureAISearch:Endpoint"];
			var searchKey = _config["AzureAISearch:Key"];
			var searchIndex = _config["AzureAISearch:IndexName"];

			if (!string.IsNullOrEmpty(searchEndpoint) && !string.IsNullOrEmpty(searchIndex))
			{
#pragma warning disable AOAI001
				chatOptions.AddDataSource(new AzureSearchChatDataSource
				{
					Endpoint = new Uri(searchEndpoint),
					Authentication = DataSourceAuthentication.FromApiKey(searchKey),
					IndexName = searchIndex
				});
			}

			var response = await _chatClient.CompleteChatAsync(history, chatOptions);
			var assistantMessage = response.Value.Content[0].Text;

			if (!string.IsNullOrEmpty(assistantMessage))
			{
				history.Add(ChatMessage.CreateAssistantMessage(assistantMessage));
			}

			return new ChatResponse
			{
				Message = assistantMessage ?? "No response generated",
				Timestamp = DateTime.UtcNow,
				ConversationId = conversationId
			};
		}

		public void ClearHistory(string conversationId)
		{
			_conversationHistories[conversationId] = new List<ChatMessage>
			{
				ChatMessage.CreateSystemMessage(_systemPrompt)
			};
		}

		public IReadOnlyList<ChatMessage> GetHistory(string conversationId)
		{
			return _conversationHistories.TryGetValue(conversationId, out var history)
				? history.AsReadOnly()
				: new List<ChatMessage> { ChatMessage.CreateSystemMessage(_systemPrompt) }.AsReadOnly();
		}

		public List<MessageDto> GetFormattedHistory(string conversationId)
		{
			var history = GetHistory(conversationId);
			return history.Select(m => new MessageDto
			{
				Role = GetRoleString(m),
				Content = GetContentString(m)
			}).ToList();
		}

		private string GetRoleString(ChatMessage message)
		{
			return message switch
			{
				SystemChatMessage => "system",
				UserChatMessage => "user",
				AssistantChatMessage => "assistant",
				_ => "unknown"
			};
		}

		private string GetContentString(ChatMessage message)
		{
			if (message.Content is IEnumerable<ChatMessageContentPart> parts)
			{
				var textPart = parts.FirstOrDefault();
				return textPart?.Text ?? string.Empty;
			}
			return message.Content?.ToString() ?? string.Empty;
		}

	}
}
