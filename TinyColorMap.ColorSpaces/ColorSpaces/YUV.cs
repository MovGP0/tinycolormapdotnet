namespace TinyColorMap;

/// <summary>
/// Represents a color in the YUV color space.
/// </summary>
/// <param name="Y">Luminance</param>
/// <param name="U">Chrominance (blue projection)</param>
/// <param name="V">Chrominance (red projection)</param>
public record YUV(double Y, double U, double V);