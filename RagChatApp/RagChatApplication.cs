using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RagChatApp.Infrastructure;

namespace RagChatApp;

public class RagChatApplication
{
    private readonly IHost _host;

    public RagChatApplication()
    {
        var hostBuilder = Host.CreateDefaultBuilder();
        hostBuilder.ConfigureAppConfiguration(cfg => { cfg.AddJsonFile("appsettings.json", false, true); });

        hostBuilder.ConfigureServices((context, services) =>
        {
            services.AddSingleton(context.Configuration);
            var ollamaBaseUrl = new Uri(context.Configuration["Ollama:Endpoint"] ?? string.Empty);
            services.AddHttpClient<IEmbeddingProvider, OllamaEmbeddingProvider>(c =>
                c.BaseAddress = ollamaBaseUrl);
            services.AddSingleton<IVectorDatabase, QdrantVectorDatabase>();

            services.AddHttpClient<ILlmProvider, OllamaLlmProvider>(c =>
                c.BaseAddress = ollamaBaseUrl);

            services.AddSingleton<RagQuoteAssistant>();
        });

        _host = hostBuilder.Build();
    }

    public T GetService<T>() where T : notnull
    {
        return _host.Services.GetRequiredService<T>();
    }
}