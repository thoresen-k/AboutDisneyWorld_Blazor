using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace AboutDisneyWorld_Blazor.Models
{
    public class Photo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; } = string.Empty;

        [Required(ErrorMessage = "A file upload is required")]
        public string FileName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Photo title is required")]
        public string Title { get; set; } = string.Empty;

        public string Caption { get; set; } = string.Empty;
        
        public string ImageSrc { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}