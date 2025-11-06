using Azure.Messaging.ServiceBus;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.SignalR;
using RoutingApp.API.Models.Responses.Routes;
using RoutingApp.Data.Repositories;
using RoutingApp.Shared.Messaging;
using System.Text;
using System.Text.Json;

namespace RoutingApp.API.Services
{
	public class RouteReplyListener : BackgroundService
	{
		private readonly IServiceProvider _provider;
		private readonly IHubContext<RouteHub> _hubContext;
		private readonly IConfiguration _configuration;
		private readonly ILogger<RouteReplyListener> _logger;
		private readonly TelemetryClient _telemetry;

		public RouteReplyListener(
		IServiceProvider provider,
		IHubContext<RouteHub> hubContext,
		IConfiguration configuration,
		ILogger<RouteReplyListener> logger,
		TelemetryClient telemetry)
		{
			_provider = provider;
			_hubContext = hubContext;
			_configuration = configuration;
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("RouteReplyListener started");

			ServiceBusReceiver? receiver = null;

			try
			{
				using var scope = _provider.CreateScope();
				var serviceBusClient = scope.ServiceProvider.GetRequiredService<ServiceBusClient>();

				_logger.LogInformation("Creating Service Bus receiver for queue: reply-route-jobs");

				receiver = serviceBusClient.CreateReceiver("reply-route-jobs", new ServiceBusReceiverOptions
				{
					ReceiveMode = ServiceBusReceiveMode.PeekLock
				});

				_logger.LogInformation("Receiver created successfully, starting message loop");

				while (!stoppingToken.IsCancellationRequested)
				{
					try
					{
						_logger.LogInformation("Waiting for message...");

						var message = await receiver.ReceiveMessageAsync(
							maxWaitTime: TimeSpan.FromSeconds(10),
							cancellationToken: stoppingToken
						);

						if (message == null)
						{
							_logger.LogInformation("No message received in 10 seconds");
							continue;
						}

						_logger.LogInformation($"MESSAGE RECEIVED! CorrelationId: {message.CorrelationId}, MessageId: {message.MessageId}");

						var body = message.Body.ToArray();
						var reply = JsonSerializer.Deserialize<RouteJobMessage>(body);

						_telemetry.TrackEvent("ServiceBusMessageReceived", new Dictionary<string, string>
						{
							{ "CorrelationId", message.CorrelationId ?? "null" },
							{ "MessageId", message.MessageId },
							{ "RouteId", reply?.RouteId.ToString() ?? "unknown" }
						});

						if (reply == null || string.IsNullOrWhiteSpace(reply.CorrelationId))
						{
							_logger.LogWarning("Invalid reply message");
							await receiver.CompleteMessageAsync(message, stoppingToken);
							continue;
						}

						_logger.LogInformation($"Processing reply for RouteId: {reply.RouteId}");

						using var processingScope = _provider.CreateScope();
						var calcRepo = processingScope.ServiceProvider
							.GetRequiredService<ICalculatedRouteRepository>();

						var calculatedRoute = await calcRepo.GetCalculatedRouteByRouteIdAsync(reply.RouteId);

						if (calculatedRoute == null)
						{
							_logger.LogWarning($"No calculated route found for RouteId: {reply.RouteId}");
							await receiver.CompleteMessageAsync(message, stoppingToken);
							continue;
						}

						var dto = new CalculatedRouteDto
						{
							Id = calculatedRoute.Id,
							RouteId = calculatedRoute.Route.Id,
							Calculation = calculatedRoute.Calculation,
							CreatedAt = calculatedRoute.CreatedAt
						};

						_logger.LogInformation($"Sending to SignalR group: {reply.CorrelationId}");
						await _hubContext.Clients.Group(reply.CorrelationId)
							.SendAsync("ReceiveRoute", dto, stoppingToken);

						await receiver.CompleteMessageAsync(message, stoppingToken);
						_logger.LogInformation($"✓ Message processed successfully");
					}
					catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
					{
						_logger.LogInformation("Shutdown requested");
						break;
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, "Error in message loop");
						_telemetry.TrackException(ex, new Dictionary<string, string>
						{
							{ "Context", "RouteReplyListener" },
							{ "Phase", "MessageLoop" }
						});
						await Task.Delay(1000, stoppingToken);
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "FATAL ERROR in RouteReplyListener - service stopped!");
				_telemetry.TrackException(ex, new Dictionary<string, string>
				{
					{ "Context", "RouteReplyListener" },
					{ "Phase", "MessageLoop" }
				});
			}
			finally
			{
				if (receiver != null)
				{
					await receiver.DisposeAsync();
					_logger.LogInformation("Receiver disposed");
				}
				_logger.LogInformation("RouteReplyListener stopped");
			}
		}
	
	}

}
