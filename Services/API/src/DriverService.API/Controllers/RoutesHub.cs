using DriverService.API.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace DriverService.API.Controllers
{
    public class RoutesHub : Hub
    {
        private readonly IRouteServices _routeServices;

        public RoutesHub(IRouteServices routeServices)
        {
            _routeServices = routeServices;
        }
        //public async Task SendMessage(string connectionId, string routeId)
        //{
        //   // await _routeServices.ProduceMessage(routeId);
        //   await Clients.All.SendAsync("UpdateRoute", "myMessage");
        //   // await Clients.Client(connectionId).SendAsync("update_route", "myMessage");
        //}


        public async Task SendMessage(string connectionId,string routeId)
        {
            await _routeServices.ProduceMessage(routeId);
           // await Clients.Client(connectionId).SendAsync("UpdateRoute", message);
            // await Clients.Client(connectionId).SendAsync("update_route", "myMessage");
        }
    }
}
