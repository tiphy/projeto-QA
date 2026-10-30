using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AlocaSenac.Web.Pages.Salas;

public sealed class EditarModel : PageModel
{
    public int Id { get; private set; }

    public void OnGet(int id)
    {
        Id = id;
    }

    public void OnPost(int id)
    {
        Id = id;
    }
}
