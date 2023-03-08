using Confluent.Kafka;
using DriverService.API.Domain.Repository;
using DriverService.API.Domain.Utils;
using Serilog;

namespace DriverService.API.Repository.Kafka
{
    public abstract class BaseKafkaMessageBrokerRepository<TKey, TValue> : IMessageBrokerRepository<TKey, TValue>, IDisposable
    {
        private readonly string _topic;
        protected ProducerConfig ProducerConfig { get; set; }
        protected ConsumerConfig ConsumerConfig { get; set; }

        private IProducer<TKey, TValue> _producer;

        private IConsumer<TKey, TValue> _consumer;
        protected IProducer<TKey, TValue> Producer => _producer ?? (_producer = new ProducerBuilder<TKey, TValue>(ProducerConfig).Build());
        protected IConsumer<TKey, TValue> Consumer => _consumer ?? (_consumer = new ConsumerBuilder<TKey, TValue>(ConsumerConfig).Build());

        public BaseKafkaMessageBrokerRepository(string topic, ISettings settings)
        {
            _topic = topic;

            var clientConfig = new ClientConfig
            {
                BootstrapServers = settings.KAFKA_BOOTSTRAP_SERVERS,
                ReconnectBackoffMs = 1,
            };


            ProducerConfig = new ProducerConfig(clientConfig)
            {
                MessageSendMaxRetries = 5,
                MessageTimeoutMs = 30000,
            };

            ConsumerConfig = new ConsumerConfig(clientConfig)
            {
                GroupId = "1",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

        }

        public async virtual Task ProduceAsync(TKey key, TValue message)
        {
            var result = await Producer.ProduceAsync(_topic, new Message<TKey, TValue> { Key = key, Value = message });
            Log.Information($"Message delivered. [TOPIC]: {_topic} | [PARTITION]: {result.Partition.Value} | [OFFSET]: {result.Offset.Value}");
        }

        public virtual void ConsumeAsync(CancellationToken cancellation, Action<TValue> handleMessage)
        {
            var cr = Consumer.Consume(cancellation);

            Log.Information($"Message received. [TOPIC]: {_topic} | [OFFSET]: {cr.Offset.Value} | [PARTITION] {cr.Partition.Value}");

            _ = Task.Run(() => handleMessage(cr.Value));

            Log.Information($"Register processed on topic: {_topic} - Offset: {cr.Offset.Value} - Chave: {cr.Message.Key} - Particao {cr.Partition}");
        }

        public void Dispose()
        {
            if(_producer!= null)
                Producer.Dispose();
            if(_consumer!= null)
                Consumer.Dispose();
        }


    }
}
