namespace TinyColorMap;

/// <summary>
/// Represents a color in the Hue, Perceived Lightness, Uniform (HPLuv) color space.
/// </summary>
/// <param name="H">Hue: Represents the type of color and ranges from 0 to 360 degrees.</param>
/// <param name="P">Perceived Lightness: Represents the lightness perceived by human vision, ranging from 0 to 100.</param>
/// <param name="C">Chroma: Represents the colorfulness relative to the brightness of another color that appears white under similar viewing conditions, adjusted for perceptual uniformity.</param>
public record HPLuv(double H, double P, double C);