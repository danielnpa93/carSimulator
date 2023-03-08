using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Simulator.Console.Extensions;
using Simulator.Console.Kakfa.Consumer;
using System.Threading;
using System.Threading.Tasks;

namespace Simulator.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json")
           .AddJsonFile("appsettings.Development.json")
           .AddEnvironmentVariables()
           .Build();

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();

            var cts = new CancellationTokenSource();
            System.Console.CancelKeyPress += (s, e) =>
            {
                Log.Information("Canceling...");
                cts.Cancel();
                e.Cancel = true;

            };

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDependencies(config);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            serviceProvider
                .GetService<StartRoutesConsumer>()
                .RunConsumerBroker(cts.Token);

        }

    }
}
