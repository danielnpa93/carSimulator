using DriverService.API.Controllers;
using DriverService.API.Domain.Repository;
using Microsoft.AspNetCore.SignalR;
using Serilog;
using System.Text.Json;

namespace DriverService.API.BackgroundServices
{
    public class TracingRouteConsumerService : BackgroundService
    {

        private readonly ITracingRouteMessageBrokerRepository _tracingRouteMessageBrokerRepository;
        private readonly IHubContext<RoutesHub> _hubContext;

        public TracingRouteConsumerService( ITracingRouteMessageBrokerRepository tracingRouteMessageBrokerRepository,
            IHubContext<RoutesHub> hubContext)
        {
            _tracingRouteMessageBrokerRepository = tracingRouteMessageBrokerRepository;
            _hubContext = hubContext;
        }


        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    _tracingRouteMessageBrokerRepository.ConsumeAsync(cancellationToken, DoSome);
                }
            }
            catch (OperationCanceledException ex)
            {
                Log.Information("Ending Consumer...");
            }
            catch(Exception ex)
            {
                Log.Information($"Some error occurred, ending consumer. Error: {ex.Message}");
            }
            //finally
            //{
            //   xx.Dispose();
            //}
        }

        private async void DoSome(string obj)
        {

            Log.Information(obj);

            var routes = JsonSerializer.Deserialize<Simulator.Schema.TracingRouteModel>(obj);
            await _hubContext.Clients.All.SendAsync("UpdateRoute",routes);

        }
   
    }
}
