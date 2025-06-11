// See https://aka.ms/new-console-template for more information

using RagChatApp;

Console.WriteLine("Hello, I am a Star Trek Quote expert. How can I help you today?");
;

var quotes = Quote.ParseFile(Path.Combine("data", "quotes.json"));

var application = new RagChatApplication();
var ragQuoteAssistant = application.GetService<RagQuoteAssistant>();
await ragQuoteAssistant.IngestQuotes(quotes);

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

    var reply = await ragQuoteAssistant.ReplyToPrompt(userInput);
    Console.WriteLine($"AI:");
    Console.WriteLine($"   {reply}");
} while (true);