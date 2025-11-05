using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using RoutingApp.Data.Entities;
using RoutingApp.Data.Repositories.Interfaces;
using RoutingApp.Shared.Messaging;
using System.Text;
using System.Text.Json;

namespace RoutingApp.Worker.Functions
{
    public class RouteJobProcessor
    {
        private readonly IRouteRepository _routeRepository;
        private readonly ILogger<RouteJobProcessor> _logger;
		private readonly ServiceBusClient _serviceBusClient;

		public RouteJobProcessor(
		   IRouteRepository routeRepository,
		   ILogger<RouteJobProcessor> logger,
		   ServiceBusClient serviceBusClient)
		{
			_routeRepository = routeRepository;
			_logger = logger;
			_serviceBusClient = serviceBusClient;
		}

		[Function("RouteJobProcessor")]
        public async Task Run(
            [ServiceBusTrigger("%ServiceBus:QueueName%", Connection = "ServiceBus:ConnectionString")]
            ServiceBusReceivedMessage message)
        {
			var body = message.Body.ToArray();
			var job = JsonSerializer.Deserialize<RouteJobMessage>(body);

			if (job == null)
            {
                _logger.LogError("Invalid message format");
                return;
            }

            _logger.LogInformation($"Processing route job: {job.RouteId}, correlation: {job.CorrelationId}");

            var route = await _routeRepository.GetByIdAsync(job.RouteId);
            if (route == null)
            {
                _logger.LogError($"Route {job.RouteId} not found");
                throw new Exception("Route not found");
            }

            await Task.Delay(3000);

            var calc = new CalculatedRoute
            {
                Calculation = $"Mock result for route {job.RouteId}",
                CreatedAt = DateTime.UtcNow,
                Route = route
            };

            route.Status = "Completed";
            route.UpdatedAt = DateTime.UtcNow;
			if (route.CalculatedRoutes == null)
			{
				route.CalculatedRoutes = new List<CalculatedRoute>();
			}

			route.CalculatedRoutes.Add(calc);

			try
			{
				await _routeRepository.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Unhandled exception while processing route job {job?.RouteId}");
				//throw; 
			}

            _logger.LogInformation($"Route {job.RouteId} updated to Completed");

			var reply = new RouteJobMessage
			{
				RouteId = job.RouteId,
				CorrelationId = job.CorrelationId,
				RequestedBy = "processor",
				Timestamp = DateTime.UtcNow,
				ReplyTo = job.ReplyTo
			};

			var replySender = _serviceBusClient.CreateSender(job.ReplyTo);
			var replyMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(reply)))
			{
				CorrelationId = job.CorrelationId
			};

			try
			{
				await replySender.SendMessageAsync(replyMessage);
				_logger.LogInformation($"Reply sent to {job.ReplyTo} with correlation {job.CorrelationId}");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Failed to send reply for route job {job.RouteId}");
			}
		}
    }
}
