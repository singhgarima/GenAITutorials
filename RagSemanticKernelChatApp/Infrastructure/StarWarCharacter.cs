using System.Text.Json.Serialization;

namespace RagSemanticKernelChatApp.Infrastructure;

public class StarWarCharacter
{
    public uint Id { get; set; } = 0;
    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("height")] public float Height { get; set; } = 0.0f;
    [JsonPropertyName("mass")] public string? Mass { get; set; } = "0";
    [JsonPropertyName("hair_color")] public string HairColor { get; set; } = string.Empty;
    [JsonPropertyName("skin_color")] public string SkinColor { get; set; } = string.Empty;
    [JsonPropertyName("eye_color")] public string EyeColor { get; set; } = string.Empty;
    [JsonPropertyName("birth_year")] public string BirthYear { get; set; } = string.Empty;
    [JsonPropertyName("gender")] public string Gender { get; set; } = string.Empty;
    [JsonPropertyName("homeworld")] public string Homeworld { get; set; } = string.Empty;
    [JsonPropertyName("species")] public string Species { get; set; } = string.Empty;
}