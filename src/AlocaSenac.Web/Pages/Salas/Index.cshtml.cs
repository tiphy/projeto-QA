using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AlocaSenac.Web.Pages.Salas;

public sealed class IndexModel(SalasStore salasStore) : PageModel
{
    public IReadOnlyList<SalaResponse> Salas { get; private set; } = [];

    public void OnGet()
    {
        Salas = salasStore.Listar();
    }

    public void OnPost()
    {
        Salas = salasStore.Listar();
    }
}
