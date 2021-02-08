using Domain.Base.SeedWork;

namespace Ordering.API.Domain.Exceptions
{
    public class PriceNegativeDomainException : DomainException
    {
        public PriceNegativeDomainException(decimal price) :
            base(string.Format($"Specified price is {price}"))
        {
            Price = price;
        }

        public decimal Price { get; }
    }
}
