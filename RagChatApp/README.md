# RAG (Retrieval-Augmented Generation) Chat Application

A console-based chat application where star-trek quotes are used as a knowledge base. It demonstrates how to build a
RAG (Retrieval-Augmented Generation) application from scratch

## Technologies Used

| Technology      | Description                                                   |
|-----------------|---------------------------------------------------------------|
| .NET 9.0        | The framework used to build the application.                  |
| Ollama          | A local LLM (Large Language Model) provider.                  |
| Qdrant          | A vector database used for storing and retrieving embeddings. |

## Running the Chat Application

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [Ollama](https://ollama.ai) installed and running locally
- [Qdrant](https://qdrant.tech) installed and running locally (or use the Docker image)

### Running the RagChatApp

1. Start up Qdrant locally. You can use the Docker image provided under folder `qdrant`
    ```bash
    cd qdrant
    docker compose up
    ```
1. Start up chat application
    ```bash
    cd RagChatApp
    dotnet run
    ```

The application will start and you can begin chatting with the AI assistant.
The assistant is configured to help you find relevant Star Trek quotes based on your input.

### Usage

![RAG Chat App](../docs/images/ragchat.png)

## Change the model

You can change model by modifying the `appsettings.json` file in the `RagChatApp` directory. Change the `ChatModel` or
`EmbeddingModel` and remember to download the model with ollama using `ollama pull <model-name>` before running the app.

## Credits

Quotes from https://codepen.io/JeanRC/details/oLJMeo