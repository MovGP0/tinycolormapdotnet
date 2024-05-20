namespace TinyColorMap;

/// <summary>
/// Represents a color in the CIE LUV color space, also referred to as CIE 1976.
/// </summary>
/// <param name="L">Lightness</param>
/// <param name="U">U component (chromaticity)</param>
/// <param name="V">V component (chromaticity)</param>
public record LUV(double L, double U, double V);