using Azure;
using Azure.AI.OpenAI;
using Azure.AI.OpenAI.Chat;
using OpenAI.Assistants;
using OpenAI.Chat;
using RoutingApp.API.Controllers;
using System.ClientModel;
using System.Text;

namespace RoutingApp.API.Services
{
	public interface IAzureAIResponseService
	{
		Task<string> ProcessMessageAsync(string userMessage);
		void ClearHistory();
		IReadOnlyList<ChatMessage> GetHistory();
		List<MessageDto> GetFormattedHistory();
	}
	public class AzureAIResponseService : IAzureAIResponseService
	{
#pragma warning disable OPENAI001
		private readonly AzureOpenAIClient _azureClient;
		private readonly ChatClient _chatClient;
		private readonly IConfiguration _config;
		private readonly List<ChatMessage> _messageHistory = new();
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

			_messageHistory.Add(ChatMessage.CreateSystemMessage(_systemPrompt));
		}

		public async Task<string> ProcessMessageAsync(string userMessage)
		{
			_messageHistory.Add(ChatMessage.CreateUserMessage(userMessage));

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

			ClientResult<ChatCompletion> response = await _chatClient.CompleteChatAsync(
				_messageHistory,
				chatOptions
			);

			var completion = response.Value;
			var assistantMessage = completion.Content[0].Text;

			if (!string.IsNullOrEmpty(assistantMessage))
			{
				_messageHistory.Add(ChatMessage.CreateAssistantMessage(assistantMessage));
			}

			return assistantMessage ?? "No response generated";
		}

		public void ClearHistory()
		{
			_messageHistory.Clear();
			_messageHistory.Add(ChatMessage.CreateSystemMessage(_systemPrompt));
		}

		public IReadOnlyList<ChatMessage> GetHistory() => _messageHistory.AsReadOnly();

		public List<MessageDto> GetFormattedHistory()
		{
			return _messageHistory.Select(m => new MessageDto
			{
				Role = GetRoleString(m),
				Content = GetContentString(m)
			}).ToList();
		}

		private string GetRoleString(ChatMessage message)
		{
			if (message is SystemChatMessage) return "system";
			if (message is UserChatMessage) return "user";
			if (message is AssistantChatMessage) return "assistant";
			return "unknown";
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
