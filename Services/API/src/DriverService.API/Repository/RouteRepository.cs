using Route = DriverService.API.Domain.Entities.Route;
using DriverService.API.Domain.Repository;
using MongoDB.Driver;
using DriverService.API.Repository.Context;

namespace DriverService.API.Repository
{
    public class RouteRepository : IRouteRepository
    {
        private readonly MongoDbContext _mongoDbContext;

        public RouteRepository(MongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }
        public Guid CreateRouteAsync(Route route)
        {
            _mongoDbContext.Routes.InsertOne(route);
            return Guid.Parse(route.Id);
        }

        public async Task<bool> DeleteRouteAsync(string routeId)
        {
           var result = await _mongoDbContext.Routes.DeleteOneAsync(m => m.Id == routeId);
           return result.DeletedCount > 0;
        }

        public async Task<IEnumerable<Route>> GetAllRoutesAsync()
        {
            return await _mongoDbContext.Routes.Find(_ => true).ToListAsync();
        }

        public async Task<Route> GetRouteById(string id)
        {
            return await _mongoDbContext.Routes.Find(m => m.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Route> GetRouteByTitle(string title)
        {
           return await _mongoDbContext.Routes.Find(m => m.Title == title).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateRouteAsync(Route route)
        {
            var result = await _mongoDbContext.Routes.ReplaceOneAsync(x => x.Id == route.Id, route);
            return result.ModifiedCount > 0;
        }
    }
}
