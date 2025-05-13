namespace TinyColorMap;

/// <summary>
/// Represents a color in the YUV color space.
/// </summary>
/// <remarks>
/// See <a href="https://en.wikipedia.org/?title=ITU-R_BT.470-6">Wikipedia: ITU-R BT.470</a> for details.
/// </remarks>
/// <param name="Y">Luminance</param>
/// <param name="U">Chrominance (blue projection)</param>
/// <param name="V">Chrominance (red projection)</param>
public record YUV(double Y, double U, double V);