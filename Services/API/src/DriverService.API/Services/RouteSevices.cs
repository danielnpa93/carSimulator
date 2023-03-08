using AutoMapper;
using DriverService.API.Domain.DTO;
using DriverService.API.Domain.Repository;
using DriverService.API.Domain.Utils;
using DriverService.API.Services.Interfaces;
using Route = DriverService.API.Domain.Entities.Route;

namespace DriverService.API.Services
{
    public class RouteSevices : IRouteServices
    {
        private readonly IRouteRepository _routeRepository;
        private readonly IMapper _mapper;
        private readonly NotificationContext _notification;
        private readonly IRouteMessageBrokerRepository _messageBrokerRepository;

        public RouteSevices(IRouteRepository routeRepository,
                            IMapper mapper,
                            NotificationContext notification,
                            IRouteMessageBrokerRepository messageBrokerRepository
                            )
        {
            _routeRepository = routeRepository;
            _mapper = mapper;
            _notification = notification;
            _messageBrokerRepository = messageBrokerRepository;
        }

        public async Task<Guid> CreateRouteAsync(CreateRouteDTO model)
        {
            var alreadyHasRoute = await _routeRepository.GetRouteByTitle(model.Title);

            if (alreadyHasRoute != null)
            {
                _notification.AddNotification($"The {model.Title} already exists");
                return default;
            }

            var route = _mapper.Map<Route>(model);
            _routeRepository.CreateRouteAsync(_mapper.Map<Route>(route));

            return Guid.Parse(route.Id);
        }

        public async Task DeleteRouteAsync(string routeId)
        {
            var wasDeleted = await _routeRepository.DeleteRouteAsync(routeId);
            if (!wasDeleted)
            {
                _notification.AddNotification($"cannot find this route");
            }
        }

        public async Task<IEnumerable<Route>> GetAllRoutesAsync()
        {
            return await _routeRepository.GetAllRoutesAsync();
        }

        public async Task ProduceMessage(string routeId)
        {
            var route = await _routeRepository.GetRouteById(routeId);

            if (route == null)
            {
                _notification.AddNotification("Invalid Route");
                return;
            }

            await _messageBrokerRepository.ProduceAsync(route);
        }

        public async Task<Route> UpdateRouteAsync(UpdateRouteDTO model, string id)
        {
            if (model.Title != null)
            {
                var alreadyHasSameTitle = await _routeRepository.GetRouteByTitle(model.Title);
                if (alreadyHasSameTitle != null)
                {
                    _notification.AddNotification("Title route already exists");
                    return default;
                }
            }
            var route = await _routeRepository.GetRouteById(id);

            if (route == null)
            {
                _notification.AddNotification("route not exists");
                return default;
            }

            route.UpdateRoute(model.Title, model.StartPosition, model.EndPosition);

            var wasUpdated = await _routeRepository.UpdateRouteAsync(route);

            if (!wasUpdated)
            {
                _notification.AddNotification("route was not updated");
                return default;
            }

            return route;
        }
    }
}
