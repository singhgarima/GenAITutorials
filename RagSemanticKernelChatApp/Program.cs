using Microsoft.SemanticKernel;
using RagSemanticKernelChatApp;
using RagSemanticKernelChatApp.Infrastructure;

Console.WriteLine("Hello, I am a Star Wars character expert. How can I help you today?");

var app = new RagSemanticKernelChatApplication();

var characterService = app.GetService<IStarWarCharacterService>();
var characters = await characterService.GetCharactersAsync();


var kernel = app.GetService<Kernel>();
var assistant = new CharacterAssistant(kernel);
await assistant.IngestAsync(characters);

do
{
    Console.WriteLine("==========================================================");
    Console.WriteLine("User: ");
    var userInput = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(userInput) || userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Exiting chat...");
        break;
    }

    var reply = await assistant.ReplyToPrompt(userInput);
    Console.WriteLine("AI:");
    foreach (var line in reply.Split("\n")) Console.WriteLine($"   {line}");
} while (true);