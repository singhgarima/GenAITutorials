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

    public async Task<string> ReplyToPrompt(string prompt)
    {
        // 1. Generate embedding for the prompt
        var promptEmbedding = await embeddingProvider.EmbedAsync(prompt);

        // 2. Retrieve the top 5 most relevant quotes
        var topQuotes = await vectorDb.SearchAsync(CollectionName, VectorName, promptEmbedding, 5);

        // 3. Build the full LLM prompt including which weeks to reference
        var context = string.Join("\n", topQuotes.Select(q => $"- {q}"));
        var fullPrompt = $"""
                          You are a star trek quote assistant.
                          Here is a question from a user: {prompt}

                          Here are the 5 most relevant star trek quotes:
                          {context}

                          Please respond to the user's question:
                          - With only one of the 5 quotes which is most relevant
                          - In the format
                            "Quote: 'quote text' - Character (Episode)"
                          - Do not modify the quote, us as-is.
                          - Do not include any additional information outside of the quotes.
                          """;

        // 5. Call the LLM
        return await llmProvider.GenerateAsync(fullPrompt.Trim()) ?? string.Empty;
    }
}