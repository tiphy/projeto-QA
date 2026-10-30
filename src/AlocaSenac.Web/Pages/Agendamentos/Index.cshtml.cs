using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AlocaSenac.Web.Pages.Agendamentos;

public sealed class IndexModel(AlocacoesStore store) : PageModel
{
    public IReadOnlyList<AlocacaoResponse> Agendamentos { get; private set; } = [];

    public void OnGet()
    {
        Agendamentos = store.Listar();
    }

    public void OnPost()
    {
    }
}
