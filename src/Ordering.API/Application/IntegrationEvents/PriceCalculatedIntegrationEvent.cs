using Application.Base.SeedWork;

namespace Ordering.API.Application.IntegrationEvents
{
    public class PriceCalculatedIntegrationEvent : IntegrationEvent
    {
        public int OrderId { get; private set; }
        public decimal Total { get; private set; }
    }
}
