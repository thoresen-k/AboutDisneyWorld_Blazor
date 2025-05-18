using AboutDisneyWorld_Blazor.Models;
using AboutDisneyWorld_Blazor.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

[ApiController]
[Route("api/photo")]
public class PhotoController : ControllerBase
{
    private readonly MongoDBPhotoService _photoService;
    private readonly IMemoryCache _cache;
    const string cacheKeyPrefix = "Photo_";

    public PhotoController(MongoDBPhotoService photoService, IMemoryCache cache)
    {
        _photoService = photoService;
        _cache = cache;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllPhotos()
    {
        var photos = await _photoService.GetAllAsync();

        var result = photos.Select(p => new {
            p.ID,
            p.Title,
            p.Caption,
            p.Date,
            p.FileName,
            p.ContentType,
            p.ImageData,
            p.PreviewData
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPhoto(string id)
    {
        var cacheKey = "Photo_" + id;
        if (!_cache.TryGetValue(cacheKey, out Photo? photo))
        {
            photo = await _photoService.GetPhotoByIdAsync(id);
            if (photo == null || photo.ImageData == null)
                return NotFound();

            _cache.Set(cacheKey, photo, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });
        }

        return File(photo.ImageData, photo.ContentType);
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadPhoto([FromForm] IFormFile file, [FromForm] string title, [FromForm] string caption)
    {
        Console.WriteLine("UPLOAD endpoint hit");  // ✅ Confirm this shows up

        const long maxFileSize = 10 * 1024 * 1024; // 10 MB
        if (file.Length > maxFileSize)
        {
            return BadRequest("File size exceeds the 10 MB limit.");
        }

        // Base64 preview
        using var ms = new MemoryStream();
        await file.OpenReadStream().CopyToAsync(ms);
        var buffer = ms.ToArray();
        var base64 = Convert.ToBase64String(buffer);
        var contentType = string.IsNullOrWhiteSpace(file.ContentType) ? "image/jpeg" : file.ContentType;

        // Preview Data
        using var image = Image.Load(buffer);
        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Mode = ResizeMode.Max,
            Size = new Size(325, 0)
        }));

        using var msPreview = new MemoryStream();
        await image.SaveAsJpegAsync(msPreview);

        var previewData = msPreview.ToArray();
        var photo = new Photo
        {
            Title = title,
            Caption = caption,
            ContentType = file.ContentType,
            FileName = file.FileName,
            ImageData = buffer,
            PreviewData = previewData,
            Date = DateTime.UtcNow
        };

        Console.WriteLine($"Uploading: {title}, {caption}, {file.FileName}, size: {file.Length}");

        await _photoService.AddAsync(photo);

        // Cache original image and preview
        _cache.Set(cacheKeyPrefix + photo.ID, photo, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
        });

        return Ok(new { message = "Upload successful." });
        
    }
}