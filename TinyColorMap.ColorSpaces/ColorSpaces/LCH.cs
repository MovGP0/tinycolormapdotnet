namespace TinyColorMap;

/// <summary>
/// Represents a color in the CIE LCh(uv) color model, also known as HCL.
/// </summary>
/// <remarks>The cylindrical representation of LUV.</remarks>
/// <param name="L">Lightness</param>
/// <param name="C">Chroma</param>
/// <param name="H">Hue</param>
public record LCH(double L, double C, double H);