namespace RagChatApp.Infrastructure;

public interface ILlmProvider
{
    Task<string?> GenerateAsync(string prompt);
}