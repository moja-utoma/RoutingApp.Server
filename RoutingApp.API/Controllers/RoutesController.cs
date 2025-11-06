using Azure.Messaging.ServiceBus;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoutingApp.API.Models.DTO;
using RoutingApp.API.Services.Interfaces;
using RoutingApp.API.Validation;
using RoutingApp.Shared.Messaging;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RoutingApp.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class RoutesController : Controller
	{
		private readonly IRouteService _routeService;
        private readonly ILogger<RoutesController> _logger;
        //private readonly IHttpClientFactory _httpClientFactory;
        //private const string OrsUrl = "https://api.openrouteservice.org/v2/directions/driving-car/geojson";
        //private const string OrsApiKey = "api_key";

        public RoutesController(IRouteService routeService, ILogger<RoutesController> logger, IHttpClientFactory httpClientFactory)
		{
			_routeService = routeService;
			_logger = logger;
            //_httpClientFactory=httpClientFactory;

            _logger.LogInformation("RoutesController Started");
        }

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var result = await _routeService.GetAllRoutesAsync();
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetByID(int id)
		{
			try
			{
				var result = await _routeService.GetRouteByIDAsync(id);
				return Ok(result);
			}
			catch (Exception e)
            {
                _logger.LogError(e, "Encountered error while getting route by id: {e.Message}", e.Message);
                return BadRequest(e.Message);
            }
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateRouteRequestDTO dto)
		{
			try
			{
				var validator = new RouteValidator();
				await validator.ValidateAndThrowAsync(dto);

				var result = await _routeService.CreateRouteAsync(dto);
				return Ok(result);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Encountered error while creating route: {e.Message}", e.Message);
				return BadRequest(e.Message);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await _routeService.DeleteAsync(id);
				return Ok();
			}
			catch (Exception e)
			{
                _logger.LogError(e, "Encountered error while deleting route: {e.Message}", e.Message);
                return BadRequest(e.Message);
			}
		}

		[HttpPut]
		public async Task<IActionResult> Edit(EditRouteRequestDTO request)
		{
			try
			{
				var result = await _routeService.EditAsync(request);
				return Ok(result);
			}
			catch (Exception e)
			{
                _logger.LogError(e, "Encountered error while updating route: {e.Message}", e.Message);
                return BadRequest(e.Message);
			}
		}

		[HttpPost("Calculate/{id}")]
		public async Task<IActionResult> CalculateRoute(int id)
		{
			try
			{
				var res = await _routeService.EnqueueRouteCalculationAsync(id); //CalculateRouteAsync
				return Ok(new { CorrelationId = res });
			}
			catch (Exception e)
			{

				return BadRequest(e.Message);
			}
		}

		//[HttpPost("TestSendReply")]
		//public async Task<IActionResult> TestSendReply()
		//{
		//	var testMessage = new RouteJobMessage
		//	{
		//		RouteId = 1,
		//		CorrelationId = Guid.NewGuid().ToString(),
		//		RequestedBy = "test",
		//		Timestamp = DateTime.UtcNow,
		//		ReplyTo = "reply-route-jobs"
		//	};

		//	var sender = _serviceBusClient.CreateSender("reply-route-jobs");
		//	var body = JsonSerializer.Serialize(testMessage);
		//	var message = new ServiceBusMessage(body)
		//	{
		//		CorrelationId = testMessage.CorrelationId
		//	};

		//	await sender.SendMessageAsync(message);

		//	return Ok(new { message = "Test message sent", correlationId = testMessage.CorrelationId });
		//}
	}
}
