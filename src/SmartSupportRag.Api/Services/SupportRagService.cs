using System.Text;
using Microsoft.Extensions.AI;
using Pgvector;
using SmartSupportRag.Api.DTOs.Support;
using SmartSupportRag.Api.Mapper;
using SmartSupportRag.Api.Repositories;

namespace SmartSupportRag.Api.Services;

public class SupportRagService : ISupportRagService
{
    private const string SystemPrompt = """
        Eres el asistente virtual experto de soporte al cliente y políticas de la plataforma de comercio electrónico.
        Tu objetivo es responder las consultas de los agentes de soporte y compradores con total precisión, claridad y profesionalismo.

        Reglas estrictas de operación:
        - Responde ÚNICAMENTE utilizando la información contenida en los fragmentos de políticas oficiales proporcionados en el contexto.
        - Menciona de forma natural el título de la política oficial que respalda tu respuesta.
        - No inventes plazos, tarifas, porcentajes ni excepciones que no aparezcan explícitamente en el contexto.
        - Si los fragmentos proporcionados no contienen información suficiente para responder la pregunta, indica claramente que esa información no se encuentra especificada en las políticas oficiales vigentes.
        """;

    private readonly IPolicyRepository _policyRepository;
    private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator;
    private readonly IChatClient _chatClient;

    public SupportRagService(
        IPolicyRepository policyRepository,
        IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator,
        IChatClient chatClient)
    {
        _policyRepository = policyRepository;
        _embeddingGenerator = embeddingGenerator;
        _chatClient = chatClient;
    }

    public async Task<AskSupportResponse> AskAsync(
        AskSupportRequest request,
        CancellationToken cancellationToken = default)
    {
        var topK = Math.Clamp(request.TopK, 1, 10);
        var generatedEmbeddings = await _embeddingGenerator.GenerateAsync(
            [request.Question.Trim()],
            cancellationToken: cancellationToken);

        var queryVector = new Vector(generatedEmbeddings[0].Vector);
        var matchedChunks = await _policyRepository.SearchSimilarChunksAsync(
            queryVector,
            request.Category,
            topK,
            cancellationToken);

        if (matchedChunks.Count == 0)
        {
            return matchedChunks.ToAskSupportResponse(
                request,
                "No se encontraron políticas cargadas en la base de conocimiento para responder esta consulta.");
        }

        var userPrompt = BuildGroundedUserPrompt(request.Question, matchedChunks);
        var messages = new List<ChatMessage>
        {
            new(ChatRole.System, SystemPrompt),
            new(ChatRole.User, userPrompt)
        };

        var chatResponse = await _chatClient.GetResponseAsync(
            messages,
            cancellationToken: cancellationToken);

        var answerText = string.IsNullOrWhiteSpace(chatResponse.Text)
            ? "No fue posible generar una respuesta con el contexto disponible."
            : chatResponse.Text.Trim();

        return matchedChunks.ToAskSupportResponse(request, answerText);
    }

    private static string BuildGroundedUserPrompt(
        string question,
        IReadOnlyList<PolicyChunkSearchResult> matchedChunks)
    {
        var sb = new StringBuilder();
        sb.AppendLine("FRAGMENTOS DE POLÍTICAS OFICIALES RECUPERADOS:");
        sb.AppendLine();

        for (var i = 0; i < matchedChunks.Count; i++)
        {
            var match = matchedChunks[i];
            sb.AppendLine($"--- Fuente {i + 1}: {match.DocumentTitle} (Categoría: {match.Chunk.Category}, Chunk #{match.Chunk.ChunkIndex}) ---");
            sb.AppendLine(match.Chunk.Content);
            sb.AppendLine();
        }

        sb.AppendLine("PREGUNTA DEL USUARIO:");
        sb.AppendLine(question.Trim());

        return sb.ToString();
    }
}

