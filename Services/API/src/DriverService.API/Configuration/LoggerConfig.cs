using Serilog;
using Serilog.Core;
using Serilog.Events;

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