using Confluent.Kafka;
using Serilog;
using Simulator.Console.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Simulator.Console.Kakfa.Consumer
{
    public abstract class BaseConsumer<TRecord>
    {
        private readonly string _topic;
        protected ConsumerConfig Config { get; set; }
        public BaseConsumer(string topic, ISettings settings)
        {
            _topic = topic;

            Config = new ConsumerConfig
            {
                BootstrapServers = settings.KAFKA_BOOTSTRAP_SERVERS,
                GroupId = settings.KAFKA_CONSUMER_GROUP_ID,
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };

        }

        public virtual void RunConsumerBroker(CancellationToken cancellationToken)
        {
            var consumer = new ConsumerBuilder<Ignore, TRecord>(Config).Build();

            Consume(consumer, cancellationToken);
        }

        private void Consume(IConsumer<Ignore, TRecord> consumer, CancellationToken cancellationToken)
        {
            Log.Information($"Preparing to consume on {_topic} topic");

            consumer.Subscribe(_topic);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var cr = consumer.Consume(cancellationToken);
                    Log.Information($"Register received on topic: {_topic} - Offset: {cr.Offset.Value} - Chave: {cr.Message.Key} - Particao {cr.Partition}");

                    _ = Task.Run(() => ProcessRegister(cr));

                    Log.Information($"Register processed on topic: {_topic} - Offset: {cr.Offset.Value} - Chave: {cr.Message.Key} - Particao {cr.Partition}");
                }

            }
            catch (OperationCanceledException _)
            {
                Log.Information("Ending Consumer...");
            }
            catch (Exception e)
            {
                Log.Information($"Some error occurred, ending consumer. Error: {e.Message}");
            }
            finally
            {
                consumer.Dispose();
            }
        }

        protected abstract Task ProcessRegister(ConsumeResult<Ignore, TRecord> record);
    }
}
