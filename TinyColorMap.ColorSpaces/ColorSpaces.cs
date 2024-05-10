namespace TinyColorMap;

/// <summary>
/// Hue, Saturation, Lightness
/// </summary>
/// <param name="H">Hue</param>
/// <param name="S">Saturation</param>
/// <param name="L">Lightness</param>
public record HSL(double H, double S, double L);

/// <summary>
/// CIE LCh(uv) color model, a.k.a. HCL.
/// </summary>
/// <remarks>the cylindrical representation of LUV</remarks>
/// <param name="L">Lightness</param>
/// <param name="C">Chroma</param>
/// <param name="H">Hue</param>
public record LCH(double L, double C, double H);

/// <summary>
/// Red, Green, Blue
/// </summary>
/// <param name="R">Red</param>
/// <param name="G">Green</param>
/// <param name="B">Blue</param>
public record RGB(double R, double G, double B);

/// <summary>
/// The CIE XYZ color model
/// </summary>
/// <remarks>XYZ is calculated relative to a given white.</remarks>
/// <param name="X"></param>
/// <param name="Y"></param>
/// <param name="Z"></param>
public record XYZ(double X, double Y, double Z);

/// <summary>
/// The CIE LUV color space, also referred to as CIE 1976.
/// </summary>
/// <param name="L"></param>
/// <param name="U"></param>
/// <param name="V"></param>
public record LUV(double L, double U, double V);

/// <summary>
/// Hue, Whiteness, Blackness
/// </summary>
/// <param name="H"></param>
/// <param name="W"></param>
/// <param name="B"></param>
public record HWB(double H, double W, double B);