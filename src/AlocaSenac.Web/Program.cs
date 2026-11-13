var builder = WebApplication.CreateBuilder(args);

// Adiciona OpenAPI
builder.Services.AddOpenApi();
builder.Services
    .AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/Login";
    });
builder.Services.AddAuthorization();
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Dashboard");
    options.Conventions.AuthorizeFolder("/Salas");
    options.Conventions.AuthorizeFolder("/Agendamentos");
    options.Conventions.AuthorizeFolder("/Relatorios");
    options.Conventions.AuthorizeFolder("/Configuracoes");
});
builder.Services.AddSingleton<AlocacoesStore>();
builder.Services.AddSingleton<SalasStore>();

var app = builder.Build();

// Mapeia a rota do OpenAPI (tanto em Development quanto em produção/avaliação)
app.MapOpenApi();

// A interface fica na raiz; o contrato OpenAPI continua disponível em /openapi/v1.json.
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

// --- Endpoints de Negócio: AlocaSenac ---

// Listar salas; com inicio e fim, informa se cada sala está livre no intervalo
app.MapGet("/api/salas", (DateTime? inicio, DateTime? fim, SalasStore salas, AlocacoesStore alocacoes) =>
{
    if (inicio.HasValue != fim.HasValue)
    {
        return Results.BadRequest(new { mensagem = "Informe inicio e fim juntos para consultar a disponibilidade." });
    }

    if (inicio >= fim)
    {
        return Results.BadRequest(new { mensagem = AlocacoesStore.MensagemIntervaloInvalido });
    }

    var lista = salas.Listar()
        .Select(sala => new SalaDisponibilidadeResponse(
            sala.Id, sala.Nome, sala.Bloco, sala.Andar, sala.Tipo, sala.Capacidade,
            inicio is null ? null : alocacoes.EstaLivre(sala.Nome, inicio.Value, fim!.Value)))
        .ToList();
    return Results.Ok(lista);
})
.WithName("ListarSalas")
.WithSummary("Lista as salas cadastradas e, se informados inicio e fim, a disponibilidade de cada uma")
.Produces<List<SalaDisponibilidadeResponse>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest);

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
    var erro = AlocacoesStore.Validar(request);
    if (erro is not null)
    {
        return Results.BadRequest(new { mensagem = erro });
    }

    var novaAlocacao = store.TentarCriar(request);
    if (novaAlocacao is null)
    {
        return Results.Conflict(new { mensagem = AlocacoesStore.MensagemConflito });
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