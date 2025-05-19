using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace AboutDisneyWorld_Blazor.Models
{
    public class Photo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Caption { get; set; } = string.Empty;

        [JsonIgnore]
        public byte[] ImageData { get; set; } = [];

        [JsonIgnore]
        public byte[] PreviewData { get; set; } = [];

        public string ContentType { get; set; } = string.Empty;

        public string ImageSrc { get; set; } = string.Empty;
        //public string PreviewImageSrc => $"data:{ContentType};base64,{Convert.ToBase64String(PreviewData)}";

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }

    public class UploadResult
    {
        public string Id { get; set; } = string.Empty;
    }

    public class PhotoUploadDto
    {
        public string Title { get; set; } = string.Empty;

        public string Caption { get; set; } = string.Empty;

        public IFormFile File { get; set; } = null!;
    }
}