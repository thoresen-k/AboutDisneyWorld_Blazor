using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AboutDisneyWorld_Blazor.Models
{
    public class Photo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string ID { get; init; }

        public required string Name { get; init; }

        public required string  Title { get; init; }

        public required string Caption { get; init; }

        public DateTime Date { get; init; }
    }
}