using Domain.Base.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Domain.Events
{
    public class ProductInStockChangedDomainEvent : DomainEvent
    {
        public ProductInStockChangedDomainEvent(int productId, int previousQuantity, int currentQuantity)
        {
            ProductId = productId;
            PreviousQuantity = previousQuantity;
            CurrentQuantity = currentQuantity;
        }

        public int ProductId { get; }
        public int PreviousQuantity { get; }
        public int CurrentQuantity { get; }
    }
}
