namespace TinyColorMap;

/// <summary>
/// Represents a color in the Hue, Whiteness, Blackness (HWB) color space.
/// </summary>
/// <param name="H">Hue</param>
/// <param name="W">Whiteness</param>
/// <param name="B">Blackness</param>
public record HWB(double H, double W, double B);