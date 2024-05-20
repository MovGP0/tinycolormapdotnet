namespace TinyColorMap;

/// <summary>
/// Represents a color in the Red, Green, Blue (RGB) color space.
/// </summary>
/// <param name="R">Red component, ranging from 0.0 to 1.0.</param>
/// <param name="G">Green component, ranging from 0.0 to 1.0.</param>
/// <param name="B">Blue component, ranging from 0.0 to 1.0.</param>
public record RGB(double R, double G, double B);