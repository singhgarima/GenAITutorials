# Simple Chat App

A simple console-based chat application that demonstrates how to use Semantic Kernel with Ollama's local LLMs.

## Running the SimpleChatApp

From the repository root:

```bash
cd SimpleChatApp
dotnet run
```

The application will start and you can begin chatting with the AI assistant.
The assistant is configured as a movie recommendation expert.

### Usage

1. The app will display "Hello, I am a movie recommendation expert. How can I help you today?"
2. Type your message after the "User: " prompt
3. The AI will respond with movie recommendations based on your input
4. Continue the conversation as desired
5. Type "exit" to end the chat session

## Change the model

You can change model by modifying the `appsettings.json` file in the `SimpleChatApp` directory. Change the `ChatModel`
and remember to download it with ollama using `ollama pull <model-name>` before running the app.

```json
{
  "Ollama": {
    "Endpoint": "http://127.0.0.1:11434",
    "ChatModel": "llama3.2:1b"
  }
}
```