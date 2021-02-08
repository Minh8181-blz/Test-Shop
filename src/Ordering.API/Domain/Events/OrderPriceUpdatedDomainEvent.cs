

using Domain.Base.SeedWork;

namespace Ordering.API.Domain.Events
{
    public class OrderPriceUpdatedDomainEvent : DomainEvent
    {
        public OrderPriceUpdatedDomainEvent(int orderId, decimal price)
        {
            OrderId = orderId;
            Price = price;
        }

        public int OrderId { get; }
        public decimal Price { get; }
    }
}
