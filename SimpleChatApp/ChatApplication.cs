using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SimpleChatApp;

public class ChatApplication
{
    private readonly IHost _host;

    public ChatApplication()
    {
        var hostBuilder = Host.CreateDefaultBuilder();
        hostBuilder.ConfigureAppConfiguration(cfg =>
        {
            cfg.AddJsonFile("appsettings.json", false, true);
        });

        hostBuilder.ConfigureServices((context, services) =>
        {
            services.AddSingleton(context.Configuration);
            var ollamaBaseUrl = new Uri(context.Configuration["Ollama:Endpoint"] ?? string.Empty);
            services.AddHttpClient<IChatClient, OllamaChatClient>(c =>
                c.BaseAddress = ollamaBaseUrl);
        });

        _host = hostBuilder.Build();
    }

    public T GetService<T>() where T : notnull
    {
        return _host.Services.GetRequiredService<T>();
    }
}