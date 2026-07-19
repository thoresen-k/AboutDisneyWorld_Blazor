using AboutDisneyWorld_Blazor.Models;
using MongoDB.Driver;

namespace AboutDisneyWorld_Blazor.Services;

public class MongoDBPhotoService
{
    private readonly IMongoCollection<Photo> _photoCollection;

    public MongoDBPhotoService(IConfiguration config)
    {
        var client = new MongoClient(config["MongoDB:ConnectionURI"]);
        var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
        _photoCollection = database.GetCollection<Photo>(config["MongoDB:CollectionName"]);
    }

    public async Task<(List<Photo> photos, bool Success)> GetAllAsync()
    {
        List<Photo> photos = [];
        bool success = false;

        try
        {
            var projection = Builders<Photo>.Projection
                .Exclude(p => p.ImageData)
                .Exclude(p => p.PreviewData);

            photos = await _photoCollection
                .Find(_ => true)
                .SortBy(p => p.Date)
                .Project<Photo>(projection)
                .ToListAsync();

            success = true;
        }
        catch (MongoException e)
        {
            Console.WriteLine($"Mongo Exception: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e.Message}");
        }

        return (photos, success);
    }

    public async Task AddAsync(Photo item) =>
        await _photoCollection.InsertOneAsync(item);

    public async Task UpdateAsync(Photo item)
    {
        var filter = Builders<Photo>.Filter.Eq(p => p.ID, item.ID);

        var update = Builders<Photo>.Update
                                    .Set(p => p.Title, item.Title)
                                    .Set(p => p.Caption, item.Caption)
                                    .Set(p => p.FileName, item.FileName)
                                    .Set(p => p.ImageData, item.ImageData)
                                    .Set(p => p.PreviewData, item.PreviewData)
                                    .Set(p => p.ContentType, item.ContentType);

        await _photoCollection.UpdateOneAsync(filter, update);
    }

    public async Task<(Photo photo, bool Success)> GetPhotoByIdAsync(string id)
    {
        Photo photo = new();
        bool success = false;

        try
        {
            var projection = Builders<Photo>.Projection
                .Exclude(p => p.ImageData)
                .Exclude(p => p.PreviewData);

            photo = await _photoCollection
                .Find(p => p.ID.Equals(id))
                .Project<Photo>(projection)
                .FirstOrDefaultAsync();

            success = true;
        }
        catch (MongoException e)
        {
            Console.WriteLine($"Mongo Exception: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e.Message}");
        }

        return (photo, success);
    }

    public async Task DeletePhotoByIdAsync(string id) =>
        await _photoCollection.DeleteOneAsync(p => p.ID.Equals(id));
}