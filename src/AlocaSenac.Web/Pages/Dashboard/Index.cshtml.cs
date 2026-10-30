using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AlocaSenac.Web.Pages.Dashboard;

public sealed class IndexModel(AlocacoesStore store) : PageModel
{
    public int TotalSalas { get; private set; }
    public int TotalAgendamentos { get; private set; }
    public int SolicitacoesPendentes { get; private set; }
    public int OcupacaoSemanal { get; private set; }
    public IReadOnlyList<AlocacaoResponse> Agendamentos { get; private set; } = [];

    public void OnGet()
    {
        Agendamentos = store.Listar();
        TotalAgendamentos = Agendamentos.Count;
        SolicitacoesPendentes = TotalAgendamentos;
    }
}
