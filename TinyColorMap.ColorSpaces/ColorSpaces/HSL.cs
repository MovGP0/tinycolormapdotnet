namespace TinyColorMap;

/// <summary>
/// Represents a color in the Hue, Saturation, Lightness (HSL) color space.
/// </summary>
/// <param name="H">Hue: Represents the type of color and ranges from 0 to 360 degrees.</param>
/// <param name="S">Saturation: Represents the intensity of the color, ranging from 0.0 to 100.0.</param>
/// <param name="L">Lightness: Represents the brightness of the color, ranging from 0 (black) to 100 (white).</param>
public record HSL(double H, double S, double L);