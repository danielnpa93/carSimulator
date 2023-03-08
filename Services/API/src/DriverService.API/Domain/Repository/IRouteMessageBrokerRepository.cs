using Route = DriverService.API.Domain.Entities.Route;

namespace DriverService.API.Domain.Repository
{
    public interface IRouteMessageBrokerRepository: IMessageBrokerRepository<string, byte[]>
    {
        Task ProduceAsync(Route message);
    }
}
