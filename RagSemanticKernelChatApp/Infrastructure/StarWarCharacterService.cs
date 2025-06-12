using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace RagSemanticKernelChatApp.Infrastructure;

public class StarWarCharacterService(HttpClient httpClient, IConfiguration config) : IStarWarCharacterService
{
    private readonly string _endpoint = config["DataSource:Endpoint"]!;

    public async Task<List<StarWarCharacter>> GetCharactersAsync()
    {
        var response = await httpClient.GetAsync($"{_endpoint}");
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Failed to fetch characters: {response.ReasonPhrase}");

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
        return apiResponse?.Rows
            .Select(r =>
            {
                r.Character.Id = r.RowIdx;
                return r.Character;
            })
            .ToList() ?? new List<StarWarCharacter>();
    }

    private class ApiResponse
    {
        [JsonPropertyName("rows")] public List<RowWrapper> Rows { get; } = new();
    }

    private class RowWrapper
    {
        [JsonPropertyName("row_idx")] public uint RowIdx { get; set; }

        [JsonPropertyName("row")] public StarWarCharacter Character { get; } = null!;
    }
}