using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace DriverService.API.Configuration
{
    public static class LoggerConfig
    {
        private static readonly LogEventLevel _defaultLogLevel = LogEventLevel.Information;
        private static readonly LoggingLevelSwitch _loggingLevel = new LoggingLevelSwitch();
        public static void AddLoggerConfig(this IConfiguration configuration)
        {
           LoadLogLevel(configuration);

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = "DriverService -{0:yyyy.MM.dd}",
                    MinimumLogEventLevel = _loggingLevel.MinimumLevel,
                })
                .MinimumLevel.ControlledBy(_loggingLevel)
                .CreateLogger();
        }

    

        private static void LoadLogLevel(IConfiguration configuration)
        {
            var configLogLevel = configuration.GetSection("Logger").GetSection("LogLevel")["Default"];
            bool parsed = Enum.TryParse(configLogLevel, true, out LogEventLevel logLevel);

            _loggingLevel.MinimumLevel = parsed ? logLevel : _defaultLogLevel;
        }
    }
}