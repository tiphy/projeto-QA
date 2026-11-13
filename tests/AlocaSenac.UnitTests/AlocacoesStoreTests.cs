namespace AlocaSenac.UnitTests;

public sealed class AlocacoesStoreTests
{
    [Fact]
    public void DeveImpedirSobreposicaoSemDiferenciarMaiusculas()
    {
        var store = new AlocacoesStore();
        var inicio = DateTime.UtcNow.Date.AddYears(2).AddHours(9);
        var primeira = new CriarAlocacaoRequest(
            "Sala de Teste",
            "Ana Souza",
            inicio,
            inicio.AddHours(2));
        var sobreposta = new CriarAlocacaoRequest(
            "sala de teste",
            "Bruno Lima",
            inicio.AddHours(1),
            inicio.AddHours(3));

        Assert.NotNull(store.TentarCriar(primeira));
        Assert.Null(store.TentarCriar(sobreposta));
    }

    [Fact]
    public void DevePermitirHorariosAdjacentes()
    {
        var store = new AlocacoesStore();
        var inicio = DateTime.UtcNow.Date.AddYears(2).AddHours(9);

        var primeira = store.TentarCriar(new CriarAlocacaoRequest(
            "Sala Adjacente",
            "Ana Souza",
            inicio,
            inicio.AddHours(1)));
        var segunda = store.TentarCriar(new CriarAlocacaoRequest(
            "Sala Adjacente",
            "Bruno Lima",
            inicio.AddHours(1),
            inicio.AddHours(2)));

        Assert.NotNull(primeira);
        Assert.NotNull(segunda);
    }

    [Fact]
    public void DeveRemoverUmaAlocacaoExistenteUmaUnicaVez()
    {
        var store = new AlocacoesStore();
        var alocacao = store.TentarCriar(new CriarAlocacaoRequest(
            $"Sala {Guid.NewGuid()}",
            "Ana Souza",
            DateTime.UtcNow.Date.AddYears(2).AddHours(9),
            DateTime.UtcNow.Date.AddYears(2).AddHours(10)));

        Assert.NotNull(alocacao);
        Assert.True(store.Remover(alocacao.Id));
        Assert.False(store.Remover(alocacao.Id));
    }

    [Theory]
    [InlineData("", "Ana Souza", 1, AlocacoesStore.MensagemCamposObrigatorios)]
    [InlineData("Sala 1", " ", 1, AlocacoesStore.MensagemCamposObrigatorios)]
    [InlineData("Sala 1", "Ana Souza", 0, AlocacoesStore.MensagemIntervaloInvalido)]
    [InlineData("Sala 1", "Ana Souza", -1, AlocacoesStore.MensagemIntervaloInvalido)]
    [InlineData("Sala 1", "Ana Souza", 1, null)]
    public void DeveValidarSolicitacao(string sala, string professor, int horas, string? esperado)
    {
        var inicio = DateTime.UtcNow.Date.AddYears(2).AddHours(9);
        var request = new CriarAlocacaoRequest(sala, professor, inicio, inicio.AddHours(horas));

        Assert.Equal(esperado, AlocacoesStore.Validar(request));
    }

    [Fact]
    public void DeveInformarSeSalaEstaLivre()
    {
        var store = new AlocacoesStore();
        var inicio = DateTime.UtcNow.Date.AddYears(2).AddHours(9);
        store.TentarCriar(new CriarAlocacaoRequest("Sala Livre", "Ana Souza", inicio, inicio.AddHours(2)));

        Assert.False(store.EstaLivre("sala livre", inicio.AddHours(1), inicio.AddHours(3)));
        Assert.True(store.EstaLivre("Sala Livre", inicio.AddHours(2), inicio.AddHours(3)));
        Assert.True(store.EstaLivre("Outra Sala", inicio, inicio.AddHours(2)));
    }

    [Theory]
    [InlineData("Sala 101", "Sala 101")]
    [InlineData("  laboratório 202 ", "Laboratório 202")]
    [InlineData("Sala Inexistente", null)]
    [InlineData("", null)]
    public void DeveEncontrarSalaDoCatalogoPeloNome(string nome, string? esperado)
    {
        var salas = new SalasStore();

        Assert.Equal(esperado, salas.ObterPorNome(nome)?.Nome);
    }
}
