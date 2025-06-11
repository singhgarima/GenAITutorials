namespace RagChatApp.Infrastructure;

public interface IEmbeddingProvider
{
    Task<List<float>> EmbedAsync(string text);
}