namespace TinyColorMap;

/// <summary>
/// Human-friendly HSL
/// </summary>
public static class HSLUV
{
    private const double ref_u = 0.19783000664283680764;
    private const double ref_v = 0.46831999493879100370;
    private const double Kappa = 903.29629629629629629630;
    private const double Epsilon = 0.00885645167903563082;

    /// <summary>
    /// For RGB
    /// </summary>
    private static Triplet[] M =
    [
        new( 3.24096994190452134377, -1.53738317757009345794, -0.49861076029300328366),
        new(-0.96924363628087982613,  1.87596750150772066772,  0.04155505740717561247),
        new( 0.05563007969699360846, -0.20397695888897656435,  1.05697151424287856072)
    ];

    /// <summary>
    /// For XYZ
    /// </summary>
    private static Triplet[] MInv = [
        new(0.41239079926595948129,  0.35758433938387796373,  0.18048078840183428751),
        new(0.21263900587151035754,  0.71516867876775592746,  0.07219231536073371500),
        new(0.01933081871559185069,  0.11919477979462598791,  0.95053215224966058086)
    ];

    private static Bounds[] GetBounds(double l)
    {
        double tl = l + 16.0;
        double sub1 = tl * tl * tl / 1560896.0;
        double sub2 = sub1 > Epsilon ? sub1 : l / Kappa;
        var bounds = new Bounds[6];

        for (int channel = 0; channel < 3; channel++)
        {
            double m1 = M[channel].a;
            double m2 = M[channel].b;
            double m3 = M[channel].c;

            for (int t = 0; t < 2; t++)
            {
                double top1 = (284517.0 * m1 - 94839.0 * m3) * sub2;
                double top2 = (838422.0 * m3 + 769860.0 * m2 + 731718.0 * m1) * l * sub2 - 769860.0 * t * l;
                double bottom = (632260.0 * m3 - 126452.0 * m2) * sub2 + 126452.0 * t;

                bounds[channel * 2 + t] = new Bounds (top1 / bottom, top2 / bottom);
            }
        }

        return bounds;
    }

    private static double IntersectLineLine(Bounds line1, Bounds line2)
    {
        return (line1.b - line2.b) / (line2.a - line1.a);
    }

    private static double DistFromPoleSquared(double x, double y)
    {
        return x * x + y * y;
    }

    private static double RayLengthUntilIntersect(double theta, Bounds line)
    {
        return line.b / (Math.Sin(theta) - line.a * Math.Cos(theta));
    }

    private static double MaxChromaForLh(double l, double h)
    {
        double minLen = double.MaxValue;
        double hRad = h * Math.PI / 180.0; // Convert degrees to radians

        var bounds = GetBounds(l);
        for (int i = 0; i < bounds.Length; i++)
        {
            double len = RayLengthUntilIntersect(hRad, bounds[i]);

            if (len >= 0 && len < minLen)
                minLen = len;
        }
        return minLen;
    }

    public static (double l, double c, double h) HsluvToLch(double h, double s, double l)
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

    public static (double l, double u, double v) LchToLuv(double l, double c, double h)
    {
        double hrad = h * (Math.PI / 180.0); // Convert degrees to radians
        double u = Math.Cos(hrad) * c;
        double v = Math.Sin(hrad) * c;
        return (l, u, v);
    }

    // https://en.wikipedia.org/wiki/CIELUV
    private static double Y2L(double y)
    {
        if (y <= Epsilon)
        {
            return y * Kappa;
        }

        return 116.0 * Math.Cbrt(y) - 16.0;
    }

    // https://en.wikipedia.org/wiki/CIELUV
    private static double L2Y(double l)
    {
        if (l <= 8.0)
        {
            return l / Kappa;
        }

        double x = (l + 16.0) / 116.0;
        return x * x * x;
    }
    
    public static (double x, double y, double z) LuvToXyz(double l, double u, double v)
    {
        if (l <= 0.000_000_01)
        {
            return (0.0, 0.0, 0.0);
        }

        double var_u = u / (13.0 * l) + ref_u;
        double var_v = v / (13.0 * l) + ref_v;
        double y = L2Y(l);
        double x = -(9.0 * y * var_u) / ((var_u - 4.0) * var_v - var_u * var_v);
        double z = (9.0 * y - 15.0 * var_v * y - var_v * x) / (3.0 * var_v);

        return (x, y, z);
    }

    public static (double r, double g, double b) XyzToRgb(double x, double y, double z)
    {
        var t = new Triplet(x, y, z);
        double r = FromLinear(DotProduct(M[0], t));
        double g = FromLinear(DotProduct(M[1], t));
        double b = FromLinear(DotProduct(M[2], t));
        return (r, g, b);
    }

    private static double DotProduct(Triplet t1, Triplet t2)
    {
        return t1.a * t2.a + t1.b * t2.b + t1.c * t2.c;
    }

    private static double FromLinear(double c)
    {
        if (c <= 0.0031308)
            return 12.92 * c;

        return 1.055 * Math.Pow(c, 1.0 / 2.4) - 0.055;
    }

    private static double ToLinear(double c)
    {
        if (c > 0.04045)
            return Math.Pow((c + 0.055) / 1.055, 2.4);
        
        return c / 12.92;
    }

