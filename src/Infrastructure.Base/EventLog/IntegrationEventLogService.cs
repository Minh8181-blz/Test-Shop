using Application.Base.SeedWork;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Base.EventLog
{
    public class IntegrationEventLogService<T> : IIntegrationEventLogService where T : IIntegrationEventDbContext
    {
        protected readonly IIntegrationEventDbContext _context;
        protected readonly IIntegrationEventBus _eventBus;

        public IntegrationEventLogService(T context, IIntegrationEventBus eventBus)
        {
            _context = context;
            _eventBus = eventBus;
        }

        public void SaveEvent(IntegrationEvent @event)
        {
            var eventLogEntry = new IntegrationEventLogEntry(@event);

            _context.EventLogEntries.Add(eventLogEntry);
        }

        public async Task PublishAsync(IntegrationEvent @event)
        {
            try
            {
                await MarkEventAsInProgressAsync(@event.Id);
                _eventBus.PublishEvent(@event);
                await MarkEventAsPublishedAsync(@event.Id);
            }
            catch (Exception ex)
            {
                await MarkEventAsFailedAsync(@event.Id);
            }
        }

        protected Task MarkEventAsPublishedAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.Published);
        }

        protected Task MarkEventAsInProgressAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.InProgress);
        }

        protected Task MarkEventAsFailedAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.PublishedFailed);
        }

        protected Task UpdateEventStatus(Guid eventId, EventStateEnum status)
        {
            var eventLogEntry = _context.EventLogEntries.Single(ie => ie.EventId == eventId);
            eventLogEntry.UpdateState(status);

            _context.EventLogEntries.Update(eventLogEntry);

            return _context.SaveChangesAsync();
        }
    }
}
