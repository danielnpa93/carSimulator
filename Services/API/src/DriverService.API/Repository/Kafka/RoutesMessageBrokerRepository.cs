using AutoMapper;
using Confluent.Kafka;
using DriverService.API.Domain.Repository;
using DriverService.API.Domain.Utils;
using Serilog;
using Simulator.Schema;
using System.Text;
using System.Text.Json;
using Route = DriverService.API.Domain.Entities.Route;

namespace DriverService.API.Repository.Kafka
{
    public class RoutesMessageBrokerRepository : BaseKafkaMessageBrokerRepository<string, byte[]>, IRouteMessageBrokerRepository
    {
        private readonly IMapper _mapper;

        public RoutesMessageBrokerRepository(ISettings settings, IMapper mapper)
        : base(settings.KAFKA_START_ROUTES_PRODUCER_TOPIC, settings)
        {
            _mapper = mapper;
        }

        public async Task ProduceAsync(Route route)
        {
            var producerRoute = _mapper.Map<InitRouteModel>(route);
            var message = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(producerRoute));
            await this.ProduceAsync(Guid.NewGuid().ToString(), message);
        }

    }
}
