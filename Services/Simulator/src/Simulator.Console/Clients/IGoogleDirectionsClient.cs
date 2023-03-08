using Simulator.Console.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simulator.Console.Clients
{
    public interface IGoogleDirectionsClient
    {
        Task<IEnumerable<Route>> GetTracingRoute(Route startRoute, Route endRoute);
    }
}
