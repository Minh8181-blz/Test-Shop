using Application.Base.SeedWork;
using Infrastructure.Base.MessageQueue;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Ordering.API.Application.IntegrationEvents;

namespace Ordering.API.Application
{
    public class EventSubscriber
    {
        public static void RegisterIntegrationEventSubscription(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IIntegrationEventBus>();

            eventBus.SubscribeEvent<PriceCalculatedIntegrationEvent>();
        }
    }
}
