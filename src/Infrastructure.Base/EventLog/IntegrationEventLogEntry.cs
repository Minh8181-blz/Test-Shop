using Application.Base.SeedWork;
using Newtonsoft.Json;
using System;

namespace Infrastructure.Base.EventLog
{
    public class IntegrationEventLogEntry
    {
        public IntegrationEventLogEntry() { }

        public IntegrationEventLogEntry(IntegrationEvent @event)
        {
            EventId = @event.Id;
            EventCreationDate = @event.CreatedAt;
            EventTypeName = @event.GetType().FullName;
            Content = JsonConvert.SerializeObject(@event);
            State = EventStateEnum.NotPublished.ToString();
            CreatedAt = DateTime.UtcNow;
            LastModifiedAt = DateTime.UtcNow;
        }

        public Guid EventId { get; private set; }
        public string EventTypeName { get; private set; }
        public string State { get; private set; }
        public DateTime EventCreationDate { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime LastModifiedAt { get; private set; }
        public string Content { get; private set; }

        public void UpdateState(EventStateEnum state)
        {
            State = state.ToString();
            LastModifiedAt = DateTime.UtcNow;
        }
    }
}
