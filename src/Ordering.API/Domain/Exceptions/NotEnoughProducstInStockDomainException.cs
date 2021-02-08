using Domain.Base.SeedWork;

namespace Ordering.API.Domain.Exceptions
{
    public class NotEnoughProducstInStockDomainException : DomainException
    {
        public NotEnoughProducstInStockDomainException(int orderedQuantity, int stockQuantity) :
            base(string.Format($"Ordered quantity is {orderedQuantity} which is greater than stock quantity of {stockQuantity}"))
        {
            OrderedQuantity = orderedQuantity;
            StockQuantity = stockQuantity;
        }

        public int OrderedQuantity { get; }
        public int StockQuantity { get; }
    }
}
