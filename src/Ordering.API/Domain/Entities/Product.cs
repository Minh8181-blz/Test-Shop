using Domain.Base.SeedWork;
using Ordering.API.Domain.Events;
using Ordering.API.Domain.Exceptions;

namespace Ordering.API.Domain.Entities
{
    public class Product : Entity<int>, IAggregateRoot
    {
        public void SubtractFromStock(OrderItem orderItem)
        {
            if (orderItem == null)
            {
                throw new DomainException("Order item not available");
            }

            if (orderItem.ProductId != Id)
            {
                throw new DomainException($"Order item is for product having id {orderItem.ProductId} while product id is {Id}");
            }

            if (orderItem.Quantity > StockQuantity)
            {
                throw new NotEnoughProducstInStockDomainException(orderItem.Quantity, StockQuantity);
            }

            int previousQuantity = StockQuantity;
            StockQuantity -= orderItem.Quantity;

            AddDomainEvent(new ProductInStockChangedDomainEvent(Id, previousQuantity, StockQuantity));
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int StockQuantity { get; private set; }
    }
}
