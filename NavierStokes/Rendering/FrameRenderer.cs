using FFMpegCore;
using FFMpegCore.Pipes;

namespace NavierStokes.Rendering;

/// <summary>
/// Converts a list of video frames to a video file.
/// </summary>
/// <remarks>Source: <a href="https://swharden.com/csdv/skiasharp/video/">Scott W. Harden: Render Video with SkiaSharp</a></remarks>
public static class FrameRenderer
{
    public static async Task<bool> RenderFramesToFileAsync(IEnumerable<IVideoFrame> frames, string filePath)
    {
        if (Path.GetExtension(filePath) != "webm")
        {
            throw new ArgumentException(Messages.FileMustBeInWebmFormat, nameof(filePath));
        }

        RawVideoPipeSource videoFramesSource = new(frames) { FrameRate = 30 };
        return await FFMpegArguments
            .FromPipeInput(videoFramesSource)
            .OutputToFile(
                filePath,
                overwrite: true,
                options => options.WithVideoCodec("libvpx-vp9"))
            .ProcessAsynchronously();
    }
}