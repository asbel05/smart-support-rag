using System.ClientModel;
using Azure.AI.OpenAI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using OpenAI.Chat;
using OpenAI.Embeddings;
using SmartSupportRag.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
