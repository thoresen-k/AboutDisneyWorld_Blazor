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

    public async Task<List<Photo>> GetAllAsync() =>
        await _photoCollection.Find(_ => true).ToListAsync();

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

    public async Task<Photo> GetPhotoByIdAsync(string id)
    {
        return await _photoCollection.Find(p => p.ID.Equals(id)).FirstOrDefaultAsync();
    }

    public async Task DeletePhotoByIdAsync(string id) =>
        await _photoCollection.DeleteOneAsync(p => p.ID.Equals(id));
}