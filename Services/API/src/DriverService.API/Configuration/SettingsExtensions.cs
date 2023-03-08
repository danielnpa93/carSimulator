using DriverService.API.Domain.Utils;

namespace DriverService.API.Configuration
{
    public static class SettingsExtensions
    {
        public static Settings _settings = new Settings();
        public static void AddApplicationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            SetSettings(configuration);
            services.AddSingleton<ISettings>(_settings);
        }

        private static void SetSettings(IConfiguration config)
        {
            _settings.KAFKA_BOOTSTRAP_SERVERS = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP_SERVERS") ?? config.GetSection("Kafka")["KAFKA_BOOTSTRAP_SERVERS"];
            _settings.KAFKA_CONSUMER_GROUP_ID = Environment.GetEnvironmentVariable("KAFKA_CONSUMER_GROUP_ID") ?? config.GetSection("Kafka")["KAFKA_CONSUMER_GROUP_ID"];
            _settings.KAFKA_START_ROUTES_CONSUMER_TOPIC = Environment.GetEnvironmentVariable("KAFKA_START_ROUTES_CONSUMER_TOPIC") ?? config.GetSection("Kafka")["KAFKA_START_ROUTES_CONSUMER_TOPIC"];
            _settings.GOOGLE_DIRECTIONS_API_KEY = Environment.GetEnvironmentVariable("GOOGLE_DIRECTIONS_API_KEY") ?? config["GOOGLE_DIRECTIONS_API_KEY"];
            _settings.KAFKA_START_ROUTES_PRODUCER_TOPIC = Environment.GetEnvironmentVariable("KAFKA_START_ROUTES_PRODUCER_TOPIC")?? config.GetSection("Kafka")["KAFKA_START_ROUTES_PRODUCER_TOPIC"];
            _settings.MONGODB_CONNECTION_STRING = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING") ?? config.GetSection("MongoConnnection")["ConnectionString"];
            _settings.MONGODB_DATABASE = Environment.GetEnvironmentVariable("MONGODB_DATABASE") ?? config.GetSection("MongoConnnection")["Database"];
            _settings.MONGODB_IS_SSL =  config.GetSection("MongoConnnection").GetValue<bool>("IsSSL");
        }
    }
}
