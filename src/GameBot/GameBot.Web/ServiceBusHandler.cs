using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using System.Text.Json;
using GameBot.Domain;
using System;
using Microsoft.Extensions.Logging;

namespace GameBot.Web
{
    public interface IServiceBusHandler {
        Task SendOrderToQueue(Order order);
        Task SetupOrderUpdateTopicListener(Func<OrderResult, Task> action);
    }

    public class ServiceBusHandler : IServiceBusHandler
    {
        private readonly string _connectionString;
        private readonly string _orderQueueName;
        private readonly string _PurchaseUpdateTopic;
        private readonly string _PurchaseUpdateTopicSubscription;
        private readonly ILogger<ServiceBusHandler> _logger;

        public ServiceBusHandler(IConfiguration config, ILogger<ServiceBusHandler> logger) {
            _connectionString = config["ServiceBusConnectionString"];
            _orderQueueName = config["OrderQueueName"];
            _PurchaseUpdateTopic = config["PurchaseUpdateTopicName"];
            _PurchaseUpdateTopicSubscription = config["PurchaseUpdateTopicSubscription"];
            _logger = logger;
        }

        public async Task SendOrderToQueue(Order order)
        {
            await using ServiceBusClient client = new ServiceBusClient(_connectionString);
            var sender = client.CreateSender(_orderQueueName);
            var message = new ServiceBusMessage(JsonSerializer.Serialize(order));
            await sender.SendMessageAsync(message);
        }

        public async Task SetupOrderUpdateTopicListener(Func<OrderResult, Task> action) {
            await using ServiceBusClient client = new ServiceBusClient(_connectionString);
            var processor = client.CreateProcessor(_PurchaseUpdateTopic, _PurchaseUpdateTopicSubscription);

            async Task handleMessage(ProcessMessageEventArgs args)
            {
                var orderResult = JsonSerializer.Deserialize<OrderResult>(args.Message.Body.ToString());
                await action(orderResult);
                await args.CompleteMessageAsync(args.Message);
            }

            Task handleError(ProcessErrorEventArgs args)
            {
                _logger.LogError(args.Exception.ToString());
                return Task.CompletedTask;
            }

            processor.ProcessMessageAsync += handleMessage;
            processor.ProcessErrorAsync += handleError;

            // start processing
            await processor.StartProcessingAsync();
        }
    }
}
