using FFMpegCore.Pipes;
using SkiaSharp;

namespace NavierStokes.Rendering;

/// <summary>
/// Converts an SKBitmap into a video frame.
/// </summary>
/// <remarks>Source: <a href="https://swharden.com/csdv/skiasharp/video/">Scott W. Harden: Render Video with SkiaSharp</a></remarks>
public sealed class SKBitmapFrame : IVideoFrame, IDisposable
{
    public int Width => _source.Width;
    public int Height => _source.Height;
    public string Format => "bgra";

    private readonly SKBitmap _source;

    public SKBitmapFrame(SKBitmap bitmap)
    {
        if (bitmap.ColorType != SKColorType.Bgra8888)
            throw new NotImplementedException("only 'bgra' color type is supported");
        _source = bitmap;
    }

    #region IVideoFrame

    public void Serialize(Stream pipe)
        => pipe.Write(_source.Bytes, 0, _source.Bytes.Length);

    public Task SerializeAsync(Stream pipe, CancellationToken token)
        => pipe.WriteAsync(_source.Bytes, 0, _source.Bytes.Length, token);

    #endregion

    #region IDisposable

    private bool _isDisposed;

    public void Dispose() => Dispose(true);

    ~SKBitmapFrame() => Dispose(false);

    private void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        _source.Dispose();

        if (disposing)
        {
            GC.SuppressFinalize(this);
        }

        _isDisposed = true;
    }

    #endregion
}