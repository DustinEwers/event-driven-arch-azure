using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace GameBot.Web.Hubs
{
    public class UserUpdateHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("SendMessage", message);
        }
    }
}
