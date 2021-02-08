using Domain.Base.SeedWork;
using Ordering.API.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Ordering.API.Domain.Events
{
    public class OrderCreatedDomainEvent : DomainEvent
    {
        public OrderCreatedDomainEvent(Order order)
        {
            Order = order;
        }

        public Order Order { get; }
    }
}
