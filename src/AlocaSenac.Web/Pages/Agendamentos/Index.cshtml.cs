using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AlocaSenac.Web.Pages.Agendamentos;

public sealed class IndexModel(AlocacoesStore store) : PageModel
{
    public const string StatusPadrao = "Pendente";

    public IReadOnlyList<AlocacaoResponse> Agendamentos { get; private set; } = [];

    public IReadOnlyList<string> SalasDisponiveis { get; private set; } = [];

    // Filtros (formulário GET)
    [BindProperty(SupportsGet = true)]
    public DateTime? Periodo { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Sala { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Status { get; set; }

    // Formulário do modal "Novo agendamento" (POST)
    [BindProperty]
    public NovoAgendamentoInput Novo { get; set; } = new();

    [TempData]
    public string? Mensagem { get; set; }

    public string? Erro { get; private set; }

    public void OnGet()
    {
        Carregar();
    }

    public IActionResult OnPost()
    {
        var request = new CriarAlocacaoRequest(
            Novo.Sala?.Trim() ?? string.Empty,
            Novo.Responsavel?.Trim() ?? string.Empty,
            Novo.Inicio ?? default,
            Novo.Fim ?? default);

        // Mesmas regras e mensagens da API POST /api/alocacoes (400 e 409).
        Erro = AlocacoesStore.Validar(request);
        if (Erro is null && store.TentarCriar(request) is null)
        {
            Erro = AlocacoesStore.MensagemConflito;
        }

        if (Erro is not null)
        {
            Carregar();
            return Page();
        }

        Mensagem = $"Agendamento da sala {request.Sala} criado com sucesso.";
        return RedirectToPage();
    }

    private void Carregar()
    {
        var todos = store.Listar();
        SalasDisponiveis = todos
            .Select(alocacao => alocacao.Sala)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Order(StringComparer.CurrentCultureIgnoreCase)
            .ToList();

        IEnumerable<AlocacaoResponse> filtrados = todos;

        if (Periodo is { } dia)
        {
            filtrados = filtrados.Where(alocacao => alocacao.Inicio.Date <= dia.Date && alocacao.Fim.Date >= dia.Date);
        }

        if (!string.IsNullOrWhiteSpace(Sala))
        {
            filtrados = filtrados.Where(alocacao => alocacao.Sala.Equals(Sala, StringComparison.OrdinalIgnoreCase));
        }

        // Ainda não há fluxo de aprovação: todo agendamento está "Pendente".
        if (!string.IsNullOrWhiteSpace(Status) && !Status.Equals(StatusPadrao, StringComparison.OrdinalIgnoreCase))
        {
            filtrados = [];
        }

        Agendamentos = filtrados.ToList();
    }
}

public sealed class NovoAgendamentoInput
{
    public string? Sala { get; set; }

    public string? Responsavel { get; set; }

    public DateTime? Inicio { get; set; }

    public DateTime? Fim { get; set; }
}
