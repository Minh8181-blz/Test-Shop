using System.Collections.Generic;

namespace Ordering.API.ViewModels
{
    public class OrderViewModel
    {
        public int CustomerId { get; set; } // to be removed
        public List<OrderItemViewModel> OrderItems { get; set; }
        public string Description { get; set; }
    }
}
