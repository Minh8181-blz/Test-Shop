using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Base.EventBus
{
    public class RabbitMqQueueModel
    {
        public string Exchange { get; }
        public string RoutingKey { get; }
        public string Queue { get; }

        public RabbitMqQueueModel(string exchange, string routingKey, string queue)
        {
            Exchange = exchange;
            RoutingKey = routingKey;
            Queue = queue;
        }
    }
}
