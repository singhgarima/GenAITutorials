using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using OllamaSharp;

namespace SimpleChatApp;

public class OllamaChatClient(OllamaApiClient client) : IChatClient
{
    private readonly IChatCompletionService _chatService = client.AsChatCompletionService();

    public OllamaChatClient(HttpClient httpClient, IConfiguration config) : this(
        new OllamaApiClient(httpClient, config["Ollama:Model"] ?? "llama3.2:1b"))
    {
    }

    public async Task<ChatMessageContent> GetChatMessageContentAsync(ChatHistory chatHistory)
    {
        return await _chatService.GetChatMessageContentAsync(chatHistory);
    }
}