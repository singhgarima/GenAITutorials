using System.Text.Json.Serialization;
using Microsoft.Extensions.VectorData;

namespace RagSemanticKernelChatApp.Infrastructure;

public class StarWarCharacter
{
    [VectorStoreKey] public ulong Id { get; set; }

    [VectorStoreData]
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [VectorStoreData]
    [JsonPropertyName("height")]
    public float? Height { get; set; } = 0.0f;

    [VectorStoreData]
    [JsonPropertyName("mass")]
    public string? Mass { get; set; } = "0";

    [VectorStoreData]
    [JsonPropertyName("hair_color")]
    public string HairColor { get; set; } = string.Empty;

    [VectorStoreData]
    [JsonPropertyName("skin_color")]
    public string SkinColor { get; set; } = string.Empty;

    [VectorStoreData]
    [JsonPropertyName("eye_color")]
    public string EyeColor { get; set; } = string.Empty;

    [VectorStoreData]
    [JsonPropertyName("birth_year")]
    public string BirthYear { get; set; } = string.Empty;

    [VectorStoreData]
    [JsonPropertyName("gender")]
    public string Gender { get; set; } = string.Empty;

    [VectorStoreData]
    [JsonPropertyName("homeworld")]
    public string Homeworld { get; set; } = string.Empty;

    [VectorStoreData]
    [JsonPropertyName("species")]
    public string Species { get; set; } = string.Empty;

    [VectorStoreVector(768)] public string Embedding => ToString();

    public override string ToString()
    {
        return $"Name: {Name}, Height: {Height}, Mass: {Mass}, Hair Color: {HairColor}, " +
               $"Skin Color: {SkinColor}, Eye Color: {EyeColor}, Birth Year: {BirthYear}, " +
               $"Gender: {Gender}, Homeworld: {Homeworld}, Species: {Species}";
    }

    public static VectorStoreCollectionDefinition GetVectorStoreCollectionDefinition(string vectorName)
    {
        return new VectorStoreCollectionDefinition
        {
            Properties = new List<VectorStoreProperty>
            {
                new VectorStoreKeyProperty("Id", typeof(ulong)),
                new VectorStoreDataProperty("Name", typeof(string)),
                new VectorStoreDataProperty("Height", typeof(float?)),
                new VectorStoreDataProperty("Mass", typeof(string)),
                new VectorStoreDataProperty("HairColor", typeof(string)),
                new VectorStoreDataProperty("SkinColor", typeof(string)),
                new VectorStoreDataProperty("EyeColor", typeof(string)),
                new VectorStoreDataProperty("BirthYear", typeof(string)),
                new VectorStoreDataProperty("Gender", typeof(string)),
                new VectorStoreDataProperty("Homeworld", typeof(string)),
                new VectorStoreDataProperty("Species", typeof(string)),
                new VectorStoreVectorProperty("Embedding", typeof(string), 768)
                {
                    DistanceFunction = DistanceFunction.CosineSimilarity,
                    StorageName = vectorName
                }
            }
        };
    }
}