using System.Net;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AlocaSenac.IntegrationTests;

public sealed class AlocacoesApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AlocacoesApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task DeveListarAlocacoesEExporOpenApi()
    {
        var alocacoes = await _client.GetAsync("/api/alocacoes");
        var openApi = await _client.GetAsync("/openapi/v1.json");

        Assert.Equal(HttpStatusCode.OK, alocacoes.StatusCode);
        Assert.Equal(HttpStatusCode.OK, openApi.StatusCode);
    }

    [Theory]
    [InlineData("Solicitante", "Solicitar espaço")]
    [InlineData("Responsável", "Visão de acompanhamento da agenda")]
    [InlineData("Administrador", "Remover")]
    public async Task DeveAbrirCadaPerfilPeloLinkDeNavegacao(string persona, string conteudoEsperado)
    {
        var resposta = await _client.GetAsync($"/?persona={UrlEncoder.Default.Encode(persona)}");
        var html = await resposta.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        Assert.Contains("aria-current=\"page\"", html);
        Assert.Contains(conteudoEsperado, html);
    }

    [Fact]
    public async Task DeveCriarRejeitarConflitoEExcluirAlocacao()
    {
        var inicio = DateTime.UtcNow.Date.AddYears(2).AddHours(9);
        var request = new CriarAlocacaoRequest(
            $"Sala {Guid.NewGuid()}",
            "Ana Souza",
            inicio,
            inicio.AddHours(1));

        var criada = await _client.PostAsJsonAsync("/api/alocacoes", request);
        var alocacao = await criada.Content.ReadFromJsonAsync<AlocacaoResponse>();

        Assert.Equal(HttpStatusCode.Created, criada.StatusCode);
        Assert.NotNull(alocacao);

        var conflito = await _client.PostAsJsonAsync("/api/alocacoes", request);
        Assert.Equal(HttpStatusCode.Conflict, conflito.StatusCode);

        var removida = await _client.DeleteAsync($"/api/alocacoes/{alocacao.Id}");
        Assert.Equal(HttpStatusCode.NoContent, removida.StatusCode);

        var consulta = await _client.GetAsync($"/api/alocacoes/{alocacao.Id}");
        Assert.Equal(HttpStatusCode.NotFound, consulta.StatusCode);
    }

    [Fact]
    public async Task DeveRetornarBadRequestParaIntervaloInvalido()
    {
        var inicio = DateTime.UtcNow.Date.AddYears(2).AddHours(10);
        var request = new CriarAlocacaoRequest(
            $"Sala {Guid.NewGuid()}",
            "Ana Souza",
            inicio,
            inicio);

        var resposta = await _client.PostAsJsonAsync("/api/alocacoes", request);

        Assert.Equal(HttpStatusCode.BadRequest, resposta.StatusCode);
    }

    private sealed record CriarAlocacaoRequest(
        string Sala,
        string Professor,
        DateTime Inicio,
        DateTime Fim);

    private sealed record AlocacaoResponse(
        Guid Id,
        string Sala,
        string Professor,
        DateTime Inicio,
        DateTime Fim);
}
