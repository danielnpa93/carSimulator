using Confluent.Kafka;
using Simulator.Schema;

namespace DriverService.API.Domain.Repository
{
    public interface ITracingRouteMessageBrokerRepository: IMessageBrokerRepository<string, string>
    {
    }
}
