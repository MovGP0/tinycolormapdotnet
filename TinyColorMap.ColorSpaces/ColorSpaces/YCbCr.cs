namespace TinyColorMap;

/// <summary>
/// Represents a color in the YCbCr color space.
/// </summary>
/// <param name="Y">Luminance</param>
/// <param name="Cb">Blue-difference chroma component</param>
/// <param name="Cr">Red-difference chroma component</param>
public record YCbCr(double Y, double Cb, double Cr);