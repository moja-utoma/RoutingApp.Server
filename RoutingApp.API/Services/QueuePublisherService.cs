using Azure.Messaging.ServiceBus;
using RoutingApp.Shared.Messaging;
using System.Text.Json;

namespace RoutingApp.API.Services
{
    public interface IQueuePublisherService
    {
        Task PublishRouteJobAsync(RouteJobMessage message);
    }
    public class QueuePublisherService:IQueuePublisherService
    {
        private readonly ServiceBusClient _client;
        private readonly string _queueName;
        public QueuePublisherService(ServiceBusClient client, IConfiguration config)
        {
            _client = client;
            _queueName = config["ServiceBus:QueueName"]!;
        }
        public async Task PublishRouteJobAsync(RouteJobMessage message)
        {
            var sender = _client.CreateSender(_queueName);
            var body = JsonSerializer.Serialize(message);

            var sbMessage = new ServiceBusMessage(body)
            {
                MessageId = message.CorrelationId,
                CorrelationId = message.CorrelationId,
                ContentType = "application/json"
            };

            await sender.SendMessageAsync(sbMessage);
        }
    }
}
