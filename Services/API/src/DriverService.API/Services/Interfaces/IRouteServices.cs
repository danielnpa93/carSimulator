using DriverService.API.Domain.DTO;
using Route = DriverService.API.Domain.Entities.Route;

namespace DriverService.API.Services.Interfaces
{
    public interface IRouteServices
    {
        Task<IEnumerable<Route>> GetAllRoutesAsync();
        Task<Guid> CreateRouteAsync(CreateRouteDTO route);
        Task DeleteRouteAsync(string routeId);
        Task<Route> UpdateRouteAsync(UpdateRouteDTO route, string id);
        Task ProduceMessage(string routeId);
    }
}
