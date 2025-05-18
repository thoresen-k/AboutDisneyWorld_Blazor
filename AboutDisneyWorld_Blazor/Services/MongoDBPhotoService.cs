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

    
    public async Task<Photo> GetPhotoByIdAsync(string id)
    {
        return await _photoCollection.Find(p => p.ID.Equals(id)).FirstOrDefaultAsync();
    }   
}