namespace Infrastructure.Base.EventBus
{
    public class RabbitMqTopicModel
    {
        public string Exchange { get; }
        public string RoutingKey { get; }

        public RabbitMqTopicModel(string exchange, string routingKey)
        {
            Exchange = exchange;
            RoutingKey = routingKey;
        }
    }
}
