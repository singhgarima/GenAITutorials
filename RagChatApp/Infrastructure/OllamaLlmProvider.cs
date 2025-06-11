using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace RagChatApp.Infrastructure;

public class OllamaLlmProvider(HttpClient httpClient, IConfiguration config) : ILlmProvider
{
    private readonly string _embeddingModel = config["Ollama:ChatModel"] ?? "llama3.2:1b";

    public async Task<string?> GenerateAsync(string prompt)
    {
        var payload = new
        {
            model = _embeddingModel,
            prompt,
            stream = false
        };
        var response = await httpClient.PostAsJsonAsync("/api/generate", payload);
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(
                $"Failed to generate text: {response.ReasonPhrase}. Response: {await response.Content.ReadAsStringAsync()}");

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        return json.GetProperty("response").GetString();
    }
}