using AboutDisneyWorld_Blazor;
using AboutDisneyWorld_Blazor.Interfaces;
using AboutDisneyWorld_Blazor.Services;
using Amazon.S3;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<MongoDBPhotoService>();
builder.Services.AddSingleton<CloudflareR2Service>();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10485760; // 10 MB
});

builder.Services.AddBlazorBootstrap();
builder.Services.AddScoped<IPhotoFormService, PhotoFormService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();