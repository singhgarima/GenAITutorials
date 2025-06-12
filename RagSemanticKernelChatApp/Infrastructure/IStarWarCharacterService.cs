namespace RagSemanticKernelChatApp.Infrastructure;

public interface IStarWarCharacterService
{
    public Task<List<StarWarCharacter>> GetCharactersAsync();
}