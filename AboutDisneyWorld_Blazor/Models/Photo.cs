using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AboutDisneyWorld_Blazor.Models
{
    public class Photo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ID { get; set; }

        public string Name { get; set; }

        public string  Title { get; set; }

        public string Caption { get; set; }

        public DateTime Date { get; set; }
    }
}