using DriverService.API.Domain.DTO;
using DriverService.API.Domain.Utils;
using DriverService.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EntityRoute = DriverService.API.Domain.Entities.Route;

namespace DriverService.API.Controllers
{
    [Route("/api/v1/routes")]
    public class RouteController : BaseController
    {
        private readonly IRouteServices _routeServices;

        public RouteController(NotificationContext notificationContext,
                               IRouteServices routeServices) 
                               : base(notificationContext)
        {
            _routeServices = routeServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _routeServices.GetAllRoutesAsync();

            return CustomResponse(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRouteAsync(CreateRouteDTO route)
        {
            var result = await _routeServices.CreateRouteAsync(route);

            return CustomResponse(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRouteAsync(string routeId)
        {
            await _routeServices.DeleteRouteAsync(routeId);

            return CustomResponse();

        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateRouteAsync(string id, UpdateRouteDTO route)
        {
            var result = await _routeServices.UpdateRouteAsync(route, id);

            return CustomResponse(result);
        }

        [HttpPost("produceMessage")]
        public async Task<IActionResult> ProduceMessage(string routeId)
        {
            await _routeServices.ProduceMessage(routeId);

            return CustomResponse();
        }
    }
}
