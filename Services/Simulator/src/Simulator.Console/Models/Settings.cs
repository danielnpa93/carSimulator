namespace Simulator.Console.Models
{
    public interface ISettings
    {
        string KAFKA_BOOTSTRAP_SERVERS { get; set; }
        string KAFKA_CONSUMER_GROUP_ID { get; set; }
        string KAFKA_START_ROUTES_CONSUMER_TOPIC { get; set; }
        string GOOGLE_DIRECTIONS_API_URL { get; set; }
        string GOOGLE_DIRECTIONS_API_KEY { get; set; }
        string KAFKA_START_ROUTES_PRODUCER_TOPIC {get; set;}
    }
    public class Settings : ISettings
    {
        public string KAFKA_BOOTSTRAP_SERVERS { get; set; }
        public string KAFKA_CONSUMER_GROUP_ID { get; set; }
        public string KAFKA_START_ROUTES_CONSUMER_TOPIC { get; set; }
        public string GOOGLE_DIRECTIONS_API_URL { get; set; }
        public string GOOGLE_DIRECTIONS_API_KEY { get; set; }
        public string KAFKA_START_ROUTES_PRODUCER_TOPIC { get; set; }
    }
}
