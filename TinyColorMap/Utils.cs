using System.Runtime.CompilerServices;

namespace TinyColorMap;

internal static class Utils
{
    /// <summary>
    /// Limits a value to the range [0, 1].
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Clamp(double x, double min = 0.0, double max = 1.0)
        => x < min ? min : x > max ? max : x;

    /// <summary>
    /// Calculates the linear interpolation of a value in the range [0, 1] using the given data.
    /// </summary>
    [Pure]
    public static Color CalcLerp(double x, ImmutableArray<Color> data)
    {
        var a = Clamp(x) * (data.Length - 1);
        var i = (int)Math.Floor(a);
        var t = a - i;
        var c0 = data[i];
        var c1 = data[Math.Min(i + 1, data.Length - 1)];

        return (1.0 - t) * c0 + t * c1;
    }

    /// <summary>
    /// Quantizes the given value to the given number of levels.
    /// </summary>
    [Pure]
    public static double QuantizeArgument(double x, uint numLevels)
    {
        // Clamp numLevels to range [1, 255].
        numLevels = Math.Max(1u, Math.Min(numLevels, 255u));

        var intervalLength = 255.0 / numLevels;

        // Calculate index of the interval to which the given x belongs to.
        // Subtracting a small epsilon prevents getting out of bounds index.
        var eps = 0.0005;
        var index = (uint)((x * 255.0 - eps) / intervalLength);

        // Calculate upper and lower bounds of the given interval.
        var upperBoundary = (uint)(index * intervalLength + intervalLength);
        var lowerBoundary = (uint)(upperBoundary - intervalLength);

        // Get middle "coordinate" of the given interval and move it back to [0.0, 1.0] interval.
        return ((double)(upperBoundary + lowerBoundary) * 0.5) / 255.0;
    }
}
