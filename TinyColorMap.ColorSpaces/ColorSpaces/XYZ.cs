namespace TinyColorMap;

/// <summary>
/// Represents a color in the CIE XYZ color space.
/// </summary>
/// <remarks>XYZ is calculated relative to a given white point.</remarks>
/// <param name="X">X component</param>
/// <param name="Y">Y component</param>
/// <param name="Z">Z component</param>
public record XYZ(double X, double Y, double Z);