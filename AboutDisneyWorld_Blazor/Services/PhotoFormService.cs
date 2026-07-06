using AboutDisneyWorld_Blazor.Interfaces;
using AboutDisneyWorld_Blazor.Models;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AboutDisneyWorld_Blazor.Services;

public class PhotoFormService : IPhotoFormService
{
    private readonly MongoDBPhotoService _mongo;

    public PhotoFormService(MongoDBPhotoService mongo)
    {
        _mongo = mongo;
    }

    // Form state
    public string? PreviewImageUrl { get; set; }
    public IBrowserFile? UploadedFile { get; set; }
    public bool IsProcessing { get; set; }

    public async Task<Photo> HandleFileSelected(IBrowserFile file)
    {
        Console.WriteLine($"HandleFileSelected from PhotoFormService: {file.Name}, size: {file.Size}, content type: {file.ContentType}");
        if (file == null) throw new ArgumentNullException(nameof(file));

        UploadedFile = file;
        using var stream = file.OpenReadStream(file.Size);
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        var data = ms.ToArray();

        var preview = data;

        return new Photo{
            ImageData = data,
            PreviewData = data,
            ContentType = !string.IsNullOrWhiteSpace(file.ContentType) ? file.ContentType : "image/jpeg",
            FileName = file.Name
        };
    }

    public void ResetForm()
    {
        PreviewImageUrl = null;
        UploadedFile = null;
        IsProcessing = false;
    }
}