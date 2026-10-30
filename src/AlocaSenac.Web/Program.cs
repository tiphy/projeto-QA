var builder = WebApplication.CreateBuilder(args);

// Adiciona OpenAPI
builder.Services.AddOpenApi();
builder.Services.AddRazorPages();
builder.Services.AddSingleton<AlocacoesStore>();

var app = builder.Build();

// Mapeia a rota do OpenAPI (tanto em Development quanto em produção/avaliação)
app.MapOpenApi();

// A interface fica na raiz; o contrato OpenAPI continua disponível em /openapi/v1.json.
app.MapRazorPages();

// --- Endpoints de Negócio: AlocaSenac ---

// Listar todas as alocações
app.MapGet("/api/alocacoes", (AlocacoesStore store) => Results.Ok(store.Listar()))
   .WithName("ListarAlocacoes")
   .WithSummary("Lista todas as alocações de salas cadastradas")
   .Produces<List<AlocacaoResponse>>(StatusCodes.Status200OK);

// Obter alocação por ID
app.MapGet("/api/alocacoes/{id:guid}", (Guid id, AlocacoesStore store) =>
{
    var alocacao = store.Obter(id);
    return alocacao is not null ? Results.Ok(alocacao) : Results.NotFound();
})
.WithName("ObterAlocacaoPorId")
.WithSummary("Busca uma alocação específica pelo ID")
.Produces<AlocacaoResponse>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

// Criar nova alocação com validação de conflito
app.MapPost("/api/alocacoes", (CriarAlocacaoRequest request, AlocacoesStore store) =>
{
    if (string.IsNullOrWhiteSpace(request.Sala) || string.IsNullOrWhiteSpace(request.Professor))
    {
        return Results.BadRequest(new { mensagem = "Sala e Professor são campos obrigatórios." });
    }

    if (request.Inicio >= request.Fim)
    {
        return Results.BadRequest(new { mensagem = "A data/hora de início deve ser anterior ao término." });
    }

    var novaAlocacao = store.TentarCriar(request);
    if (novaAlocacao is null)
    {
        return Results.Conflict(new { mensagem = "Conflito de alocação: a sala já está ocupada neste horário." });
    }

    return Results.Created($"/api/alocacoes/{novaAlocacao.Id}", novaAlocacao);
})
.WithName("CriarAlocacao")
.WithSummary("Cria uma nova alocação de sala")
.Produces<AlocacaoResponse>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status409Conflict);

// Deletar alocação
app.MapDelete("/api/alocacoes/{id:guid}", (Guid id, AlocacoesStore store) =>
{
    return store.Remover(id) ? Results.NoContent() : Results.NotFound();
})
.WithName("RemoverAlocacao")
.WithSummary("Remove uma alocação existente")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound);

app.Run();

// --- DTOs / Records ---
public record CriarAlocacaoRequest(string Sala, string Professor, DateTime Inicio, DateTime Fim);
public record AlocacaoResponse(Guid Id, string Sala, string Professor, DateTime Inicio, DateTime Fim);

public partial class Program { }