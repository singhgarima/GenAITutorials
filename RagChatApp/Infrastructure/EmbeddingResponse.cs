using System.Text.Json.Serialization;

namespace RagChatApp.Infrastructure;

public class EmbeddingResponse
{
    [JsonPropertyName("model")] public string Model { get; set; } = string.Empty;
    [JsonPropertyName("embeddings")] public List<List<float>> Embeddings { get; set; } = [];
}