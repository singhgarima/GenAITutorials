namespace RagChatApp.Infrastructure;

public interface IVectorDatabase
{
    Task CreateCollectionAsync(string collection, string vectorName);
    Task UpsertAsync(string collection, string vectorName, uint id, List<float> vector, string metadata);
    Task<IEnumerable<string>> SearchAsync(string collection, string vectorName, List<float> queryVector, ulong topK);
}