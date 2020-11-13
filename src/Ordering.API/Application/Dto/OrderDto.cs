using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Application.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedAt { get;  set; }
        public DateTime LastUpdatedAt { get;  set; }
        public string Description { get;  set; }
        public string Status { get;  set; }
        public decimal Amount { get;  set; }
        public IEnumerable<OrderItemDto> OrderItems { get; set; }
    }
}
