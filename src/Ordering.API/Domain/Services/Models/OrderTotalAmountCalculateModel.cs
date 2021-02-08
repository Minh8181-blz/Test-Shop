using Domain.Base.SeedWork;
using Ordering.API.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Ordering.API.Domain.Services.Models
{
    public class OrderTotalAmountCalculateModel
    {
        private readonly IEnumerable<OrderItem> OrderItems;
        private readonly IEnumerable<Product> Products;

        public OrderTotalAmountCalculateModel(IEnumerable<OrderItem> orderItems, IEnumerable<Product> products)
        {
            OrderItems = orderItems ?? throw new DomainException("Order items are not available");
            Products = products ?? throw new DomainException("Products are not available");
        }

        public decimal GetTotalPrice()
        {
            var price = Products.Sum(x => x.Price * OrderItems.First(y => y.ProductId == x.Id).Quantity);

            return price;
        }
    }
}
