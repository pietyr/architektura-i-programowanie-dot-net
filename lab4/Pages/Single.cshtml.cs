using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace lab4.Pages;

public class SingleModel : PageModel
{
    private readonly string _imagesDir;

    public SingleModel(IWebHostEnvironment environment)
    {
        _imagesDir = Path.Combine(environment.WebRootPath, "images");
        Directory.CreateDirectory(_imagesDir);
    }

    [BindProperty(SupportsGet = true)]
    public string Image { get; set; } = string.Empty;

    public IActionResult OnGet()
    {
        if (string.IsNullOrWhiteSpace(Image))
        {
            return NotFound();
        }

        // Unikaj path traversal – tylko nazwa pliku
        var safeName = Path.GetFileName(Image);
        if (!string.Equals(safeName, Image, StringComparison.Ordinal))
        {
            return NotFound();
        }

        if (System.IO.File.Exists(Path.Combine(_imagesDir, safeName)))
        {
            return Page();
        }

        return NotFound();
    }
}
