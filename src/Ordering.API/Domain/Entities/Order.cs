using Domain.Base.SeedWork;
using System;
using System.Collections.Generic;

namespace Ordering.API.Domain.Entities
{
    public class Order : Entity<int>, IAggregateRoot
    {
        public int CustomerId { get; private set; } 
        public DateTime CreatedAt { get; private set; }
        public DateTime LastUpdatedAt { get; private set; }
        public string Description { get; private set; }
        public string Status { get; private set; }
        public decimal Amount { get; private set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
    }
}
