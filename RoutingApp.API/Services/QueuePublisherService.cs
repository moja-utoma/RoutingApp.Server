using Azure.Messaging.ServiceBus;
using RoutingApp.Shared.Messaging;
using System.Text;
using System.Text.Json;

namespace RoutingApp.API.Services
{
    public interface IQueuePublisherService
    {
        Task PublishRouteJobAsync(RouteJobMessage message);
		Task<RouteJobMessage?> WaitForReplyAsync(string replyQueue, string correlationId, TimeSpan timeout);
		Task<RouteJobMessage?> ReadReplyAsync(string replyQueue);
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
			Console.WriteLine($"Publishing job to {_queueName} with CorrelationId: {message.CorrelationId}");
			await sender.SendMessageAsync(sbMessage);
			Console.WriteLine("Message published successfully");
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
				return null;
			}

			if (replyMessage == null)
				return null;

			var body = replyMessage.Body.ToArray();
			return JsonSerializer.Deserialize<RouteJobMessage>(body);
		}

		public async Task<RouteJobMessage?> ReadReplyAsync(string replyQueue)
		{
			Console.WriteLine($"Attempting to read from queue: {replyQueue}");
			var receiver = _client.CreateReceiver(replyQueue, new ServiceBusReceiverOptions
			{
				ReceiveMode = ServiceBusReceiveMode.PeekLock
			});

			var message = await receiver.ReceiveMessageAsync(TimeSpan.FromSeconds(5));
			if (message == null)
			{
				Console.WriteLine($"No message received from queue '{replyQueue}'.");
				return null;
			}

			Console.WriteLine($"Raw message body: {Encoding.UTF8.GetString(message.Body)}");
			Console.WriteLine($"CorrelationId: {message.CorrelationId}");

			var body = message.Body.ToArray();
			var reply = JsonSerializer.Deserialize<RouteJobMessage>(body);

			if (reply == null)
			{
				Console.WriteLine("Deserialization failed.");
			}
			else
			{
				Console.WriteLine($"Deserialized reply: RouteId={reply.RouteId}, CorrelationId={reply.CorrelationId}");
			}

			await receiver.CompleteMessageAsync(message);
			return reply;
		}
	}
}
