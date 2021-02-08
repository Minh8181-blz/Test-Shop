using Ordering.API.Domain.Entities;
using Ordering.API.Domain.Services.Models;
using System;
using System.Collections.Generic;

namespace Ordering.API.Domain.Services
{
    public class OrderDomainService : IOrderDomainService
    {
        public decimal GetOrderTotalAmount(IEnumerable<OrderItem> orderItems, IEnumerable<Product> products)
        {
            var calculator = new OrderTotalAmountCalculateModel(orderItems, products);

            return calculator.GetTotalPrice();
        }
    }
}
