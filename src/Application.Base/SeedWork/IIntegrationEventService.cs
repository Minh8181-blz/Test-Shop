using System;
using System.Threading.Tasks;

namespace Application.Base.SeedWork
{
    public interface IIntegrationEventService
    {
        Task SaveEventAsync(IntegrationEvent @event);
        Task PublishAsync(IntegrationEvent @event);
    }
}
