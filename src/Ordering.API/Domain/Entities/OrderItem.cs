using Domain.Base.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Domain.Entities
{
    public class OrderItem : Entity<int>
    {
        public static OrderItem CreateOrderItem(int productId, int quantity)
        {
            var item = new OrderItem
            {
                ProductId = productId,
                Quantity = quantity
            };

            return item;
        }

        public int OrderId { get; private set; }
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }
    }
}
