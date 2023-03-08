using Confluent.Kafka;
using Simulator.Console.Models;
using System.Text.Json;
using Simulator.Schema;
using Simulator.Console.Clients;
using Simulator.Console.Kakfa.Producer;
using Route = Simulator.Console.Models.Route;
using System.Text;
using System.Linq;
using Serilog;
using System.Threading.Tasks;
using System;

namespace Simulator.Console.Kakfa.Consumer
{
    public class StartRoutesConsumer : BaseConsumer<string>
    {
        private readonly IGoogleDirectionsClient _googleClient;
        private readonly ProducerRoute _producerRoute;

        public StartRoutesConsumer(ISettings settings, IGoogleDirectionsClient googleClient,
                                   ProducerRoute producerRoute) :
        base(settings.KAFKA_START_ROUTES_CONSUMER_TOPIC, settings)
        {
            _googleClient = googleClient;
            _producerRoute = producerRoute;
        }

        protected override async Task ProcessRegister(ConsumeResult<Ignore, string> record)
        {
            try
            {

                //Log.Information($"Thread id (begin process method) {Thread.CurrentThread.ManagedThreadId}");

                var model = JsonSerializer.Deserialize<InitRouteModel>(record.Message.Value, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                var startRoute = new Route
                {
                    Latitude = model.StartRoute.Latitude,
                    Longitude = model.StartRoute.Longitude
                };

                var endRoute = new Route
                {
                    Longitude = model.EndRoute.Longitude,
                    Latitude = model.EndRoute.Latitude
                };

                var result = await _googleClient.GetTracingRoute(startRoute, endRoute);


                if (result == null)
                    return;

                Route last = result.Last();
                foreach (var item in result)
                {
                    var produceModel = new TracingRouteModel
                    {
                        CurrentRoute = new Schema.Route { Latitude = item.Latitude, Longitude = item.Longitude },
                        IsFinished = item.Equals(last),
                        OrderId = model.OrderId
                    };

                    var m = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(produceModel));

                    await Task.Delay(5000);
                    await _producerRoute.ProduceMessage<string, byte[]>(model.OrderId, m);

                }


            }
            catch(Exception e)
            {
                Log.Error($"Error occurred {e.Message}");
                //TODO Do something to not lose the registry
            }

        }
    }
}
