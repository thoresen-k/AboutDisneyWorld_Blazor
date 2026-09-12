using AboutDisneyWorld_Blazor.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace AboutDisneyWorld_Blazor.Interfaces;

public interface IPhotoFormService
{    
    // Form state management
    string? PreviewImageUrl { get; set; }
    IBrowserFile? UploadedFile { get; set; }
    bool IsProcessing { get; set; }
    
    // Form operations
    Task<Photo> HandleFileSelected(IBrowserFile file);
    void ResetForm();
}