namespace TinyColorMap;

public static partial class ColorSpaceConverter
{
    private const double RefU = 0.19783000664283680764;
    private const double RefV = 0.46831999493879100370;
    private const double Kappa = 903.29629629629629629630;
    private const double Epsilon = 0.00885645167903563082;

    /// <summary>
    /// For RGB => XYZ
    /// </summary>
    private static Vector3[] Rgb2XyzMatrix =
    [
        new( 3.24096994190452134377, -1.53738317757009345794, -0.49861076029300328366),
        new(-0.96924363628087982613,  1.87596750150772066772,  0.04155505740717561247),
        new( 0.05563007969699360846, -0.20397695888897656435,  1.05697151424287856072)
    ];

    /// <summary>
    /// For XYZ => RGB
    /// </summary>
    /// <remarks>Inverse matrix of <see cref="Rgb2XyzMatrix"/></remarks>
    private static Vector3[] Xyz2RgbMatrix = [
        new(0.41239079926595948129,  0.35758433938387796373,  0.18048078840183428751),
        new(0.21263900587151035754,  0.71516867876775592746,  0.07219231536073371500),
        new(0.01933081871559185069,  0.11919477979462598791,  0.95053215224966058086)
    ];

    /// <summary>
    /// Standard illuminant defined by the International Commission on Illumination (CIE).
    /// Specifically, it is a standard light source that represents average daylight
    /// with a correlated color temperature (CCT) of approximately 6504 Kelvin
    /// </summary>
    private static readonly double[] D65 = [95.047, 100.0, 108.883];
}
