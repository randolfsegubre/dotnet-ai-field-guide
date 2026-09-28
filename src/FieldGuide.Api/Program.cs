using FieldGuide.Core.Chat;
using FieldGuide.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;

var builder = WebApplication.CreateBuilder(args);

// STEP 1 of 4: EF Core against SQL Server. Connection string in appsettings.json points at
// the default local instance with Windows Authentication (matches this machine's setup).
builder.Services.AddDbContext<FieldGuideDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FieldGuide")));

// STEP 2 of 4: the IChatClient, built once per request from config so the model/URL can
// change without a code change. See docs/topics/ichatclient-basics.md for why this
// wraps OllamaChatClientFactory rather than constructing OllamaApiClient directly here.
builder.Services.AddScoped<IChatClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var baseUrl = config["Ollama:BaseUrl"] ?? OllamaChatClientFactory.DefaultBaseUrl;
    var modelId = config["Ollama:ChatModelId"] ?? "llama3.1:8b";
    return OllamaChatClientFactory.Create(modelId, baseUrl);
});
builder.Services.AddScoped<ChatDemoService>();

// STEP 3 of 4: CORS for the React dev server (Vite's default port). Tightened to a named,
// explicit origin rather than AllowAnyOrigin, since this API will later hold real endpoints.
const string ReactDevCorsPolicy = "ReactDev";
builder.Services.AddCors(options =>
{
    options.AddPolicy(ReactDevCorsPolicy, policy =>
        policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// STEP 4 of 4: standard pipeline, CORS before auth/controllers.
app.UseHttpsRedirection();
app.UseCors(ReactDevCorsPolicy);
app.UseAuthorization();
app.MapControllers();

app.Run();
