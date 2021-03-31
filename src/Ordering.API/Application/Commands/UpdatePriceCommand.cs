using MediatR;

namespace Ordering.API.Application.Commands
{
    public class UpdatePriceCommand : IRequest<bool>
    {
        public UpdatePriceCommand(int orderId, decimal total)
        {
            OrderId = orderId;
            Total = total;
        }

        public int OrderId { get; }
        public decimal Total { get; }
    }
}
