using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Base.SeedWork
{
    public interface IDomainEventPublisher
    {
        Task Publish(DomainEvent @event);
        Task Publish(IEnumerable<DomainEvent> events);
    }
}
