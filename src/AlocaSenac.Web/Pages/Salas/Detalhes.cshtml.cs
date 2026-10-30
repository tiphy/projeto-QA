using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AlocaSenac.Web.Pages.Salas;

public sealed class DetalhesModel : PageModel
{
    public int Id { get; private set; }

    public void OnGet(int id)
    {
        Id = id;
    }
}
