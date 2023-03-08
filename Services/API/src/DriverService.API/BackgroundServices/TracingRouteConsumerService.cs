using DriverService.API.Domain.Repository;
using DriverService.API.Domain.Utils;
using Serilog;

namespace DriverService.API.BackgroundServices
{
    public class TracingRouteConsumerService : BackgroundService
    {

        private readonly string _topic;
        private readonly ITracingRouteMessageBrokerRepository _tracingRouteMessageBrokerRepository;

        public TracingRouteConsumerService(ISettings settings, ITracingRouteMessageBrokerRepository tracingRouteMessageBrokerRepository)
        {
            _topic = settings.KAFKA_START_ROUTES_CONSUMER_TOPIC;
            _tracingRouteMessageBrokerRepository = tracingRouteMessageBrokerRepository;
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

        private void DoSome(string obj)
        {
            Log.Information($"Processando o registo {obj}");
        }
   
    }
}
