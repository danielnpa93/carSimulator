using Confluent.Kafka;
using Serilog;
using Simulator.Console.Models;
using System;
using System.Threading.Tasks;

namespace Simulator.Console.Kakfa.Producer
{
    public abstract class BaseProducer
    {
        private readonly string _topic;
        protected ProducerConfig ProducerConfig { get; set; }

        public BaseProducer(string topic, ISettings settings)
        {
            _topic = topic;
            ProducerConfig = new ProducerConfig
            {
                BootstrapServers = settings.KAFKA_BOOTSTRAP_SERVERS,
            };

        }

        public virtual async Task ProduceMessage<TKey, TValue>(TKey key, TValue registry)
        {
            using (var producer = new ProducerBuilder<TKey, TValue>(ProducerConfig).Build())
            {
                try
                {
                    var result = await producer.ProduceAsync(_topic, new Message<TKey, TValue> { Key = key, Value = registry });
                    Log.Information($"Message Delivered. [TOPIC] {_topic} [KEY]:{key.ToString()}");
                }
                catch (Exception ex)
                {
                    Log.Information($"Failed to delivery on topic {_topic}: {ex.Message}");
                }

            }
        }


    }
}
