using GameBot.Domain;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
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

            var paymentResult = SendtoPaymentApi(order);

            var result = new OrderResult
            {
                OrderId = order.OrderId,
                OrderSuccessful = paymentResult,
                User = order.User,
                Coins = order.Coins
            };

            log.LogInformation($"C# Queue trigger function processed: {orderMessage}");
            return JsonSerializer.Serialize(result);
        }

        private static bool SendtoPaymentApi(Order order) {
            // Pretend to call a payment API
            Thread.Sleep(3000);

            var rand = new Random();
            return rand.Next(1, 5) != 5; // It works most of the time...
        }
    }
}
