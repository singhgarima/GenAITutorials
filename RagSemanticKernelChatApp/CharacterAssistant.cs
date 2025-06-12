using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Qdrant;
using Qdrant.Client;
using RagSemanticKernelChatApp.Infrastructure;

namespace RagSemanticKernelChatApp;

public class CharacterAssistant(Kernel kernel)
{
    private const string CollectionName = "star-wars-characters";
    private const string VectorName = "character-details";

    public async Task IngestAsync(List<StarWarCharacter> characters)
    {
        var embeddingGenerator = kernel.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();
        var collectionOptions = new QdrantCollectionOptions
        {
            EmbeddingGenerator = embeddingGenerator,
            HasNamedVectors = true,
            Definition = StarWarCharacter.GetVectorStoreCollectionDefinition(VectorName)
        };

        var vectorStore = kernel.GetRequiredService<QdrantVectorStore>();
        var qdrantClient = vectorStore.GetService(typeof(QdrantClient)) as QdrantClient;
        var collection = new QdrantCollection<ulong, StarWarCharacter>(
            qdrantClient, CollectionName, true, collectionOptions);
        await collection.EnsureCollectionExistsAsync();
        await collection.UpsertAsync(characters);
    }
}