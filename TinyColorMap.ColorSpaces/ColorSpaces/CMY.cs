namespace TinyColorMap;

/// <summary>
/// Represents a color in the Cyan, Magenta, Yellow (CMY) color space.
/// </summary>
/// <param name="C">Cyan component</param>
/// <param name="M">Magenta component</param>
/// <param name="Y">Yellow component</param>
public record CMY(double C, double M, double Y);