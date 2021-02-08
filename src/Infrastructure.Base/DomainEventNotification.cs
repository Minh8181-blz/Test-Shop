using Domain.Base.SeedWork;
using MediatR;

namespace Infrastructure.Base
{
    public class DomainEventNotification<T> : INotification where T : DomainEvent
    {
        public DomainEventNotification(T data)
        {
            Data = data;
        }

        public T Data { get; }
    }
}
