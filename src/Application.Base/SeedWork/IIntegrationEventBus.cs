namespace Application.Base.SeedWork
{
    public interface IIntegrationEventBus
    {
        bool PublishEvent(IntegrationEvent @event);
        bool SubscribeEvent<T>() where T : IntegrationEvent;
    }
}
