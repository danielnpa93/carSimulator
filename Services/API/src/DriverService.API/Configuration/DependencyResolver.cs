using DriverService.API.Domain.Repository;
using DriverService.API.Domain.Utils;
using DriverService.API.Repository;
using DriverService.API.Repository.Context;
using DriverService.API.Repository.Kafka;
using DriverService.API.Services;
using DriverService.API.Services.Interfaces;

namespace DriverService.API.Configuration
{
    public static class DependencyResolver
    {
        public static void AddDependencies(this IServiceCollection services)
        {
            services.AddSignalR();

            services.AddScoped<IRouteServices, RouteSevices>();
            services.AddScoped<IRouteRepository, RouteRepository>();

            services.AddScoped<NotificationContext>();

            services.AddAutoMapper(typeof(DependencyResolver).Assembly);

            services.AddScoped<MongoDbContext>();

            services.AddScoped<IRouteMessageBrokerRepository, RoutesMessageBrokerRepository>();

            services.AddSingleton<ITracingRouteMessageBrokerRepository, TracingRouteMessageBrokerRepository>();
        }

    }
}
