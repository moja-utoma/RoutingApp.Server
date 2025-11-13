using Microsoft.AspNetCore.Mvc;
using RoutingApp.API.Services;

namespace RoutingApp.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ChatController : ControllerBase
	{
		private readonly IAzureAIResponseService _chatService;

		public ChatController(IAzureAIResponseService chatService)
		{
			_chatService = chatService;
		}

		[HttpPost("message")]
		public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
		{
			if (string.IsNullOrWhiteSpace(request.Message))
			{
				return BadRequest(new { error = "Message cannot be empty" });
			}

			var response = await _chatService.ProcessMessageAsync(request);

			return Ok(response);
		}

		[HttpPost("clear")]
		public IActionResult ClearHistory(string conversationId)
		{
			_chatService.ClearHistory(conversationId);
			return Ok(new { message = "Chat history cleared" });
		}

		[HttpGet("history")]
		public IActionResult GetHistory(string conversationId)
		{
			var history = _chatService.GetFormattedHistory(conversationId);

			return Ok(new { history });
		}
	}
	public class MessageDto
	{
		public string Role { get; set; }
		public string Content { get; set; }
	}
	public class ChatRequest
	{
		public string ConversationId { get; set; }
		public string Message { get; set; }
	}

	public class ChatResponse
	{
		public string ConversationId { get; set; }
		public string Message { get; set; }
		public DateTime Timestamp { get; set; }
	}

}
