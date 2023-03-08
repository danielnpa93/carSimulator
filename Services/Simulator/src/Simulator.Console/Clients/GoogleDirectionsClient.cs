using Simulator.Console.Clients;
using Simulator.Console.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Simulator.Console.Client
{
    public class GoogleDirectionsClient : BaseClient, IGoogleDirectionsClient
    {
        private readonly ISettings _settings;

        public GoogleDirectionsClient(IHttpClientFactory httpClient, ISettings settings) : base(httpClient.CreateClient("googleClient"))
        {
            _settings = settings;
        }

        public async Task<IEnumerable<Route>> GetTracingRoute(Route startRoute
            , Route endRoute)
        {
            var parameters = new Dictionary<string, string>
            {
                {"destination", string.Format("{0},{1}",endRoute.Latitude, endRoute.Longitude)  },
                {"origin", string.Format("{0},{1}",startRoute.Latitude, startRoute.Longitude)  },
                {"key", _settings.GOOGLE_DIRECTIONS_API_KEY },

            };

            var result = await base.GetAsync<Root>("", parameters);

            if (result is null || !result.Routes.Any())
            {
                return default;
            }

            var routes = new List<Route>();
            routes.Add(startRoute);

            foreach(var item in result.Routes.First().Legs.First().Steps)
            {
                routes.Add(new Route
                {
                    Latitude = item.EndLocation.Lat.ToString(CultureInfo.InvariantCulture),
                    Longitude = item.EndLocation.Lng.ToString(CultureInfo.InvariantCulture),

                });
            }

            return routes;
        }
    }
}