    /// <summary>
    /// Convert HSLuv to RGB.
    /// </summary>
    /// <param name="h">Hue. Between 0.0 and 360.0.</param>
    /// <param name="s">Saturation. Between 0.0 and 100.0.</param>
    /// <param name="l">Lightness. Between 0.0 and 100.0.</param>
    /// <returns>A tuple containing the RGB components: Red, Green, and Blue. Each component is between 0.0 and 1.0.</returns>
    public static (double r, double g, double b) hsluv2rgb(double h, double s, double l)
    {
        var lch = HsluvToLch(h, s, l);
        var luv = LchToLuv(lch.l, lch.c, lch.h);
        var xyz = LuvToXyz(luv.l, luv.u, luv.v);
        return XyzToRgb(xyz.x, xyz.y, xyz.z);
    }

    public static (double x, double y, double z) Rgb2Xyz(double r, double g, double b)
    {
        double rgblR = ToLinear(r);
        double rgblG = ToLinear(g);
        double rgblB = ToLinear(b);
        double x = DotProduct(new Triplet(MInv[0].a, MInv[0].b, MInv[0].c), new Triplet(rgblR, rgblG, rgblB));
        double y = DotProduct(new Triplet(MInv[1].a, MInv[1].b, MInv[1].c), new Triplet(rgblR, rgblG, rgblB));
        double z = DotProduct(new Triplet(MInv[2].a, MInv[2].b, MInv[2].c), new Triplet(rgblR, rgblG, rgblB));

        return (x, y, z);
    }

    /// <summary>
    /// Convert RGB to HSLuv.
    /// </summary>
    /// <param name="r">Red component. Between 0.0 and 1.0.</param>
    /// <param name="g">Green component. Between 0.0 and 1.0.</param>
    /// <param name="b">Blue component. Between 0.0 and 1.0.</param>
    /// <returns>A tuple containing the HSLuv components: Hue, Saturation, and Lightness. Each component has specific ranges.</returns>
    public static (double h, double s, double l) Rgb2Hsluv(double r, double g, double b)
    {
        var xyz = Rgb2Xyz(r, g, b);
        var luv = Xyz2Luv(xyz.x, xyz.y, xyz.z);
        var lch = Luv2Lch(luv.l, luv.u, luv.v);
        var hsluv = Lch2Hsluv(lch.l, lch.c, lch.h);
        return (hsluv.h, hsluv.s, hsluv.l);
    }

    /// <summary>
    /// Convert HPLuv to RGB.
    /// </summary>
    /// <param name="h">Hue. Between 0.0 and 360.0.</param>
    /// <param name="s">Saturation. Between 0.0 and 100.0.</param>
    /// <param name="l">Lightness. Between 0.0 and 100.0.</param>
    /// <returns>A tuple containing the RGB components: Red, Green, and Blue. Each component is between 0.0 and 1.0.</returns>
    public static (double r, double g, double b) hpluv2rgb(double h, double s, double l)
    {
        var lch = Hpluv2Lch(h, s, l);
        var luv = LchToLuv(lch.l, lch.c, lch.h);
        var xyz = LuvToXyz(luv.l, luv.u, luv.v);
        return XyzToRgb(xyz.x, xyz.y, xyz.z);
    }

    private static double MaxSafeChromaForL(double l)
    {
        double minLenSquared = double.MaxValue;
        Bounds[] bounds = GetBounds(l);

        for (int i = 0; i < 6; i++)
        {
            double m1 = bounds[i].a;
            double b1 = bounds[i].b;

            // x where line intersects with perpendicular running through (0, 0)
            Bounds line2 = new Bounds(-1.0 / m1, 0.0);
            double x = IntersectLineLine(bounds[i], line2);
            double distance = DistFromPoleSquared(x, b1 + x * m1);

            if (distance < minLenSquared)
                minLenSquared = distance;
        }

        return Math.Sqrt(minLenSquared);
    }

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
    public static (double h, double s, double l) rgb2hpluv(double r, double g, double b)
    {
        var xyz = Rgb2Xyz(r, g, b);
        var luv = Xyz2Luv(xyz.x, xyz.y, xyz.z);
        var lch = Luv2Lch(luv.l, luv.u, luv.v);
        var hpluv = Lch2Hpluv(lch.l, lch.c, lch.h);
        return (hpluv.h, hpluv.s, hpluv.l);
    }

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

    public static (double l, double c, double h) Luv2Lch(double l, double u, double v)
    {
        double c = Math.Sqrt(u * u + v * v);
        double h = c < 0.000_000_01 ? 0.0 : Math.Atan2(v, u) * (180 / Math.PI);
        if (h < 0.0)
        {
            h += 360.0;
        }

        return (l, c, h);
    }

    public static (double h, double s, double l) Lch2Hsluv(double l, double c, double h)
    {
        double s = l is >= 100.0 or < 0.000_000_01
            ? 0.0
            : c / MaxChromaForLh(l, h) * 100.0;

        if (c < 0.000_000_01)
        {
            h = 0.0;
        }

        return (h, s, l);
    }

    public static (double l, double u, double v) Xyz2Luv(double x, double y, double z)
    {
        double varU = 4.0 * x / (x + 15.0 * y + 3.0 * z);
        double varV = 9.0 * y / (x + 15.0 * y + 3.0 * z);
        double l = Y2L(y);
        double u = 13.0 * l * (varU - ref_u);
        double v = 13.0 * l * (varV - ref_v);

        return (l, u, v);
    }
}
