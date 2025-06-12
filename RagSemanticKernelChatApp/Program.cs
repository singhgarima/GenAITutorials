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

