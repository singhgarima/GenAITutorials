using System.Text.Json;
using System.Text.Json.Serialization;

namespace RagChatApp;

public class Quote
{
    [JsonPropertyName("quote")] public string Text { get; set; } = string.Empty;
    [JsonPropertyName("character")] public string Character { get; set; } = string.Empty;
    [JsonPropertyName("episode")] public string Episode { get; set; } = string.Empty;

    public static List<Quote> ParseFile(string filePath)
    {
        if (!File.Exists(filePath)) throw new FileNotFoundException($"The file {filePath} does not exist.");

        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<Quote>>(json) ?? [];
    }

    public override string ToString()
    {
        return $"Quote: {Text}, Character: {Character} (Episode: {Episode})";
    }
}