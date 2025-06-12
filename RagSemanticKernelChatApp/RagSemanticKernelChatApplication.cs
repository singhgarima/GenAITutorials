using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;
using RagSemanticKernelChatApp.Infrastructure;

namespace RagSemanticKernelChatApp;

public class RagSemanticKernelChatApplication
{
    private readonly IHost _host;

    public RagSemanticKernelChatApplication()
    {
        var hostBuilder = Host.CreateDefaultBuilder();
        hostBuilder.ConfigureAppConfiguration(cfg => { cfg.AddJsonFile("appsettings.json", false, true); });

        hostBuilder.ConfigureServices((context, services) =>
        {
            services.AddSingleton(context.Configuration);
            RegisterDataSourceService(context.Configuration, services);
            RegisterEmbeddingGenerator(context.Configuration, services);
            RegisterVectorDb(context.Configuration, services);
            RegisterKernel(services);
        });

        _host = hostBuilder.Build();
    }

    private static void RegisterDataSourceService(IConfiguration config, IServiceCollection services)
    {
        var starWarBaseUrl = new Uri(config["DataSource:Endpoint"] ?? string.Empty);
        services.AddHttpClient<IStarWarCharacterService, StarWarCharacterService>(c =>
            c.BaseAddress = starWarBaseUrl);
    }

    private static void RegisterEmbeddingGenerator(IConfiguration config, IServiceCollection services)
    {
        var embeddingEndpoint = new Uri(config["Ollama:Endpoint"] ?? string.Empty);
        var model = config["Ollama:EmbeddingModel"] ?? string.Empty;
        services.AddOllamaEmbeddingGenerator(model, embeddingEndpoint);
    }

    private void RegisterVectorDb(IConfiguration config, IServiceCollection services)
    {
        services.AddQdrantVectorStore(
            config["Qdrant:Endpoint"] ?? "",
            int.Parse(config["Qdrant:Port"] ?? "6334"),
            false
        );
    }

    private void RegisterKernel(IServiceCollection services)
    {
        services.AddTransient(serviceProvider => new Kernel(serviceProvider));
    }

    public T GetService<T>() where T : notnull
    {
        return _host.Services.GetRequiredService<T>();
    }
}