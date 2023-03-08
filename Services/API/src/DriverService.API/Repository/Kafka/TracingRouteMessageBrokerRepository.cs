using DriverService.API.Domain.Repository;
using DriverService.API.Domain.Utils;

namespace DriverService.API.Repository.Kafka
{
    public class TracingRouteMessageBrokerRepository : BaseKafkaMessageBrokerRepository<string, string>, ITracingRouteMessageBrokerRepository
    {
        public TracingRouteMessageBrokerRepository(ISettings settings)
        : base(settings.KAFKA_START_ROUTES_CONSUMER_TOPIC, settings)
        {
            this.Consumer.Subscribe(settings.KAFKA_START_ROUTES_CONSUMER_TOPIC);
        }

    }
}
