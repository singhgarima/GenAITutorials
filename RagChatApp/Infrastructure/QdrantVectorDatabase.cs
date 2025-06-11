using Microsoft.Extensions.Configuration;
using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace RagChatApp.Infrastructure;

public class QdrantVectorDatabase(QdrantClient client) : IVectorDatabase
{
    public QdrantVectorDatabase(IConfiguration config)
        : this(new QdrantClient(new Uri(config["Qdrant:Endpoint"] ?? "127.0.0.1").Host,
            new Uri(config["Qdrant:Endpoint"] ?? "6334").Port))
    {
    }

    public async Task CreateCollectionAsync(string collection, string vectorName)
    {
        var exists = await client.CollectionExistsAsync(collection);
        if (exists)
            return;

        await client.CreateCollectionAsync(
            collection,
            new VectorParamsMap
            {
                Map =
                {
                    [vectorName] = new VectorParams { Size = 768, Distance = Distance.Dot }
                }
            }
        );
    }

    public async Task UpsertAsync(string collection, string vectorName, uint id, List<float> vector,
        string metadata)
    {
        await client.UpsertAsync(collection, new[]
        {
            new PointStruct
            {
                Id = new PointId(id),
                Vectors = new Dictionary<string, Vector> { [vectorName] = vector.ToArray() },
                Payload = { ["metadata"] = new Value { StringValue = metadata } }
            }
        });
    }

    public async Task<IEnumerable<string>> SearchAsync(string collection, string vectorName, List<float> queryVector,
        ulong limit = 2)
    {
        var searchResults = await client.SearchAsync(
            collection,
            queryVector.ToArray(),
            vectorName: vectorName,
            limit: limit);
        return searchResults.Select(point => point.Payload["metadata"].StringValue);
    }
}