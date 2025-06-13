// See https://aka.ms/new-console-template for more information

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using SimpleChatApp;

Console.WriteLine("Hello, I am a movie recommendation expert. How can I help you today?");

var application = new ChatApplication();
var kernel = application.GetService<Kernel>();
var chatService = kernel.GetRequiredService<IChatCompletionService>();

var chatHistory = new ChatHistory("You are a movie recommendation expert assistant.");

do
{
    Console.WriteLine("User: ");
    var userInput = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(userInput) || userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Exiting chat...");
        break;
    }

    chatHistory.AddUserMessage(userInput);
    var reply = await chatService.GetChatMessageContentAsync(chatHistory);

    Console.WriteLine($"AI: {reply.Content}");
    chatHistory.AddAssistantMessage(reply.Content ?? "No response received.");
} while (true);