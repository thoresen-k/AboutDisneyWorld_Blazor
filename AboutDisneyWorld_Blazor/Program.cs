using Microsoft.OpenApi.Models;
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

builder.Services.AddControllers();
builder.Services.AddHttpClient("ServerAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5109/"); // Replace with your app's actual base URL
});

// Also register a default HttpClient that uses the above named client
builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("ServerAPI"));

builder.Services.AddEndpointsApiExplorer(); // Enables Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
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


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.UseAntiforgery();

app.MapControllers();

app.MapRazorPages();         // ✅ Required to support _Host.cshtml
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();