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
}
