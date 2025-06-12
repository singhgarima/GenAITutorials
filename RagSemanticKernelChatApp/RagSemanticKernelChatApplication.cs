using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

            var starWarBaseUrl = new Uri(context.Configuration["DataSource:Endpoint"] ?? string.Empty);
            services.AddHttpClient<IStarWarCharacterService, StarWarCharacterService>(c =>
                c.BaseAddress = starWarBaseUrl);
        });

        _host = hostBuilder.Build();
    }

    public T GetService<T>() where T : notnull
    {
        return _host.Services.GetRequiredService<T>();
    }
}