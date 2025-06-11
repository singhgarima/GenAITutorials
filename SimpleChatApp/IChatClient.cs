using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace SimpleChatApp;

public interface IChatClient
{
    public Task<ChatMessageContent> GetChatMessageContentAsync(ChatHistory chatHistory);
}