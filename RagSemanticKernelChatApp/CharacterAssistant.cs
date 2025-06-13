using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Qdrant;
using Qdrant.Client;
using RagSemanticKernelChatApp.Infrastructure;

namespace RagSemanticKernelChatApp;

public class CharacterAssistant(Kernel kernel)
{
    private const string CollectionName = "star-wars-characters";
    private const string VectorName = "character-details";

    private readonly IChatCompletionService _chatService = kernel.GetRequiredService<IChatCompletionService>();

    private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator =
        kernel.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();

    private readonly QdrantVectorStore _vectorStore = kernel.GetRequiredService<QdrantVectorStore>();

    public async Task IngestAsync(List<StarWarCharacter> characters)
    {
        var collection = await GetCollection(_embeddingGenerator);
        await collection.UpsertAsync(characters);
    }

    public async Task<string> ReplyToPrompt(string prompt)
    {
        // 1. Generate embedding for the prompt
        var promptEmbedding = await _embeddingGenerator.GenerateAsync(prompt);

        // 2. Retrieve the top 5 most relevant characters
        var collection = GetCollection(_embeddingGenerator).Result;
        var topCharacters = collection.SearchAsync(promptEmbedding, 5);


        // 3. Build the full LLM prompt including which weeks to reference
        var context = string.Join(
            "\n",
            await topCharacters.SelectAwait(c => ValueTask.FromResult($"- {c.Record}")).ToArrayAsync()
        );
        var fullPrompt = $"""
                          You are a star war character expert.
                          Here is a question from a user: {prompt}

                          Here are the 5 most relevant star war characters to reference in your response:
                          {context}

                          Please respond to the user's question:
                          - With context provided ONLY
                          - List all information about the character(s) the AI model thinks are relevant to the question in format:
                            Character Name: <name>
                                Height: <height>
                                Mass: <mass>
                                Hair Color: <hair_color>
                                Skin Color: <skin_color>
                                Eye Color: <eye_color>
                                Birth Year: <birth_year>
                                <age>: <age>
                                Homeworld: <homeworld>
                                Species: <species>
                          - Do not provide any inference or additional information
                          - Do not include any additional character outside of the 5 provides in the prompt.
                          - Do not pull any information about any character form outside of information provided
                          """;


        // 5. Call the LLM
        var chatMessageContent = await _chatService.GetChatMessageContentAsync(fullPrompt);
        return chatMessageContent.Content ?? string.Empty;
    }

    private async Task<QdrantCollection<ulong, StarWarCharacter>> GetCollection(
        IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator)
    {
        var collectionOptions = new QdrantCollectionOptions
        {
            EmbeddingGenerator = embeddingGenerator,
            HasNamedVectors = true,
            Definition = StarWarCharacter.GetVectorStoreCollectionDefinition(VectorName)
        };

        var qdrantClient = _vectorStore.GetService(typeof(QdrantClient)) as QdrantClient;
        var collection = new QdrantCollection<ulong, StarWarCharacter>(
            qdrantClient, CollectionName, true, collectionOptions);
        await collection.EnsureCollectionExistsAsync();
        return collection;
    }
}