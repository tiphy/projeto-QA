public sealed class AlocacoesStore
{
    private readonly object _sync = new();
    private readonly List<AlocacaoResponse> _alocacoes =
    [
        new(Guid.NewGuid(), "Laboratório 101", "Prof. Carlos", DateTime.UtcNow.Date.AddHours(8), DateTime.UtcNow.Date.AddHours(10)),
        new(Guid.NewGuid(), "Auditório Principal", "Profa. Maria", DateTime.UtcNow.Date.AddHours(14), DateTime.UtcNow.Date.AddHours(17))
    ];

    public List<AlocacaoResponse> Listar()
    {
        lock (_sync)
        {
            return _alocacoes.OrderBy(alocacao => alocacao.Inicio).ToList();
        }
    }

    public AlocacaoResponse? Obter(Guid id)
    {
        lock (_sync)
        {
            return _alocacoes.FirstOrDefault(alocacao => alocacao.Id == id);
        }
    }

    public AlocacaoResponse? TentarCriar(CriarAlocacaoRequest request)
    {
        lock (_sync)
        {
            bool temConflito = _alocacoes.Any(existing =>
                existing.Sala.Equals(request.Sala, StringComparison.OrdinalIgnoreCase) &&
                request.Inicio < existing.Fim &&
                request.Fim > existing.Inicio);

            if (temConflito)
            {
                return null;
            }

            var alocacao = new AlocacaoResponse(
                Guid.NewGuid(),
                request.Sala,
                request.Professor,
                request.Inicio,
                request.Fim);
            _alocacoes.Add(alocacao);
            return alocacao;
        }
    }

    public bool Remover(Guid id)
    {
        lock (_sync)
        {
            var alocacao = _alocacoes.FirstOrDefault(existing => existing.Id == id);
            return alocacao is not null && _alocacoes.Remove(alocacao);
        }
    }
}
