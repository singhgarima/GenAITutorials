using RagChatApp.Infrastructure;

namespace RagChatApp;

public class RagQuoteAssistant(IEmbeddingProvider embeddingProvider, IVectorDatabase vectorDb, ILlmProvider llmProvider)
{
    private const string CollectionName = "star-trek-quotes";
    private const string VectorName = "quote-text";

    public async Task IngestQuotes(List<Quote> quotes)
    {
        var embeddings = await Task.WhenAll(
            quotes.Select(quote => embeddingProvider.EmbedAsync(quote.ToString())));

        vectorDb.CreateCollectionAsync(CollectionName, VectorName).Wait();

        foreach (var (value, index) in quotes.Select((v, i) => (v, i)))
            vectorDb.UpsertAsync(CollectionName, VectorName, (uint)index, embeddings[index], value.ToString()).Wait();
    }
}