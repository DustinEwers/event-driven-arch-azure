using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace GameBot.Web.Hubs
{
    public class UserUpdateHub : Hub
    {
        private readonly IServiceBusHandler _serviceBus;

        public UserUpdateHub(IServiceBusHandler serviceBus) {
            _serviceBus = serviceBus;
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task Start() {
            await _serviceBus.SetupOrderUpdateTopicListener(async (orderResult) =>
            {
                await SendMessage($"{orderResult.User}'s order number {orderResult.OrderId } was {orderResult.OrderSuccessful}");
            });
        }
    }
}
