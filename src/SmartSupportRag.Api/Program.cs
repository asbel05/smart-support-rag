using System.ClientModel;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Azure.AI.OpenAI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using OpenAI.Chat;
using OpenAI.Embeddings;
using SmartSupportRag.Api.Data;
using SmartSupportRag.Api.Middleware;
using SmartSupportRag.Api.Repositories;
using SmartSupportRag.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddOpenApi();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.OnRejected = async (context, cancellationToken) =>
    {
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status429TooManyRequests,
            Title = "Límite de peticiones excedido",
            Detail = "Se alcanzó el máximo permitido de 6 peticiones por minuto. Intenta nuevamente en unos segundos.",
            Instance = context.HttpContext.Request.Path
        };

        await context.HttpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
    };

    options.AddPolicy("PerIpPolicy", httpContext =>
    {
        var clientIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: clientIp,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 6,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            });
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularClient", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'DefaultConnection'.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.UseVector();
    }));

var azureEndpoint = builder.Configuration["AzureOpenAI:Endpoint"]
    ?? throw new InvalidOperationException("No se configuró 'AzureOpenAI:Endpoint'.");
var azureApiKey = builder.Configuration["AzureOpenAI:ApiKey"]
    ?? throw new InvalidOperationException("No se configuró 'AzureOpenAI:ApiKey'.");
var chatDeployment = builder.Configuration["AzureOpenAI:ChatDeploymentName"] ?? "gpt-6-luna";
var embeddingDeployment = builder.Configuration["AzureOpenAI:EmbeddingDeploymentName"] ?? "text-embedding-3-small";

var azureClient = new AzureOpenAIClient(new Uri(azureEndpoint), new ApiKeyCredential(azureApiKey));

builder.Services.AddSingleton<IEmbeddingGenerator<string, Embedding<float>>>(
    azureClient.GetEmbeddingClient(embeddingDeployment).AsIEmbeddingGenerator());

builder.Services.AddSingleton<IChatClient>(
    azureClient.GetChatClient(chatDeployment).AsIChatClient());

builder.Services.AddSingleton<TextChunker>();
builder.Services.AddScoped<IPolicyRepository, PolicyRepository>();
builder.Services.AddScoped<IPolicyService, PolicyService>();
builder.Services.AddScoped<ISupportRagService, SupportRagService>();

var app = builder.Build();

app.UseExceptionHandler();

app.UseCors("AngularClient");

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseRateLimiter();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
