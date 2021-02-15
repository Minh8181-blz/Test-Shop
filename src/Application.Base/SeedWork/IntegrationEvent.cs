using Domain.Base.SeedWork;
using System;

namespace Application.Base.SeedWork
{
    public abstract class IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        public IntegrationEvent(Entity<int> entity, bool entityHasBeenCreated) : this()
        {
            EntityType = entity.GetType().FullName;
            EntityId = entity.Id;
            EntityHasBeenCreated = entityHasBeenCreated;
        }

        public Guid Id { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string EntityType { get; private set; }
        public int EntityId { get; private set; }
        public bool EntityHasBeenCreated { get; private set; }
        public Guid? PreviousEventId { get; private set; }
    }
}
