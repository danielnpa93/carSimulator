using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using Simulator.Console.Client;
using Simulator.Console.Clients;
using Simulator.Console.Kakfa.Consumer;
using Simulator.Console.Kakfa.Producer;
using Simulator.Console.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Simulator.Console.Extensions
{
    public static class DependencyResolver
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<ISettings, Settings>(_ => GetSettings(config));
            services.AddSingleton<StartRoutesConsumer>();
            services.AddSingleton<IGoogleDirectionsClient, GoogleDirectionsClient>();
            services.AddSingleton<ProducerRoute>();

            services.AddHttpClient<GoogleDirectionsClient>("googleClient", client =>
            {
                client.BaseAddress = new Uri(config["GOOGLE_DIRECTIONS_API_URL"]);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            })
             .SetHandlerLifetime(TimeSpan.FromSeconds(30))
             .AddPolicyHandler(GetRetryPolicy())
             .AddPolicyHandler(GetCircuitBreakerPolicy());
        }

        private static Settings GetSettings(IConfiguration config)
        {
            return new Settings
            {
                KAFKA_BOOTSTRAP_SERVERS = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP_SERVERS") ?? config.GetSection("Kafka")["KAFKA_BOOTSTRAP_SERVERS"],
                KAFKA_CONSUMER_GROUP_ID = Environment.GetEnvironmentVariable("KAFKA_CONSUMER_GROUP_ID") ?? config.GetSection("Kafka")["KAFKA_CONSUMER_GROUP_ID"],
                KAFKA_START_ROUTES_CONSUMER_TOPIC = Environment.GetEnvironmentVariable("KAFKA_START_ROUTES_CONSUMER_TOPIC") ?? config.GetSection("Kafka")["KAFKA_START_ROUTES_CONSUMER_TOPIC"],
                GOOGLE_DIRECTIONS_API_KEY = Environment.GetEnvironmentVariable("GOOGLE_DIRECTIONS_API_KEY") ?? config["GOOGLE_DIRECTIONS_API_KEY"],
                KAFKA_START_ROUTES_PRODUCER_TOPIC = Environment.GetEnvironmentVariable("KAFKA_START_ROUTES_PRODUCER_TOPIC") ?? config.GetSection("Kafka")["KAFKA_START_ROUTES_PRODUCER_TOPIC"]
            };

        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt =>
                {
                    Log.Warning($"[Retrying policy] {retryAttempt}");
                    return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
                });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return Policy
               .HandleResult<HttpResponseMessage>(r => r.StatusCode == System.Net.HttpStatusCode.InternalServerError)
               .CircuitBreakerAsync(3,
               TimeSpan.FromSeconds(30),
               onBreak: (result, timeSpan) =>
               {
                   Log.Error("---------OPEN CIRCUIT---------");
               },
               onReset: () =>
               {
                   Log.Warning("---------RESETED CIRCUIT---------");
               },
               onHalfOpen: () =>
               {
                   Log.Information("-------OPEN CIRCUIT--------");
               });
        }
    }
}
