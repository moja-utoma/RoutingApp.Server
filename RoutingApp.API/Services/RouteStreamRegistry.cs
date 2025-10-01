using RoutingApp.API.Services.Interfaces;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading.Channels;

namespace RoutingApp.API.Services
{
    public interface IRouteStreamRegistry
    {
        Channel<string> GetOrCreateChannel(string routeId);
        void StartRouteStream(string routeId, string payloadJson, CancellationToken cancellationToken);
    }

    public class RouteStreamRegistry : IRouteStreamRegistry
    {
        private readonly ConcurrentDictionary<string, Channel<string>> _channels = new();
        private readonly ConcurrentDictionary<string, Task> _streamTasks = new();
        private readonly ILogger<RouteStreamRegistry> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public RouteStreamRegistry(IServiceScopeFactory scopeFactory, ILogger<RouteStreamRegistry> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public Channel<string> GetOrCreateChannel(string routeId) =>
            _channels.GetOrAdd(routeId, _ => Channel.CreateUnbounded<string>());

        public void StartRouteStream(string routeId, string payloadJson, CancellationToken cancellationToken)
        {
            if (_streamTasks.ContainsKey(routeId)) return;

            var channel = GetOrCreateChannel(routeId);
            var writer = channel.Writer;

            var task = Task.Run(async () =>
            {
                using var scope = _scopeFactory.CreateScope();
                var orsService = scope.ServiceProvider.GetRequiredService<IOrsService>();

                try
                {
                    await foreach (var coord in orsService.StreamRouteAsync(routeId, payloadJson, cancellationToken))
                    {
                        var timestamp = DateTime.UtcNow;
                        var json = JsonSerializer.Serialize(new
                        {
                            routeId,
                            lat = coord[1],
                            lng = coord[0],
                            timestamp = timestamp.ToString("o")
                        });

                        var ssePayload = $"id: {timestamp:O}\ndata: {json}\n\n";
                        await writer.WriteAsync(ssePayload, cancellationToken);
                        _logger.LogInformation("Route {RouteId} emitted point at {Timestamp}", routeId, timestamp);

                        await Task.Delay(500, cancellationToken);
                    }

                    writer.Complete();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in background stream for route {RouteId}", routeId);
                    writer.Complete(ex);
                }
                finally
                {
                    _streamTasks.TryRemove(routeId, out _);
                    _channels.TryRemove(routeId, out _);
                    _logger.LogInformation("Stream for route {RouteId} has ended and was removed", routeId);
                }
            }, cancellationToken);

            _streamTasks[routeId] = task;
        }
    }

}
