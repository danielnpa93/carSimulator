using Confluent.Kafka;
using Simulator.Console.Models;
using System.Threading.Tasks;

namespace Simulator.Console.Kakfa.Producer
{
    public class ProducerRoute : BaseProducer
    {
        public ProducerRoute(ISettings settings) : base(settings.KAFKA_START_ROUTES_PRODUCER_TOPIC, settings)
        {
        }
      
    }
}
