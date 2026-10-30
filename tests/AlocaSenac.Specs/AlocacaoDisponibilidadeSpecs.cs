namespace AlocaSenac.Specs;

public sealed class AlocacaoDisponibilidadeSpecs
{
    [Fact]
    public void OutraSalaContinuaDisponivelNoMesmoHorario()
    {
        var store = new AlocacoesStore();
        var inicio = DateTime.UtcNow.Date.AddYears(2).AddHours(9);

        var primeiraSala = store.TentarCriar(new CriarAlocacaoRequest(
            "Laboratório A",
            "Ana Souza",
            inicio,
            inicio.AddHours(1)));
        var segundaSala = store.TentarCriar(new CriarAlocacaoRequest(
            "Laboratório B",
            "Bruno Lima",
            inicio,
            inicio.AddHours(1)));

        Assert.NotNull(primeiraSala);
        Assert.NotNull(segundaSala);
    }
}
