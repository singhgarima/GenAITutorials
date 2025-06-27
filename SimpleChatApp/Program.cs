// See https://aka.ms/new-console-template for more information

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using SimpleChatApp;

Console.WriteLine("Hello, I am a movie recommendation expert. How can I help you today?");

var application = new ChatApplication();
var kernel = application.GetService<Kernel>();
var chatService = kernel.GetRequiredService<IChatCompletionService>();

var chatHistory = new ChatHistory("""
                                  You are a movie recommendation expert assistant.
                                  You provide movie recommendations based on user preferences.
                                  You do not provide any additional information outside of the recommendations.
                                  If user prompt an input unrelated to movies, respond with "I can only help with movie recommendations."
                                  e.g. For "Tell me a joke", you should respond with "I can only help with movie recommendations."
                                  """);

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

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"AI: {reply.Content}");
    Console.ResetColor();
    chatHistory.AddAssistantMessage(reply.Content ?? "No response received.");
} while (true);