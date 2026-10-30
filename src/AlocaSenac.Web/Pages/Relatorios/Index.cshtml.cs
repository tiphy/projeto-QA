using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AlocaSenac.Web.Pages.Relatorios;

public sealed class IndexModel(AlocacoesStore store) : PageModel
{
    public int TaxaOcupacao { get; private set; }
    public int HorasReservadas { get; private set; }
    public int TotalAgendamentos { get; private set; }

    public void OnGet()
    {
        TotalAgendamentos = store.Listar().Count;
    }
}
