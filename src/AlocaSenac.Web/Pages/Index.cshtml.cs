using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AlocaSenac.Web.Pages;

public sealed class IndexModel : PageModel
{
    public IActionResult OnGet()
    {
        return User.Identity?.IsAuthenticated == true
            ? RedirectToPage("/Dashboard/Index")
            : RedirectToPage("/Login/Index");
    }
}
