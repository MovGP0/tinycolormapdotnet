namespace TinyColorMap;

/// <summary>
/// Generator for creating color gradients.
/// </summary>
public sealed class GradientGenerator
{
    private const int Segments = 4;
    private const int DataClasses = 7;
    private const double PStep = 1.0 / DataClasses;

    /// <summary>
    /// Creates a sequence of gradients based on the specified parameters.
    /// </summary>
    /// <param name="generalType">The general type of the gradient.</param>
    /// <param name="joiningType">The type of gradient joining.</param>
    /// <param name="hue">The base hue of the gradient.</param>
    /// <param name="saturation">The saturation level of the gradient.</param>
    /// <param name="numberOfColors">The number of colors in the gradient.</param>
    /// <returns>Enumerable of tuples containing the position and RGB color of each gradient step.</returns>
    [Pure]
    public static IEnumerable<(double position, RGB color)> CreateGradient(
        GradientGeneralType generalType,
        GradientJoiningType joiningType,
        double hue,
        double saturation,
        int numberOfColors)
    {
        var complementaryHue = GetComplementaryHue(hue);

        for (int i = 0; i < numberOfColors; ++i)
        {
            double position = (double)i / numberOfColors;
            yield return CalculateColorAndModifiedPosition(position, generalType, joiningType, hue, saturation, complementaryHue);
        }
    }

    /// <summary>
    /// Calculates the modified position and the corresponding RGB color for a given position.
    /// </summary>
    /// <param name="position">The position in the gradient.</param>
    /// <param name="generalType">The general type of the gradient.</param>
    /// <param name="joiningType">The type of gradient joining.</param>
    /// <param name="hue">The base hue of the gradient.</param>
    /// <param name="saturation">The saturation level of the gradient.</param>
    /// <param name="complementaryHue">The complementary hue of the base hue.</param>
    /// <returns>A tuple containing the modified position and the RGB color.</returns>
    [Pure]
    private static (double mp, RGB rgb) CalculateColorAndModifiedPosition(
        double position,
        GradientGeneralType generalType,
        GradientJoiningType joiningType,
        double hue,
        double saturation,
        double complementaryHue)
    {
        double mh = hue;
        double modifiedPosition = ModifyPosition(position, generalType, joiningType);
        if (joiningType == GradientJoiningType.Diverging && position > 0.5)
        {
            mh = complementaryHue;
        }

        var rgb = SequentialMapVaryingLightnessSingleHue(modifiedPosition, mh, saturation);
        return (modifiedPosition, rgb);
    }

    /// <summary>
    /// Maps a position to an RGB color with varying lightness and a single hue.
    /// </summary>
    /// <param name="position">The position in the gradient.</param>
    /// <param name="hue">The hue of the color.</param>
    /// <param name="saturation">The saturation level of the color.</param>
    /// <returns>The RGB color at the given position.</returns>
    [Pure]
    private static RGB SequentialMapVaryingLightnessSingleHue(
        double position,
        double hue,
        double saturation)
    {
        double lightness = position * 100.0;
        return ColorSpaceConverter.Hsluv2Rgb(new(hue, saturation, lightness));
    }

    /// <summary>
    /// Modifies the position in the gradient based on the general and joining types.
    /// </summary>
    /// <param name="position">The position in the gradient.</param>
    /// <param name="generalType">The general type of the gradient.</param>
    /// <param name="joiningType">The type of gradient joining.</param>
    /// <returns>The modified position.</returns>
    [Pure]
    private static double ModifyPosition(
        double position,
        GradientGeneralType generalType,
        GradientJoiningType joiningType)
    {
        double p = position;

        switch (joiningType)
        {
            case GradientJoiningType.No:
                break;
            case GradientJoiningType.Steps:
                p *= Segments;
                p = Frac(p);
                break;
            case GradientJoiningType.Tubes:
                p *= Segments;
                int ip = (int)p;
                p = p - ip;
                if (ip % 2 != 0) p = 1.0 - p;
                break;
            case GradientJoiningType.Diverging:
                p *= 2.0;
                if (p > 1.0) p = 2.0 - p;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(joiningType), joiningType, null);
        }

        if (generalType == GradientGeneralType.Discrete)
        {
            p = Double2Steps(p);
        }

        return p;
    }

    /// <summary>
    /// Converts a double value to a step value based on the predefined step size.
    /// </summary>
    /// <param name="p">The double value to convert.</param>
    /// <returns>The step value.</returns>
    [Pure]
    private static double Double2Steps(double p)
    {
        double s = p / PStep;
        s -= Frac(s);
        s *= PStep;
        return s;
    }

    /// <summary>
    /// Returns the fractional part (digits after the comma) of a double.
    /// </summary>
    /// <param name="d">The double value.</param>
    /// <returns>The fractional part of the double value.</returns>
    [Pure]
    private static double Frac(double d) => d - Math.Floor(d);

    /// <summary>
    /// Calculates the complementary hue of a given hue.
    /// </summary>
    /// <param name="hue">The base hue.</param>
    /// <returns>The complementary hue.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the hue is out of the valid range [0, 360].</exception>
    [Pure]
    private static double GetComplementaryHue(double hue)
    {
        if (hue is < 0.0 or > 360.0)
            throw new ArgumentOutOfRangeException(nameof(hue), hue, "Hue must be in the range [0, 360].");

        double complementaryHue = 180.0 + hue;
        if (complementaryHue > 360.0)
        {
            complementaryHue -= 360.0;
        }

        return complementaryHue;
    }
}
