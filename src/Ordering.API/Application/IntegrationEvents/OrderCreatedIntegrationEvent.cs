using Application.Base.SeedWork;
using Ordering.API.Domain.Entities;

namespace Ordering.API.Application.IntegrationEvents
{
    public class OrderCreatedIntegrationEvent : IntegrationEvent
    {
        public OrderCreatedIntegrationEvent(Order order) : base()
        {
            Order = order;
        }

        public Order Order { get; }
    }
}
