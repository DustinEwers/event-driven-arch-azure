using GameBot.Web.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GameBot.Web
{
    public class TopicWatcher : BackgroundService
    {
        public IServiceProvider Services { get; }
        private readonly ILogger<TopicWatcher> _logger;

        public TopicWatcher(IServiceProvider services,
            ILogger<TopicWatcher> logger)
        {
            Services = services;
            _logger = logger;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await ListenToTopic(stoppingToken);
        }

        private async Task ListenToTopic(CancellationToken stoppingToken)
        {
            using var scope = Services.CreateScope();

            var userHub = scope.ServiceProvider.GetRequiredService<IHubContext<UserUpdateHub>>();
            var serviceBus = scope.ServiceProvider.GetRequiredService<IServiceBusHandler>();

            await serviceBus.SetupOrderUpdateTopicListener(async (orderResult) =>
            {
                await userHub.Clients.All.SendAsync("SendMessage", $"{orderResult.User}'s order number {orderResult.OrderId } for {orderResult.Coins} was {(orderResult.OrderSuccessful? "successful": "not successful")}", stoppingToken);
            });
        }
    }
}
