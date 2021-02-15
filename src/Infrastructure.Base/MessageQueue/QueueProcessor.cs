using Infrastructure.Base.Newtonsoft;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Base.MessageQueue
{
    public class QueueProcessor : IQueueProcessor
    {
        private readonly IQueueConnectionFactory _factory;
        private IConnection _connection;
        private readonly Dictionary<string, IModel> Channels;

        public QueueProcessor(IQueueConnectionFactory factory)
        {
            _factory = factory;
            Channels = new Dictionary<string, IModel>();
        }

        public bool PublishMessageToExchange(string exchange, string routingKey, object message)
        {
            var channel = GetChannel(exchange, routingKey);

            channel.ExchangeDeclare(exchange: exchange, type: "topic", durable: true);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: exchange,
                                 routingKey: routingKey,
                                 basicProperties: properties,
                                 body: body);

            return true;
        }

        public bool SubscribeQueueToExchange<T>(string exchange, string routingKey, string queue, Func<T, bool> onMessageReceived)
        {
            var channel = GetChannel(exchange, routingKey);

            channel.ExchangeDeclare(exchange: exchange, type: "topic", durable: true);

            var queueName = channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false).QueueName;

            channel.QueueBind(queue: queueName, exchange: exchange, routingKey: routingKey);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var raw = Encoding.UTF8.GetString(ea.Body.ToArray());
                var body = JsonConvert.DeserializeObject<T>(raw, new JsonSerializerSettings()
                {
                    ContractResolver = new IntegrationEventResolverContract()
                });
                bool result = onMessageReceived.Invoke(body);

                if (result)
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                else
                    channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: true);    //todo
            };

            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

            return true;
        }

        public bool PublishPlainMessageToExchange(string exchange, string routingKey, string message)
        {
            var channel = GetChannel(exchange, routingKey);

            channel.ExchangeDeclare(exchange: exchange, type: "topic", durable: true);

            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: exchange,
                                 routingKey: routingKey,
                                 basicProperties: properties,
                                 body: body);

            return true;
        }

        private IConnection GetConnection()
        {
            if (_connection == null)
            {
                _connection = _factory.GetConnection();
            }

            return _connection;
        }

        private string GetChannelKey(string exchange, string routingKey)
        {
            return string.Format("{0}.{1}", exchange, routingKey);
        }

        private IModel GetChannel(string exchange, string routingKey)
        {
            string key = GetChannelKey(exchange, routingKey);

            if (!Channels.ContainsKey(key))
            {
                var connection = GetConnection();

                var channel = connection.CreateModel();

                Channels.Add(key, channel);
            }

            return Channels[key];
        }

        public byte[] ToByteArray<T>(T obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public T FromByteArray<T>(byte[] data)
        {
            if (data == null)
                return default;

            BinaryFormatter bf = new BinaryFormatter();

            using MemoryStream ms = new MemoryStream(data);
            object obj = bf.Deserialize(ms);

            return (T)obj;
        }
    }
}
