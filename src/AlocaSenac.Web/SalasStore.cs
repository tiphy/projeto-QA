/// <summary>
/// Catálogo de salas da aplicação (em memória, como os agendamentos).
/// É a fonte da rota GET /api/salas, da tela Salas e do formulário de novo agendamento.
/// </summary>
public sealed class SalasStore
{
    private readonly List<SalaResponse> _salas =
    [
        new(1, "Sala 101", "Bloco A", "1º andar", "Sala de aula", 30),
        new(2, "Laboratório 202", "Bloco B", "2º andar", "Laboratório", 24),
        new(3, "Auditório Central", "Bloco A", "Térreo", "Auditório", 120)
    ];

    public IReadOnlyList<SalaResponse> Listar() => _salas;

    public SalaResponse? ObterPorNome(string? nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            return null;
        }

        return _salas.FirstOrDefault(sala => sala.Nome.Equals(nome.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}

public record SalaResponse(int Id, string Nome, string Bloco, string Andar, string Tipo, int Capacidade);

/// <summary>Sala com a disponibilidade no intervalo consultado (null quando nenhum intervalo foi informado).</summary>
public record SalaDisponibilidadeResponse(int Id, string Nome, string Bloco, string Andar, string Tipo, int Capacidade, bool? Disponivel);
