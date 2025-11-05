using Azure.Messaging.ServiceBus;
using RoutingApp.Shared.Messaging;
using System.Text.Json;

namespace RoutingApp.API.Services
{
    public interface IQueuePublisherService
    {
        Task PublishRouteJobAsync(RouteJobMessage message);
		Task<RouteJobMessage?> WaitForReplyAsync(string replyQueue, string correlationId, TimeSpan timeout);

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
		public async Task<RouteJobMessage?> WaitForReplyAsync(string replyQueue, string correlationId, TimeSpan timeout)
		{
			var receiver = _client.CreateReceiver(replyQueue, new ServiceBusReceiverOptions
			{
				ReceiveMode = ServiceBusReceiveMode.PeekLock
			});

			var cts = new CancellationTokenSource(timeout);
			ServiceBusReceivedMessage? replyMessage = null;

			try
			{
				while (!cts.IsCancellationRequested)
				{
					var messages = await receiver.ReceiveMessagesAsync(maxMessages: 5, maxWaitTime: TimeSpan.FromSeconds(2), cancellationToken: cts.Token);

					foreach (var msg in messages)
					{
						if (msg.CorrelationId == correlationId)
						{
							replyMessage = msg;
							await receiver.CompleteMessageAsync(msg, cts.Token);
							break;
						}
					}

					if (replyMessage != null)
						break;
				}
			}
			catch (TaskCanceledException)
			{
				return null; // Timeout
			}

			if (replyMessage == null)
				return null;

			var body = replyMessage.Body.ToArray();
			return JsonSerializer.Deserialize<RouteJobMessage>(body);
		}

	}
}
