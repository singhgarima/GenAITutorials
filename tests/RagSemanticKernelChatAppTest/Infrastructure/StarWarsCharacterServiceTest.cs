using System.Net;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using RagSemanticKernelChatApp.Infrastructure;

namespace RagSemanticKernelChatAppTest.Infrastructure;

public class StarWarsCharacterServiceTest
{
    [Fact]
    public async Task GetCharactersAsync_ShouldReturnListOfStarWarCharacters()
    {
        // Arrange
        var mockHttpMsgHandler = new Mock<HttpMessageHandler>();
        mockHttpMsgHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    """
                    {"features":[],"rows":[{"row_idx":0,"row":{"name":"Luke Skywalker","height":172.0,"mass":"77","hair_color":"blond","skin_color":"fair","eye_color":"blue","birth_year":"19BBY","gender":"male","homeworld":"Tatooine","species":"Human"},"truncated_cells":[]}]}
                    """)
            });
        var mockHttpClient = new HttpClient(mockHttpMsgHandler.Object);

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { { "DataSource:Endpoint", "http://localhost:1234" } })
            .Build();

        var service = new StarWarCharacterService(mockHttpClient, config);

        // Act
        var characters = await service.GetCharactersAsync();

        // Assert
        Assert.NotNull(characters);
        Assert.Single(characters);
        var character = characters[0];
        Assert.Equal("Luke Skywalker", character.Name);
        Assert.Equal(172.0f, character.Height);
        Assert.Equal("77", character.Mass);
        Assert.Equal("blond", character.HairColor);
        Assert.Equal("fair", character.SkinColor);
        Assert.Equal("blue", character.EyeColor);
        Assert.Equal("19BBY", character.BirthYear);
        Assert.Equal("male", character.Gender);
        Assert.Equal("Tatooine", character.Homeworld);
        Assert.Equal("Human", character.Species);
        Assert.Equal(0u, character.Id);
    }
}