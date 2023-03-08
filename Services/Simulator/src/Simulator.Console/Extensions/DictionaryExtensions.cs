using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simulator.Console.Extensions
{
    public static class DictionaryExtensions
    {
        public static string ToQueryString(this Dictionary<string, string> queriesCollection)
        {
            var queries = queriesCollection.Select(x => string.Format("{0}={1}",
                HttpUtility.UrlEncode(x.Key), HttpUtility.UrlEncode(x.Value))).ToArray();

            return "?" + string.Join("&", queries);
        }

    }
}
