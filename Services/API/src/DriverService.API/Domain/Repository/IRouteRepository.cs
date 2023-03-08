using Route = DriverService.API.Domain.Entities.Route;

namespace DriverService.API.Domain.Repository
{
    public interface IRouteRepository
    {
        Task<Route> GetRouteByTitle(string title);
        Task<Route> GetRouteById(string id);
        Task<IEnumerable<Route>> GetAllRoutesAsync(); 
        Guid CreateRouteAsync(Route route);
        Task<bool> DeleteRouteAsync(string routeId);
        Task<bool> UpdateRouteAsync(Route route);
    }
}
