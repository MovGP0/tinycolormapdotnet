namespace TinyColorMap;

/// <summary>
/// Human-friendly color space converter.
/// </summary>
public static partial class ColorSpaceConverter
{
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LCH Hsluv2Lch(HSLuv hsl)
    {
        (double h, double s, double l) = hsl;
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

        return new(l, c, h);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LUV Lch2Luv(LCH lch)
    {
        var (l, c, h) = lch;
        var hrad = h * (Math.PI / 180.0); // Convert degrees to radians
        var u = Math.Cos(hrad) * c;
        var v = Math.Sin(hrad) * c;
        return new (l, u, v);
    }

    [Pure]
    public static XYZ Luv2Xyz(LUV luv)
    {
        var (l, u, v) = luv;
        if (l <= 0.000_000_01)
        {
            return new(0.0, 0.0, 0.0);
        }

        var var_u = u / (13.0 * l) + RefU;
        var var_v = v / (13.0 * l) + RefV;
        var y = L2Y(l);
        var x = -(9.0 * y * var_u) / ((var_u - 4.0) * var_v - var_u * var_v);
        var z = (9.0 * y - 15.0 * var_v * y - var_v * x) / (3.0 * var_v);
        return new(x, y, z);
    }

    [Pure]
    public static RGB Xyz2Rgb(XYZ xyz)
    {
        var (x, y, z) = xyz;
        var t = new Vector3(x, y, z);
        var r = FromLinear(DotProduct(Rgb2XyzMatrix[0], t));
        var g = FromLinear(DotProduct(Rgb2XyzMatrix[1], t));
        var b = FromLinear(DotProduct(Rgb2XyzMatrix[2], t));
        return new(r, g, b);
    }

    /// <summary>
    /// Convert HSLuv to RGB.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RGB Hsluv2Rgb(HSLuv hsl)
    {
        var lch = Hsluv2Lch(hsl);
        var luv = Lch2Luv(lch);
        var xyz = Luv2Xyz(luv);
        return Xyz2Rgb(xyz);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XYZ Rgb2Xyz(RGB rgb)
    {
        var (r, g, b) = rgb;
        var rgblR = ToLinear(r);
        var rgblG = ToLinear(g);
        var rgblB = ToLinear(b);
        var x = DotProduct(new Vector3(Xyz2RgbMatrix[0].A, Xyz2RgbMatrix[0].B, Xyz2RgbMatrix[0].C), new Vector3(rgblR, rgblG, rgblB));
        var y = DotProduct(new Vector3(Xyz2RgbMatrix[1].A, Xyz2RgbMatrix[1].B, Xyz2RgbMatrix[1].C), new Vector3(rgblR, rgblG, rgblB));
        var z = DotProduct(new Vector3(Xyz2RgbMatrix[2].A, Xyz2RgbMatrix[2].B, Xyz2RgbMatrix[2].C), new Vector3(rgblR, rgblG, rgblB));

        return new(x, y, z);
    }

    /// <summary>
    /// Convert RGB to HSLuv.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HSLuv Rgb2Hsluv(RGB rgb)
    {
        var xyz = Rgb2Xyz(rgb);
        var luv = Xyz2Luv(xyz);
        var lch = Luv2Lch(luv);
        return Lch2Hsluv(lch);
    }

    /// <summary>
    /// Convert HPLuv to RGB.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RGB Hpluv2Rgb(HPLuv hpc)
    {
        var lch = Hpluv2Lch(hpc);
        var luv = Lch2Luv(lch);
        var xyz = Luv2Xyz(luv);
        return Xyz2Rgb(xyz);
    }

    [Pure]
    public static LCH Hpluv2Lch(HPLuv hpc)
    {
        var (h, p, c) = hpc;
        double chromaForL;
        // White and black: disambiguate chroma
        if (c is >= 100 or < 0.000_000_01)
        {
            chromaForL = 0.0;
        }
        else
        {
            chromaForL = MaxSafeChromaForL(c) / 100.0 * p;
        }

        // Grays: disambiguate hue
        if (p < 0.000_000_01)
        {
            h = 0.0;
        }

        return new(c, chromaForL, h);
    }

    [Pure]
    public static HPLuv Rgb2Hpluv(RGB rgb)
    {
        var xyz = Rgb2Xyz(rgb);
        var luv = Xyz2Luv(xyz);
        var lch = Luv2Lch(luv);
        var hpluv = Lch2Hpluv(lch);
        return hpluv;
    }

    [Pure]
    public static HPLuv Lch2Hpluv(LCH lch)
    {
        var (l, c, h) = lch;
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

        return new(h, s, l);
    }

    [Pure]
    public static LCH Luv2Lch(LUV luv)
    {
        var (l, u, v) = luv;
        var c = Math.Sqrt(u * u + v * v);
        var h = c < 0.000_000_01 ? 0.0 : Math.Atan2(v, u) * (180 / Math.PI);
        if (h < 0.0)
        {
            h += 360.0;
        }

        return new(l, c, h);
    }

    [Pure]
    public static HSLuv Lch2Hsluv(LCH lch)
    {
        var (l, c, h) = lch;
        var s = l is >= 100.0 or < 0.000_000_01
            ? 0.0
            : c / MaxChromaForLh(l, h) * 100.0;

        if (c < 0.000_000_01)
        {
            h = 0.0;
        }

        return new(h, s, l);
    }

    [Pure]
    public static LUV Xyz2Luv(XYZ xyz)
    {
        var (x, y, z) = xyz;
        var varU = 4.0 * x / (x + 15.0 * y + 3.0 * z);
        var varV = 9.0 * y / (x + 15.0 * y + 3.0 * z);
        var l = Y2L(y);
        var u = 13.0 * l * (varU - RefU);
        var v = 13.0 * l * (varV - RefV);
        return new(l, u, v);
    }

    [Pure]
    public static RGB Hwb2Rgb(HWB hwb)
    {
        double h = hwb.H;
        double w = hwb.W;
        double b = hwb.B;
        double ratio = w + b;

        if (ratio > 1)
        {
            w /= ratio;
            b /= ratio;
        }

        double v = 1 - b;
        if (v == 0)
        {
            return new RGB(0, 0, 0);
        }

        double i = Math.Floor(h * 6);
        double f = h * 6 - i;
        double p = v * (1 - w);
        double q = v * (1 - f * w);
        double t = v * (1 - (1 - f) * w);

        return ((int)i % 6) switch
        {
            0 => new RGB(v, t, p),
            1 => new RGB(q, v, p),
            2 => new RGB(p, v, t),
            3 => new RGB(p, q, v),
            4 => new RGB(t, p, v),
            5 => new RGB(v, p, q),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    [Pure]
    public static HWB Rgb2Hwb(RGB rgb)
    {
        double r = rgb.R;
        double g = rgb.G;
        double b = rgb.B;
        double max = Math.Max(r, Math.Max(g, b));
        double min = Math.Min(r, Math.Min(g, b));
        double delta = max - min;
        double h = 0;

        if (delta != 0)
        {
            if (max == r)
            {
                h = (g - b) / delta + (g < b ? 6 : 0);
            }
            else if (max == g)
            {
                h = (b - r) / delta + 2;
            }
            else
            {
                h = (r - g) / delta + 4;
            }

            h /= 6;
        }

        double w = min;
        double bl = 1 - max;
        return new HWB(h, w, bl);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RGB Cmy2Rgb(CMY cmy)
        => new(1 - cmy.C, 1 - cmy.M, 1 - cmy.Y);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CMY Rgb2Cmy(RGB rgb)
        => new(1 - rgb.R, 1 - rgb.G, 1 - rgb.B);

    [Pure]
    public static RGB Cmyk2Rgb(CMYK cmyk)
    {
        double c = cmyk.C * (1 - cmyk.K) + cmyk.K;
        double m = cmyk.M * (1 - cmyk.K) + cmyk.K;
        double y = cmyk.Y * (1 - cmyk.K) + cmyk.K;
        return new RGB(1 - c, 1 - m, 1 - y);
    }

    [Pure]
    public static CMYK Rgb2Cmyk(RGB rgb)
    {
        var (r, g, b) = rgb;
        double k = 1 - Math.Max(r, Math.Max(g, b));
        double c = (1 - r - k) / (1 - k);
        double m = (1 - g - k) / (1 - k);
        double y = (1 - b - k) / (1 - k);
        return new CMYK(c, m, y, k);
    }

    [Pure]
    public static LAB Xyz2Lab(XYZ xyz)
    {
        double x = PivotXyz(xyz.X / D65[0]);
        double y = PivotXyz(xyz.Y / D65[1]);
        double z = PivotXyz(xyz.Z / D65[2]);

        return new LAB(
            116 * y - 16,
            500 * (x - y),
            200 * (y - z)
        );
    }

    [Pure]
    public static XYZ Lab2Xyz(LAB lab)
    {
        var (l, a, b) = lab;
        double y = (l + 16) / 116.0;
        double x = a / 500.0 + y;
        double z = y - b / 200.0;

        double x3 = Math.Pow(x, 3);
        double y3 = Math.Pow(y, 3);
        double z3 = Math.Pow(z, 3);

        return new XYZ(
            D65[0] * (x3 > 0.008856 ? x3 : (x - 16.0 / 116.0) / 7.787),
            D65[1] * (y3 > 0.008856 ? y3 : (y - 16.0 / 116.0) / 7.787),
            D65[2] * (z3 > 0.008856 ? z3 : (z - 16.0 / 116.0) / 7.787)
        );
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LAB Rgb2Lab(RGB rgb) => Xyz2Lab(Rgb2Xyz(rgb));

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RGB Lab2Rgb(LAB lab) => Xyz2Rgb(Lab2Xyz(lab));

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RGB Yuv2Rgb(YUV yuv)
    {
        var (y, u, v) = yuv;
        double r = y + 1.13983 * v;
        double g = y - 0.39465 * u - 0.58060 * v;
        double b = y + 2.03211 * u;
        return new RGB(r, g, b);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static YUV Rgb2Yuv(RGB rgb)
    {
        var (r, g, b) = rgb;
        double y = 0.299 * r + 0.587 * g + 0.114 * b;
        double u = -0.14713 * r - 0.28886 * g + 0.436 * b;
        double v = 0.615 * r - 0.51499 * g - 0.10001 * b;
        return new YUV(y, u, v);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RGB YCbCr2Rgb(YCbCr yCbCr)
    {
        var (y, cb, cr) = yCbCr;
        double r = y + 1.402 * (cr - 0.5);
        double g = y - 0.344136 * (cb - 0.5) - 0.714136 * (cr - 0.5);
        double b = y + 1.772 * (cb - 0.5);
        return new RGB(r, g, b);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static YCbCr Rgb2YCbCr(RGB rgb)
    {
        var (r, g, b) = rgb;
        double y = 0.299 * r + 0.587 * g + 0.114 * b;
        double cb = -0.168736 * r - 0.331264 * g + 0.5 * b + 0.5;
        double cr = 0.5 * r - 0.418688 * g - 0.081312 * b + 0.5;
        return new YCbCr(y, cb, cr);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RGB Hsv2Rgb(HSV hsv)
    {
        var (h, s, v) = hsv;
        double c = v * s;
        double x = c * (1 - Math.Abs(h * 6 % 2 - 1));
        double m = v - c;

        double r, g, b;

        if (h < 1.0 / 6.0) { r = c; g = x; b = 0; }
        else if (h < 2.0 / 6.0) { r = x; g = c; b = 0; }
        else if (h < 3.0 / 6.0) { r = 0; g = c; b = x; }
        else if (h < 4.0 / 6.0) { r = 0; g = x; b = c; }
        else if (h < 5.0 / 6.0) { r = x; g = 0; b = c; }
        else { r = c; g = 0; b = x; }

        return new RGB(r + m, g + m, b + m);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HSV Rgb2Hsv(RGB rgb)
    {
        var (r, g, b) = rgb;
        double max = Math.Max(r, Math.Max(g, b));
        double min = Math.Min(r, Math.Min(g, b));
        double delta = max - min;
        double h = 0;

        if (delta != 0)
        {
            if (max == r) { h = (g - b) / delta + (g < b ? 6 : 0); }
            else if (max == g) { h = (b - r) / delta + 2; }
            else { h = (r - g) / delta + 4; }
            h /= 6;
        }

        double s = max == 0 ? 0 : delta / max;
        double v = max;

        return new HSV(h, s, v);
    }
}
