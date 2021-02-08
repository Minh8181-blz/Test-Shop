using Domain.Base.SeedWork;
using Ordering.API.Domain.Enums;
using Ordering.API.Domain.Events;
using Ordering.API.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Ordering.API.Domain.Entities
{
    public class Order : Entity<int>, IAggregateRoot
    {
        protected Order() { }

        public static Order CreateOrder(int customerId, string description, IEnumerable<OrderItem> items)
        {
            var order = new Order
            {
                CustomerId = customerId,
                CreatedAt = DateTime.UtcNow,
                Description = description,
                LastUpdatedAt = DateTime.UtcNow,
                Status = OrderStatus.Pending.Id,
                OrderItems = new Collection<OrderItem>(items.ToList())
            };

            order.AddDomainEvent(new OrderCreatedDomainEvent(order));

            return order;
        }

        public int CustomerId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime LastUpdatedAt { get; private set; }
        public string Description { get; private set; }
        public int Status { get; private set; }
        public decimal Amount { get; private set; }
        public ICollection<OrderItem> OrderItems { get; private set; }

        public void UpdatePrice(decimal totalPrice)
        {
            if (totalPrice < 0)
            {
                throw new PriceNegativeDomainException(totalPrice);
            }

            Amount = totalPrice;
            LastUpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new OrderPriceUpdatedDomainEvent(Id, Amount));
        }
    }
}
