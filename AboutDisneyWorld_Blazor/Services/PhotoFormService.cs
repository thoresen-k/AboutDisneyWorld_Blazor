using AboutDisneyWorld_Blazor.Interfaces;
using AboutDisneyWorld_Blazor.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace AboutDisneyWorld_Blazor.Services;

public class PhotoFormService : IPhotoFormService
{
    public PhotoFormService()
    {
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
        PreviewImageUrl = $"data:{file.ContentType};base64,{Convert.ToBase64String(data)}";

        return new Photo{
            FileName = file.Name,
            ImageSrc = $"https://thoresen-disneyphotos.com/{file.Name}",
        };
    }

    public void ResetForm()
    {
        PreviewImageUrl = null;
        UploadedFile = null;
        IsProcessing = false;
    }
}