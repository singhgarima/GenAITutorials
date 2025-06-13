using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;

namespace SimpleChatApp;

public class ChatApplication
{
    private readonly IHost _host;

    public ChatApplication()
    {
        var hostBuilder = Host.CreateDefaultBuilder();
        hostBuilder.ConfigureAppConfiguration(cfg => { cfg.AddJsonFile("appsettings.json", false, true); });

        hostBuilder.ConfigureServices((context, services) =>
        {
            var chatEndpoint = new Uri(context.Configuration["Ollama:Endpoint"] ?? string.Empty);
            var model = context.Configuration["Ollama:ChatModel"] ?? string.Empty;
            services.AddOllamaChatCompletion(
                model,
                chatEndpoint
            );

            services.AddTransient(serviceProvider => new Kernel(serviceProvider));
        });

        _host = hostBuilder.Build();
    }

    public T GetService<T>() where T : notnull
    {
        return _host.Services.GetRequiredService<T>();
    }
}