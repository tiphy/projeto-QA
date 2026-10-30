using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public sealed class IndexModel(AlocacoesStore store) : PageModel
{
    private static readonly string[] PersonasValidas = ["Solicitante", "Responsável", "Administrador"];

    public IReadOnlyList<AlocacaoResponse> Alocacoes { get; private set; } = [];
    public string Persona { get; private set; } = "Solicitante";

    [TempData]
    public string? Aviso { get; set; }

    public void OnGet(string? persona)
    {
        Carregar(persona);
    }

    public IActionResult OnPostCreate(string? persona, string sala, string professor, DateTime inicio, DateTime fim)
    {
        Carregar(persona);

        if (string.IsNullOrWhiteSpace(sala))
        {
            ModelState.AddModelError(nameof(sala), "Informe a sala ou o espaço.");
        }

        if (string.IsNullOrWhiteSpace(professor))
        {
            ModelState.AddModelError(nameof(professor), "Informe o responsável pela atividade.");
        }

        if (inicio >= fim)
        {
            ModelState.AddModelError(string.Empty, "O início precisa ser anterior ao término.");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var request = new CriarAlocacaoRequest(sala.Trim(), professor.Trim(), inicio, fim);
        if (store.TentarCriar(request) is null)
        {
            ModelState.AddModelError(string.Empty, "Já existe uma alocação para essa sala nesse horário.");
            return Page();
        }

        Aviso = "Alocação criada.";
        return RedirectToPage(new { persona = Persona });
    }

    public IActionResult OnPostDelete(string? persona, Guid id)
    {
        Carregar(persona);
        if (Persona != "Administrador")
        {
            return Forbid();
        }

        Aviso = store.Remover(id) ? "Alocação removida." : "A alocação não foi encontrada.";
        return RedirectToPage(new { persona = Persona });
    }

    private void Carregar(string? persona)
    {
        Persona = PersonasValidas.Contains(persona, StringComparer.OrdinalIgnoreCase)
            ? PersonasValidas.First(validPersona => validPersona.Equals(persona, StringComparison.OrdinalIgnoreCase))
            : "Solicitante";
        Alocacoes = store.Listar();
    }
}
