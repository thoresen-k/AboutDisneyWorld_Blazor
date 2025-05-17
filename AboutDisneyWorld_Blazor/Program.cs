using AboutDisneyWorld_Blazor.Services;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages(); // ✅ Required for _Host.cshtml
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<MongoDBPhotoService>();
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10485760; // 10 MB
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting(); // ✅ Needed before MapBlazorHub and MapFallbackToPage
app.UseAntiforgery();

app.MapRazorPages();         // ✅ Required to support _Host.cshtml
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();