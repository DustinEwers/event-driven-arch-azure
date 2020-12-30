using GameBot.Domain;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;

namespace GameBot.API
{
    public static class HandlePurchase
    {
        [FunctionName("HandlePurchase")]
        [return: ServiceBus("game-coin-purchase-updates", Connection = "ServiceBusConnection")]
        public static string Run(
            [ServiceBusTrigger("in-game-coin-purchases", Connection = "ServiceBusConnection")] string orderMessage,
            ILogger log)
        {
            var order = JsonSerializer.Deserialize<Order>(orderMessage);

            // Pretend to call a payment API
            Thread.Sleep(3000);

            var result = new OrderResult
            {
                OrderId = order.OrderId,
                OrderSuccessful = true, // Pretending it all worked out fine
                User = order.User
            };

            log.LogInformation($"C# Queue trigger function processed: {orderMessage}");
            return JsonSerializer.Serialize(result);
        }
    }
}
