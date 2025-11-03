using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using RoutingApp.Data.Entities;
using RoutingApp.Data.Repositories.Interfaces;
using RoutingApp.Shared.Messaging;

namespace RoutingApp.Worker.Functions
{
    public class RouteJobProcessor
    {
        private readonly IRouteRepository _routeRepository;
        private readonly ILogger<RouteJobProcessor> _logger;

        public RouteJobProcessor(IRouteRepository routeRepository, ILogger<RouteJobProcessor> logger)
        {
            _routeRepository = routeRepository;
            _logger = logger;
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
        }
    }
}
