namespace TinyColorMap;

/// <summary>
/// Represents a color in the Cyan, Magenta, Yellow, Key (Black) (CMYK) color space.
/// </summary>
/// <param name="C">Cyan component</param>
/// <param name="M">Magenta component</param>
/// <param name="Y">Yellow component</param>
/// <param name="K">Key (Black) component</param>
public record CMYK(double C, double M, double Y, double K);