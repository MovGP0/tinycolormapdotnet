namespace TinyColorMap;

/// <summary>
/// Human-friendly HSL
/// </summary>
public static class ColorSpaceConverter
{
    private const double RefU = 0.19783000664283680764;
    private const double RefV = 0.46831999493879100370;
    private const double Kappa = 903.29629629629629629630;
    private const double Epsilon = 0.00885645167903563082;

    /// <summary>
    /// For RGB => XYZ
    /// </summary>
    private static Vector3[] M =
    [
        new( 3.24096994190452134377, -1.53738317757009345794, -0.49861076029300328366),
        new(-0.96924363628087982613,  1.87596750150772066772,  0.04155505740717561247),
        new( 0.05563007969699360846, -0.20397695888897656435,  1.05697151424287856072)
    ];

    /// <summary>
    /// For XYZ => RGB
    /// </summary>
    private static Vector3[] MInv = [
        new(0.41239079926595948129,  0.35758433938387796373,  0.18048078840183428751),
        new(0.21263900587151035754,  0.71516867876775592746,  0.07219231536073371500),
        new(0.01933081871559185069,  0.11919477979462598791,  0.95053215224966058086)
    ];

    [Pure]
    private static Bounds[] GetBounds(double l)
    {
        var tl = l + 16.0;
        var sub1 = tl * tl * tl / 1560896.0;
        var sub2 = sub1 > Epsilon ? sub1 : l / Kappa;
        var bounds = new Bounds[6];

        for (var channel = 0; channel < 3; channel++)
        {
            var m1 = M[channel].a;
            var m2 = M[channel].b;
            var m3 = M[channel].c;

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
    {
        return (line1.High - line2.High) / (line2.Low - line1.Low);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double DistFromPoleSquared(double x, double y)
    {
        return x * x + y * y;
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double RayLengthUntilIntersect(double theta, Bounds line)
    {
        return line.High / (Math.Sin(theta) - line.Low * Math.Cos(theta));
    }

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

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LCH Hsluv2Lch(HSL hsl)
    {
        var (h, s, l) = Hsluv2Lch(hsl.H, hsl.S, hsl.L);
        return new(h, s, l);
    }

    [Pure]
    public static (double l, double c, double h) Hsluv2Lch(double h, double s, double l)
    {
        double c;

        if (l is >= 100 or < 0.000_000_01)
        {
            c = 0.0;
        }
        else
        {
            c = MaxChromaForLh(l, h) / 100.0 * s;
        }

        if (s < 0.000_000_01)
        {
            h = 0.0;
        }

        return (l, c, h);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LUV Lch2Luv(LCH lch)
    {
        var (l, u, v) = Lch2Luv(lch.L, lch.C, lch.H);
        return new(l, u, v);
    }

    
    [Pure]
    public static (double l, double u, double v) Lch2Luv(double l, double c, double h)
    {
        var hrad = h * (Math.PI / 180.0); // Convert degrees to radians
        var u = Math.Cos(hrad) * c;
        var v = Math.Sin(hrad) * c;
        return (l, u, v);
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
    public static XYZ Luv2Xyz(LUV luv)
    {
        var (x, y, z) = Luv2Xyz(luv.L, luv.U, luv.V);
        return new(x, y, z);
    }

    [Pure]
    public static (double x, double y, double z) Luv2Xyz(double l, double u, double v)
    {
        if (l <= 0.000_000_01)
        {
            return (0.0, 0.0, 0.0);
        }

        var var_u = u / (13.0 * l) + RefU;
        var var_v = v / (13.0 * l) + RefV;
        var y = L2Y(l);
        var x = -(9.0 * y * var_u) / ((var_u - 4.0) * var_v - var_u * var_v);
        var z = (9.0 * y - 15.0 * var_v * y - var_v * x) / (3.0 * var_v);

        return (x, y, z);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RGB Xyz2Rgb(XYZ xyz)
    {
        var (r, g, b) = Xyz2Rgb(xyz.X, xyz.Y, xyz.Z);
        return new(r, g, b);
    }

    [Pure]
    public static (double r, double g, double b) Xyz2Rgb(double x, double y, double z)
    {
        var t = new Vector3(x, y, z);
        var r = FromLinear(DotProduct(M[0], t));
        var g = FromLinear(DotProduct(M[1], t));
        var b = FromLinear(DotProduct(M[2], t));
        return (r, g, b);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double DotProduct(Vector3 t1, Vector3 t2)
    {
        return t1.a * t2.a + t1.b * t2.b + t1.c * t2.c;
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
    public static RGB Hsluv2Rgb(HSL hsl)
    {
        var (r, g, b) = Hsluv2Rgb(hsl.H, hsl.S, hsl.L);
        return new(r, g, b);
    }

    /// <summary>
    /// Convert HSLuv to RGB.
    /// </summary>
    /// <param name="h">Hue. Between 0.0 and 360.0.</param>
    /// <param name="s">Saturation. Between 0.0 and 100.0.</param>
    /// <param name="l">Lightness. Between 0.0 and 100.0.</param>
    /// <returns>A tuple containing the RGB components: Red, Green, and Blue. Each component is between 0.0 and 1.0.</returns>
    [Pure]
    public static (double r, double g, double b) Hsluv2Rgb(double h, double s, double l)
    {
        var lch = Hsluv2Lch(h, s, l);
        var luv = Lch2Luv(lch.l, lch.c, lch.h);
        var xyz = Luv2Xyz(luv.l, luv.u, luv.v);
        return Xyz2Rgb(xyz.x, xyz.y, xyz.z);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XYZ Rgb2Xyz(RGB rgb)
    {
        var (x, y, z) = Rgb2Xyz(rgb.R, rgb.G, rgb.B);
        return new(x, y, z);
    }

    [Pure]
    public static (double x, double y, double z) Rgb2Xyz(double r, double g, double b)
    {
        var rgblR = ToLinear(r);
        var rgblG = ToLinear(g);
        var rgblB = ToLinear(b);
        var x = DotProduct(new Vector3(MInv[0].a, MInv[0].b, MInv[0].c), new Vector3(rgblR, rgblG, rgblB));
        var y = DotProduct(new Vector3(MInv[1].a, MInv[1].b, MInv[1].c), new Vector3(rgblR, rgblG, rgblB));
        var z = DotProduct(new Vector3(MInv[2].a, MInv[2].b, MInv[2].c), new Vector3(rgblR, rgblG, rgblB));

        return (x, y, z);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HSL Rgb2Hsluv(RGB rgb)
    {
        var (h, s, l) = Rgb2Hsluv(rgb.R, rgb.G, rgb.B);
        return new(h, s, l);
    }

    /// <summary>
    /// Convert RGB to HSLuv.
    /// </summary>
    /// <param name="r">Red component. Between 0.0 and 1.0.</param>
    /// <param name="g">Green component. Between 0.0 and 1.0.</param>
    /// <param name="b">Blue component. Between 0.0 and 1.0.</param>
    /// <returns>A tuple containing the HSLuv components: Hue, Saturation, and Lightness. Each component has specific ranges.</returns>
    [Pure]
    public static (double h, double s, double l) Rgb2Hsluv(double r, double g, double b)
    {
        var xyz = Rgb2Xyz(r, g, b);
        var luv = Xyz2Luv(xyz.x, xyz.y, xyz.z);
        var lch = Luv2Lch(luv.l, luv.u, luv.v);
        var hsluv = Lch2Hsluv(lch.l, lch.c, lch.h);
        return (hsluv.h, hsluv.s, hsluv.l);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RGB Hpluv2Rgb(HSL hsl)
    {
        var (r, g, b) = Hpluv2Rgb(hsl.H, hsl.S, hsl.L);
        return new(r, g, b);
    }

    /// <summary>
    /// Convert HPLuv to RGB.
    /// </summary>
    /// <param name="h">Hue. Between 0.0 and 360.0.</param>
    /// <param name="s">Saturation. Between 0.0 and 100.0.</param>
    /// <param name="l">Lightness. Between 0.0 and 100.0.</param>
    /// <returns>A tuple containing the RGB components: Red, Green, and Blue. Each component is between 0.0 and 1.0.</returns>
    [Pure]
    public static (double r, double g, double b) Hpluv2Rgb(double h, double s, double l)
    {
        var lch = Hpluv2Lch(h, s, l);
        var luv = Lch2Luv(lch.l, lch.c, lch.h);
        var xyz = Luv2Xyz(luv.l, luv.u, luv.v);
        return Xyz2Rgb(xyz.x, xyz.y, xyz.z);
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

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static LCH Hpluv2Lch(HSL hsl)
    {
        var (h, s, l) = Hpluv2Lch(hsl.H, hsl.S, hsl.L);
        return new(h, s, l);
    }
    
    [Pure]
    private static (double l, double c, double h) Hpluv2Lch(double h, double s, double l)
    {
        double c;
        // White and black: disambiguate chroma
        if (l is >= 100 or < 0.000_000_01)
        {
            c = 0.0;
        }
        else
        {
            c = MaxSafeChromaForL(l) / 100.0 * s;
        }

        // Grays: disambiguate hue
        if (s < 0.000_000_01)
        {
            h = 0.0;
        }

        return (l, c, h);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HSL Rgb2Hpluv(RGB rgb)
    {
        var (h, s, l) = Rgb2Hpluv(rgb.R, rgb.G, rgb.B);
        return new(h, s, l);
    }

    /// <summary>
    /// Convert RGB to HPLuv.
    /// </summary>
    /// <remarks>
    /// Note that HPLuv does not contain all the colors of RGB, so converting
    /// arbitrary RGB to it may generate invalid HPLuv colors.
    /// </remarks>
    /// <param name="r">Red component. Between 0.0 and 1.0.</param>
    /// <param name="g">Green component. Between 0.0 and 1.0.</param>
    /// <param name="b">Blue component. Between 0.0 and 1.0.</param>
    /// <returns>A tuple containing the HPLuv components: Hue, Saturation, and Lightness. Each component has specific ranges.</returns>
    [Pure]
    public static (double h, double s, double l) Rgb2Hpluv(double r, double g, double b)
    {
        var xyz = Rgb2Xyz(r, g, b);
        var luv = Xyz2Luv(xyz.x, xyz.y, xyz.z);
        var lch = Luv2Lch(luv.l, luv.u, luv.v);
        var hpluv = Lch2Hpluv(lch.l, lch.c, lch.h);
        return (hpluv.h, hpluv.s, hpluv.l);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RGB Lch2Hpluv(LCH lch)
    {
        var (r, g, b) = Lch2Hpluv(lch.L, lch.C, lch.H);
        return new(r, g, b);
    }

    [Pure]
    public static (double h, double s, double l) Lch2Hpluv(double l, double c, double h)
    {
        double s;
        // White and black: disambiguate saturation
        if (l is >= 100.0 or < 0.000_000_01)
        {
            s = 0.0;
        }
        else
        {
            s = c / MaxSafeChromaForL(l) * 100.0;
        }

        // Grays: disambiguate hue
        if (c < 0.000_000_01)
        {
            h = 0.0;
        }

        return (h, s, l);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LCH Luv2Lch(LUV luv)
    {
        var (l, c, h) = Luv2Lch(luv.L, luv.U, luv.V);
        return new(l, c, h);
    }

    [Pure]
    public static (double l, double c, double h) Luv2Lch(double l, double u, double v)
    {
        var c = Math.Sqrt(u * u + v * v);
        var h = c < 0.000_000_01 ? 0.0 : Math.Atan2(v, u) * (180 / Math.PI);
        if (h < 0.0)
        {
            h += 360.0;
        }

        return (l, c, h);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HSL Lch2Hsluv(LCH lch)
    {
        var (h, s, l) = Lch2Hsluv(lch.L, lch.C, lch.H);
        return new(h, s, l);
    }

    [Pure]
    public static (double h, double s, double l) Lch2Hsluv(double l, double c, double h)
    {
        var s = l is >= 100.0 or < 0.000_000_01
            ? 0.0
            : c / MaxChromaForLh(l, h) * 100.0;

        if (c < 0.000_000_01)
        {
            h = 0.0;
        }

        return (h, s, l);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LUV Xyz2Luv(XYZ xyz)
    {
        var (l, u, v) = Xyz2Luv(xyz.X, xyz.Y, xyz.Z);
        return new(l, u, v);
    }

    [Pure]
    public static (double l, double u, double v) Xyz2Luv(double x, double y, double z)
    {
        var varU = 4.0 * x / (x + 15.0 * y + 3.0 * z);
        var varV = 9.0 * y / (x + 15.0 * y + 3.0 * z);
        var l = Y2L(y);
        var u = 13.0 * l * (varU - RefU);
        var v = 13.0 * l * (varV - RefV);

        return (l, u, v);
    }
}
