using TinyColorMap.Palettes;
using static TinyColorMap.Utils;

namespace TinyColorMap;

public readonly struct Color
{
    [Pure]
    public double R { get; }

    [Pure]
    public double G { get; }

    [Pure]
    public double B { get; }

    public Color(double gray)
    {
        R = gray;
        G = gray;
        B = gray;
    }

    public Color(double r, double g, double b)
    {
        R = r;
        G = g;
        B = b;
    }

    /// <summary>
    /// The color red.
    /// </summary>
    public static readonly Color Red = new(1.0, 0.0, 0.0);
    
    /// <summary>
    /// The color green.
    /// </summary>
    public static readonly Color Green = new(0.0, 1.0, 0.0);

    /// <summary>
    /// The color blue.
    /// </summary>
    public static readonly Color Blue = new(0.0, 0.0, 1.0);

    /// <summary>
    /// Returns the Red channel as an integer from 0 to 255.
    /// </summary>
    [Pure]
    public byte Ri => (byte)(Clamp(R) * 255.0);

    /// <summary>
    /// Returns the Green channel as an integer from 0 to 255.
    /// </summary>
    [Pure]
    public byte Gi => (byte)(Clamp(G) * 255.0);

    /// <summary>
    /// Returns the Blue channel as an integer from 0 to 255.
    /// </summary>
    [Pure]
    public byte Bi => (byte)(Clamp(B) * 255.0);

    [Pure]
    public static Color operator +(Color c1, Color c2) =>
        new(c1.R + c2.R, c1.G + c2.G, c1.B + c2.B);

    [Pure]
    public static Color operator +(Color c, double scalar) =>
        new(c.R + scalar, c.G + scalar, c.B + scalar);

    [Pure]
    public static Color operator *(double scalar, Color c) =>
        new(c.R * scalar, c.G * scalar, c.B * scalar);

    [Pure]
    public static Color operator *(Color c, double scalar) =>
        new(c.R * scalar, c.G * scalar, c.B * scalar);

    [Pure]
    public static Color operator /(Color c, double scalar) =>
        new(c.R / scalar, c.G / scalar, c.B / scalar);

    [Pure]
    public void Deconstruct(out byte r, out byte g, out byte b)
    {
        r = Ri;
        g = Gi;
        b = Bi;
    }

    [Pure]
    public static Color GetColor(double x, ColormapType type = ColormapType.Viridis)
    {
        return type switch
        {
            ColormapType.Parula => GetParulaColor(x),
            ColormapType.Heat => GetHeatColor(x),
            ColormapType.Jet => GetJetColor(x),
            ColormapType.Turbo => GetTurboColor(x),
            ColormapType.Hot => GetHotColor(x),
            ColormapType.Gray => GetGrayColor(x),
            ColormapType.Magma => GetMagmaColor(x),
            ColormapType.Inferno => GetInfernoColor(x),
            ColormapType.Plasma => GetPlasmaColor(x),
            ColormapType.Viridis => GetViridisColor(x),
            ColormapType.Cividis => GetCividisColor(x),
            ColormapType.Github => GetGithubColor(x),
            ColormapType.Cubehelix => GetCubehelixColor(x),
            ColormapType.HSV => GetHSVColor(x),
            _ => GetViridisColor(x)
        };
    }

    [Pure]
    public static Color GetColor(double x, uint numLevels, ColormapType type = ColormapType.Viridis)
    {
        double xx = QuantizeArgument(x, numLevels);
        return GetColor(xx, type);
    }

    [Pure]
    public static Color GetParulaColor(double x) => CalcLerp(x, Parula.Colors);

    [Pure]
    public static Color GetHeatColor(double x) => CalcLerp(x, Heat.Colors);

    [Pure]
    public static Color GetJetColor(double x) => CalcLerp(x, Jet.Colors);

    [Pure]
    public static Color GetTurboColor(double x) => CalcLerp(x, Turbo.Colors);

    [Pure]
    public static Color GetHotColor(double x)
    {
        x = Clamp(x);

        switch (x)
        {
            case < 0.4:
            {
                double t = x / 0.4;
                return t * Red;
            }
            case < 0.8:
            {
                double t = (x - 0.4) / (0.8 - 0.4);
                return Red + t * Green;
            }
            default:
            {
                double t = (x - 0.8) / (1.0 - 0.8);
                return Red + Green + t * Blue;
            }
        }
    }

    [Pure]
    public static Color GetGrayColor(double x) => new(1.0 - Clamp(x));

    [Pure]
    public static Color GetMagmaColor(double x) => CalcLerp(x, Magma.Colors);

    [Pure]
    public static Color GetInfernoColor(double x) => CalcLerp(x, Inferno.Colors);

    [Pure]
    public static Color GetPlasmaColor(double x) => CalcLerp(x, Plasma.Colors);

    [Pure]
    public static Color GetViridisColor(double x) => CalcLerp(x, Viridis.Colors);

    [Pure]
    public static Color GetCividisColor(double x) => CalcLerp(x, Cividis.Colors);

    [Pure]
    public static Color GetGithubColor(double x) => CalcLerp(x, Github.Colors);

    [Pure]
    public static Color GetCubehelixColor(double x) => CalcLerp(x, Cubehelix.Colors);

    [Pure]
    public static Color GetHSVColor(double x) => CalcLerp(x, HSV.Colors);
}
