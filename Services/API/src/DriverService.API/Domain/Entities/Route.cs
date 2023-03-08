using MongoDB.Bson.Serialization.Attributes;

namespace DriverService.API.Domain.Entities
{
    public class Route
    {
        [BsonId]
        public string Id { get; private set; }

        [BsonElement("title")]
        public string Title { get; private set; }

        [BsonElement("startPosition")]
        public Position StartPosition { get; private set; }

        [BsonElement("endPosition")]
        public Position EndPosition { get; private set; }

        public Route(string title, Position startPosition, Position endPosition)
        {
            Id = Guid.NewGuid().ToString();
            Title = title;
            StartPosition = startPosition;
            EndPosition = endPosition;
        }

        public void UpdateRoute(string? title = null, 
                                Position? startPosition = null, 
                                Position? endPosition = null)
        {
            Title = title ?? Title;
            StartPosition = startPosition ?? StartPosition;
            EndPosition = endPosition ?? EndPosition;
        }
    }
}
