using Microsoft.AspNetCore.Mvc.RazorPages;

namespace lab4.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly string _imagesDir;

    public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        _imagesDir = Path.Combine(environment.WebRootPath, "images");
        Directory.CreateDirectory(_imagesDir);
    }

    public List<string> Images { get; set; } = new();

    public void OnGet()
    {
        UpdateFileList();
    }

    private static readonly HashSet<string> ImageExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tif", ".tiff", ".webp"
    };

    private void UpdateFileList()
    {
        Images = new List<string>();

        foreach (var item in Directory.EnumerateFiles(_imagesDir))
        {
            string name = Path.GetFileName(item);
            if (ImageExtensions.Contains(Path.GetExtension(name)))
            {
                Images.Add(name);
            }
        }
    }
}
