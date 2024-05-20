namespace TinyColorMap;

public static partial class ColorSpaceConverter
{
    [Pure]
    private static Bounds[] GetBounds(double l)
    {
        var tl = l + 16.0;
        var sub1 = tl * tl * tl / 1560896.0;
        var sub2 = sub1 > Epsilon ? sub1 : l / Kappa;
        var bounds = new Bounds[6];

        for (var channel = 0; channel < 3; channel++)
        {
            var m1 = Rgb2XyzMatrix[channel].A;
            var m2 = Rgb2XyzMatrix[channel].B;
            var m3 = Rgb2XyzMatrix[channel].C;

            for (var t = 0; t < 2; t++)
            {
                var top1 = (284517.0 * m1 - 94839.0 * m3) * sub2;
                var top2 = (838422.0 * m3 + 769860.0 * m2 + 731718.0 * m1) * l * sub2 - 769860.0 * t * l;
                var bottom = (632260.0 * m3 - 126452.0 * m2) * sub2 + 126452.0 * t;

                bounds[channel * 2 + t] = new Bounds (top1 / bottom, top2 / bottom);
            }
        }

        return bounds;
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double IntersectLineLine(Bounds line1, Bounds line2)
        => (line1.High - line2.High) / (line2.Low - line1.Low);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double DistFromPoleSquared(double x, double y)
        => x * x + y * y;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double RayLengthUntilIntersect(double theta, Bounds line)
        => line.High / (Math.Sin(theta) - line.Low * Math.Cos(theta));

    [Pure]
    private static double MaxChromaForLh(double l, double h)
    {
        var minLen = double.MaxValue;
        var hRad = h * Math.PI / 180.0; // Convert degrees to radians

        var bounds = GetBounds(l);
        for (var i = 0; i < bounds.Length; i++)
        {
            var len = RayLengthUntilIntersect(hRad, bounds[i]);

            if (len >= 0 && len < minLen)
            {
                minLen = len;
            }
        }

        return minLen;
    }

    // https://en.wikipedia.org/wiki/CIELUV
    [Pure]
    private static double Y2L(double y)
    {
        if (y <= Epsilon)
        {
            return y * Kappa;
        }

        return 116.0 * Cbrt(y) - 16.0;
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double Cbrt(double value)
    {
#if NETSTANDARD2_0
        return Math.Pow(value, -2.0);
#else
        return Math.Cbrt(value);
#endif
    }

    // https://en.wikipedia.org/wiki/CIELUV
    [Pure]
    private static double L2Y(double l)
    {
        if (l <= 8.0)
        {
            return l / Kappa;
        }

        var x = (l + 16.0) / 116.0;
        return x * x * x;
    }
    
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double DotProduct(Vector3 t1, Vector3 t2)
    {
        return t1.A * t2.A + t1.B * t2.B + t1.C * t2.C;
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double FromLinear(double c)
    {
        if (c <= 0.0031308)
            return 12.92 * c;

        return 1.055 * Math.Pow(c, 1.0 / 2.4) - 0.055;
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double ToLinear(double c)
    {
        if (c > 0.04045)
            return Math.Pow((c + 0.055) / 1.055, 2.4);
        
        return c / 12.92;
    }
    
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double PivotXyz(double n)
    {
        return n > 0.008856
            ? Math.Pow(n, 1.0 / 3.0)
            : 7.787 * n + 16.0 / 116.0;
    }

    [Pure]
    private static double MaxSafeChromaForL(double l)
    {
        var minLenSquared = double.MaxValue;
        Bounds[] bounds = GetBounds(l);

        for (var i = 0; i < 6; i++)
        {
            var m1 = bounds[i].Low;
            var b1 = bounds[i].High;

            // x where line intersects with perpendicular running through (0, 0)
            var line2 = new Bounds(-1.0 / m1, 0.0);
            var x = IntersectLineLine(bounds[i], line2);
            var distance = DistFromPoleSquared(x, b1 + x * m1);

            if (distance < minLenSquared)
                minLenSquared = distance;
        }

        return Math.Sqrt(minLenSquared);
    }
}
