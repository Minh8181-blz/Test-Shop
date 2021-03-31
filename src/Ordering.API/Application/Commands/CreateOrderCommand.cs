using MediatR;
using Ordering.API.Application.Models;
using System;
using System.Collections.Generic;

namespace Ordering.API.Application.Commands
{
    public class CreateOrderCommand : IRequest<bool>
    {
        public CreateOrderCommand(int customerId, IEnumerable<OrderItemDto> orderItems, DateTime orderDate, string description)
        {
            CustomerId = customerId;
            OrderItems = orderItems;
            CreatedAt = orderDate;
            Description = description;
        }

        public int CustomerId { get; }
        public IEnumerable<OrderItemDto> OrderItems { get; }
        public DateTime CreatedAt { get; }
        public string Description { get; }
    }
}
