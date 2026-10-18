using ImageMagick;

namespace lab4.Services;

/// <summary>
/// Ładuje znak wodny raz (singleton) i udostępnia klony gotowe do nałożenia.
/// </summary>
public sealed class WatermarkService : IDisposable
{
    private readonly MagickImage _watermark;
    private bool _disposed;

    public WatermarkService(IWebHostEnvironment environment)
    {
        string path = Path.Combine(environment.ContentRootPath, "watermark.png");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Brak pliku znaku wodnego.", path);
        }

        _watermark = new MagickImage(path);
        // przezroczystość znaku wodnego – raz przy starcie, nie przy każdym uploadzie
        _watermark.Evaluate(Channels.Alpha, EvaluateOperator.Divide, 4);
    }

    public IMagickImage<byte> CreateWatermarkClone()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return _watermark.Clone();
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _watermark.Dispose();
        _disposed = true;
    }
}
