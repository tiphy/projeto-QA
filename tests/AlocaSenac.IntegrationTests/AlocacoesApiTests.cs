using System.Net;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AlocaSenac.IntegrationTests;

public sealed class AlocacoesApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AlocacoesApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task DeveListarAlocacoesEExporOpenApi()
    {
        var alocacoes = await _client.GetAsync("/api/alocacoes");
        var openApi = await _client.GetAsync("/openapi/v1.json");

        Assert.Equal(HttpStatusCode.OK, alocacoes.StatusCode);
        Assert.Equal(HttpStatusCode.OK, openApi.StatusCode);
    }

    [Fact]
    public async Task DeveRedirecionarRaizParaLogin()
    {
        var resposta = await _client.GetAsync("/");

        Assert.Equal(HttpStatusCode.Redirect, resposta.StatusCode);
        Assert.Equal("/Login", resposta.Headers.Location?.OriginalString);
    }

    [Theory]
    [InlineData("/Login/Index", "Acesso institucional")]
    [InlineData("/Dashboard/Index", "Dashboard")]
    [InlineData("/Salas/Index", "Catálogo de ambientes")]
    [InlineData("/Salas/Editar/1", "Editar sala")]
    [InlineData("/Salas/Detalhes/1", "Detalhes da sala")]
    [InlineData("/Agendamentos/Index", "Gestão das reservas")]
    [InlineData("/Relatorios/Index", "Métricas consolidadas")]
    [InlineData("/Configuracoes/Index", "Horários de funcionamento")]
    public async Task DeveAbrirTelasDoSistema(string pagina, string conteudoEsperado)
    {
        if (pagina != "/Login/Index")
        {
            await EntrarComoAdministrador();
        }

        var resposta = await _client.GetAsync(pagina);
        var html = await resposta.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        Assert.Contains(conteudoEsperado, html);
    }

    [Fact]
    public async Task DeveAutenticarComCredenciaisDeDemonstracao()
    {
        await EntrarComoAdministrador();

        var resposta = await _client.GetAsync("/Dashboard/Index");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
    }

    [Fact]
    public async Task DeveRejeitarCredenciaisInvalidas()
    {
        var token = await ObterTokenAntiforgery();
        var resposta = await _client.PostAsync("/Login?handler=Login", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["Email"] = "admin@gmail.com",
            ["Senha"] = "senha-incorreta",
            ["__RequestVerificationToken"] = token
        }));
        var html = await resposta.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        Assert.Contains("E-mail ou senha inválidos.", WebUtility.HtmlDecode(html));
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

    [Fact]
    public async Task DeveCriarAgendamentoPelaTela()
    {
        await EntrarComoAdministrador();
        var sala = $"Sala Tela {Guid.NewGuid():N}";
        var inicio = DateTime.Today.AddYears(2).AddHours(9);

        var resposta = await EnviarNovoAgendamento(sala, "Ana Souza", inicio, inicio.AddHours(1));

        Assert.Equal(HttpStatusCode.Redirect, resposta.StatusCode);
        var alocacoes = await _client.GetFromJsonAsync<List<AlocacaoResponse>>("/api/alocacoes");
        Assert.Contains(alocacoes!, alocacao => alocacao.Sala == sala && alocacao.Professor == "Ana Souza");

        var lista = await _client.GetStringAsync("/Agendamentos/Index");
        Assert.Contains(sala, WebUtility.HtmlDecode(lista));
    }

    [Fact]
    public async Task DeveMostrarConflitoAoAgendarPelaTela()
    {
        await EntrarComoAdministrador();
        var sala = $"Sala Conflito {Guid.NewGuid():N}";
        var inicio = DateTime.Today.AddYears(2).AddHours(14);

        var primeira = await EnviarNovoAgendamento(sala, "Ana Souza", inicio, inicio.AddHours(2));
        var sobreposta = await EnviarNovoAgendamento(sala, "Bruno Lima", inicio.AddHours(1), inicio.AddHours(3));
        var html = WebUtility.HtmlDecode(await sobreposta.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.Redirect, primeira.StatusCode);
        Assert.Equal(HttpStatusCode.OK, sobreposta.StatusCode);
        Assert.Contains("Conflito de alocação", html);
    }

    [Fact]
    public async Task DeveRecusarIntervaloInvalidoPelaTela()
    {
        await EntrarComoAdministrador();
        var inicio = DateTime.Today.AddYears(2).AddHours(10);

        var resposta = await EnviarNovoAgendamento($"Sala {Guid.NewGuid():N}", "Ana Souza", inicio, inicio);
        var html = WebUtility.HtmlDecode(await resposta.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        Assert.Contains("deve ser anterior ao término", html);
    }

    [Fact]
    public async Task DeveFiltrarAgendamentosPorSala()
    {
        await EntrarComoAdministrador();
        var salaA = $"Sala Filtro A {Guid.NewGuid():N}";
        var salaB = $"Sala Filtro B {Guid.NewGuid():N}";
        var inicio = DateTime.Today.AddYears(2).AddHours(8);
        await EnviarNovoAgendamento(salaA, "Ana Souza", inicio, inicio.AddHours(1));
        await EnviarNovoAgendamento(salaB, "Bruno Lima", inicio, inicio.AddHours(1));

        var html = WebUtility.HtmlDecode(await _client.GetStringAsync($"/Agendamentos/Index?Sala={Uri.EscapeDataString(salaA)}"));
        var tabela = html[html.IndexOf("<tbody>", StringComparison.Ordinal)..];

        Assert.Contains(salaA, tabela);
        Assert.DoesNotContain(salaB, tabela);
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

    private async Task EntrarComoAdministrador()
    {
        var token = await ObterTokenAntiforgery();
        var resposta = await _client.PostAsync("/Login?handler=Login", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["Email"] = "admin@gmail.com",
            ["Senha"] = "1234",
            ["__RequestVerificationToken"] = token
        }));

        Assert.Equal(HttpStatusCode.Redirect, resposta.StatusCode);
    }

    private async Task<HttpResponseMessage> EnviarNovoAgendamento(string sala, string responsavel, DateTime inicio, DateTime fim)
    {
        var pagina = await _client.GetStringAsync("/Agendamentos/Index");
        var token = Regex.Match(pagina, "name=\"__RequestVerificationToken\"[^>]*value=\"([^\"]+)\"");
        Assert.True(token.Success, "O formulário de novo agendamento deve renderizar o token antiforgery.");

        return await _client.PostAsync("/Agendamentos/Index", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["Novo.Sala"] = sala,
            ["Novo.Responsavel"] = responsavel,
            ["Novo.Inicio"] = inicio.ToString("yyyy-MM-ddTHH:mm"),
            ["Novo.Fim"] = fim.ToString("yyyy-MM-ddTHH:mm"),
            ["__RequestVerificationToken"] = token.Groups[1].Value
        }));
    }

    private async Task<string> ObterTokenAntiforgery()
    {
        var resposta = await _client.GetAsync("/Login");
        var html = await resposta.Content.ReadAsStringAsync();
        var token = Regex.Match(html, "name=\"__RequestVerificationToken\"[^>]*value=\"([^\"]+)\"");

        Assert.True(token.Success, "O formulário de login deve renderizar o token antiforgery.");
        return token.Groups[1].Value;
    }
}
