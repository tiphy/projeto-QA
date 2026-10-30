using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AlocaSenac.Web.Pages.Login;

public sealed class IndexModel : PageModel
{
    private const string EmailAdministrador = "admin@gmail.com";
    private const string SenhaAdministrador = "1234";

    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public string Senha { get; set; } = string.Empty;

    public string? Erro { get; private set; }

    public IActionResult OnGet()
    {
        return User.Identity?.IsAuthenticated == true
            ? RedirectToPage("/Dashboard/Index")
            : Page();
    }

    public async Task<IActionResult> OnPostLoginAsync()
    {
        if (!string.Equals(Email, EmailAdministrador, StringComparison.OrdinalIgnoreCase)
            || !string.Equals(Senha, SenhaAdministrador, StringComparison.Ordinal))
        {
            Erro = "E-mail ou senha inválidos.";
            return Page();
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "Administrador"),
            new Claim(ClaimTypes.Email, EmailAdministrador)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        return RedirectToPage("/Dashboard/Index");
    }
}
