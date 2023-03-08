using MongoDB.Bson.Serialization.Attributes;

namespace DriverService.API.Domain.Entities
{
    public class Position
    {
        [BsonElement("latitude")]
        public double Latitude { get; set; }

        [BsonElement("longitude")]
        public double Longitude { get; set; }
    }
}
