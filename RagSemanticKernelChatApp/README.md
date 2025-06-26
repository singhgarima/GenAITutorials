# RAG (Retrieval-Augmented Generation) Chat Application with Semantic Kernel

A console-based chat app that looks up Star Wars characters when you ask about them. It’s a hands-on guide to building a
RAG system with Semantic Kernel from the ground up.

## Technologies Used

| Technology      | Description                                                   |
|-----------------|---------------------------------------------------------------|
| .NET 9.0        | The framework used to build the application.                  |
| Semantic Kernel | A framework for building AI applications with .NET.           |
| Ollama          | A local LLM (Large Language Model) provider.                  |
| Qdrant          | A vector database used for storing and retrieving embeddings. |

## Running the Chat Application

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [Ollama](https://ollama.ai) installed and running locally
- [Qdrant](https://qdrant.tech) installed and running locally (or use the Docker image)

### Running the Chat Application

1. Start up Qdrant locally. You can use the Docker image provided under folder `qdrant`
    ```bash
    cd qdrant
    docker compose up
    ```
1. Start up chat application
    ```bash
    cd RagSemanticKernelChatApp
    dotnet run
    ```

The application will start and you can begin chatting with the AI assistant.
The assistant is configured to help you find relevant Star Wars characters based on your input.

### Usage

> Add Screenshot of the app here

## Change the model

You can change model by modifying the `appsettings.json` file in the `RagChatApp` directory. Change the `ChatModel` or
`EmbeddingModel` and remember to download the model with ollama using `ollama pull <model-name>` before running the app.

## Credits

Data Set: [
lizziepikachu/starwars_characters[(https://huggingface.co/datasets/lizziepikachu/starwars_characters/viewer/default/train?views%5B%5D=train)