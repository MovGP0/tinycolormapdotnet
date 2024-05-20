namespace TinyColorMap;

/// <summary>
/// Represents a color in the Hue, Saturation, Value (HSV) color space.
/// </summary>
/// <param name="H">Hue</param>
/// <param name="S">Saturation</param>
/// <param name="V">Value (Brightness)</param>
public record HSV(double H, double S, double V);