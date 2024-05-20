namespace TinyColorMap;

/// <summary>
/// Represents a color in the CIE Lab color space.
/// </summary>
/// <param name="L">Lightness</param>
/// <param name="A">A component (green-red axis)</param>
/// <param name="B">B component (blue-yellow axis)</param>
public record LAB(double L, double A, double B);