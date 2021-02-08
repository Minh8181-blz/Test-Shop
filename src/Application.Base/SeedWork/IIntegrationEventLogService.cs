using System;
using System.Threading.Tasks;

namespace Application.Base.SeedWork
{
    public interface IIntegrationEventLogService
    {
        void SaveEvent(IntegrationEvent @event);
        Task PublishAsync(IntegrationEvent @event);
    }
}
