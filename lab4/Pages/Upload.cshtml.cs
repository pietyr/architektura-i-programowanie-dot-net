using ImageMagick;
using lab4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace lab4.Pages;

public class UploadModel : PageModel
{
    private const long MaxUploadBytes = 1 * 1024 * 1024;

    private readonly string _imagesDir;
    private readonly WatermarkService _watermarkService;

    public UploadModel(IWebHostEnvironment environment, WatermarkService watermarkService)
    {
        _imagesDir = Path.Combine(environment.WebRootPath, "images");
        Directory.CreateDirectory(_imagesDir);
        _watermarkService = watermarkService;
    }

    [BindProperty]
    public IFormFile? Upload { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Upload is null)
        {
            ModelState.AddModelError(nameof(Upload), "Wybierz plik do wysłania.");
            return Page();
        }

        if (Upload.Length > MaxUploadBytes)
        {
            ModelState.AddModelError(nameof(Upload), "Plik jest za duży (max. 1 MB)");
            return Page();
        }

        string extension = ".jpg";
        switch (Upload.ContentType)
        {
            case "image/png":
                extension = ".png";
                break;
            case "image/gif":
                extension = ".gif";
                break;
        }

        var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + extension;
        var path = Path.Combine(_imagesDir, fileName);

        // Jeden odczyt ze strumienia uploadu + jeden asynchroniczny zapis na dysk
        // (bez pośredniego zapisu „surowego” pliku).
        await using Stream uploadStream = Upload.OpenReadStream();
        using var image = new MagickImage(uploadStream);
        using IMagickImage<byte> watermark = _watermarkService.CreateWatermarkClone();
        image.Composite(watermark, Gravity.Southeast, CompositeOperator.Over);
        await image.WriteAsync(path);

        return RedirectToPage("Index");
    }
}
