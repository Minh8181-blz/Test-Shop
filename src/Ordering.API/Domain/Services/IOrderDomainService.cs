using Ordering.API.Domain.Entities;
using System.Collections.Generic;

namespace Ordering.API.Domain.Services
{
    public interface IOrderDomainService
    {
        public decimal GetOrderTotalAmount(IEnumerable<OrderItem> orderItems, IEnumerable<Product> products);
    }
}
