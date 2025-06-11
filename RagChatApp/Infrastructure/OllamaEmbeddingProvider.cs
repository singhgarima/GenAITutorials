using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace RagChatApp.Infrastructure;

public class OllamaEmbeddingProvider(HttpClient httpClient, IConfiguration config) : IEmbeddingProvider
{
    private readonly string _embeddingModel = config["Ollama:EmbeddingModel"] ?? "nomic-embed-text";
    private readonly string _endpoint = config["Ollama:Endpoint"] ?? "http://127.0.0.1:11434";

    public async Task<List<float>> EmbedAsync(string text)
    {
        var payload = new
        {
            model = _embeddingModel,
            input = text
        };
        var response = await httpClient.PostAsJsonAsync($"{_endpoint}/api/embed", payload);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Failed to get embeddings: {response.ReasonPhrase}");

        // For debugging purposes
        var responseContent = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<EmbeddingResponse>(responseContent);
        return result?.Embeddings.FirstOrDefault([]) ?? [];
    }
}