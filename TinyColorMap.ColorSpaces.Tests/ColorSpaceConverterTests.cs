using Shouldly;

namespace TinyColorMap.ColorSpaces.Tests;

public static class ColorSpaceConverterTests
{
    public sealed class Rgb2CmyTests
    {
        [Theory]
        [MemberData(nameof(ColorSamples.RgbToCmyPairs), MemberType = typeof(ColorSamples))]
        public void ShouldConvertFromRgbToCmy(RGB rgb, CMY expected)
        {
            var actual = ColorSpaceConverter.Rgb2Cmy(rgb);
            actual.ShouldSatisfyAllConditions(
                () => actual.C.ShouldBe(expected.C, ColorSamples.Tolerance),
                () => actual.M.ShouldBe(expected.M, ColorSamples.Tolerance),
                () => actual.Y.ShouldBe(expected.Y, ColorSamples.Tolerance)
            );
        }
    }

    public sealed class Cmy2RgbTests
    {
        [Theory]
        [MemberData(nameof(ColorSamples.RgbToCmyPairs), MemberType = typeof(ColorSamples))]
        public void ShouldConvertFromCmyToRgb(RGB expected, CMY cmy)
        {
            var actual = ColorSpaceConverter.Cmy2Rgb(cmy);
            actual.ShouldSatisfyAllConditions(
                () => actual.R.ShouldBe(expected.R, ColorSamples.Tolerance),
                () => actual.G.ShouldBe(expected.G, ColorSamples.Tolerance),
                () => actual.B.ShouldBe(expected.B, ColorSamples.Tolerance)
            );
        }
    }

    public sealed class Rgb2CmykTests
    {
        [Theory]
        [MemberData(nameof(ColorSamples.RgbToCmykPairs), MemberType = typeof(ColorSamples))]
        public void ShouldConvertFromRgbToCmyk(RGB rgb, CMYK expected)
        {
            var actual = ColorSpaceConverter.Rgb2Cmyk(rgb);
            actual.ShouldSatisfyAllConditions(
                () => actual.C.ShouldBe(expected.C, ColorSamples.Tolerance),
                () => actual.M.ShouldBe(expected.M, ColorSamples.Tolerance),
                () => actual.Y.ShouldBe(expected.Y, ColorSamples.Tolerance),
                () => actual.K.ShouldBe(expected.K, ColorSamples.Tolerance)
            );
        }
    }

    public sealed class Cmyk2RgbTests
    {
        [Theory]
        [MemberData(nameof(ColorSamples.RgbToCmykPairs), MemberType = typeof(ColorSamples))]
        public void ShouldConvertFromCmykToRgb(RGB expected, CMYK cmyk)
        {
            var actual = ColorSpaceConverter.Cmyk2Rgb(cmyk);
            actual.ShouldSatisfyAllConditions(
                () => actual.R.ShouldBe(expected.R, ColorSamples.Tolerance),
                () => actual.G.ShouldBe(expected.G, ColorSamples.Tolerance),
                () => actual.B.ShouldBe(expected.B, ColorSamples.Tolerance)
            );
        }
    }

    public sealed class Rgb2HsvTests
    {
        [Theory]
        [MemberData(nameof(ColorSamples.RgbToHsvPairs), MemberType = typeof(ColorSamples))]
        public void ShouldConvertFromRgbToHsv(RGB rgb, HSV expected)
        {
            var actual = ColorSpaceConverter.Rgb2Hsv(rgb);
            actual.ShouldSatisfyAllConditions(
                () => actual.H.ShouldBe(expected.H, ColorSamples.Tolerance),
                () => actual.S.ShouldBe(expected.S, ColorSamples.Tolerance),
                () => actual.V.ShouldBe(expected.V, ColorSamples.Tolerance)
            );
        }
    }

    public sealed class Hsv2RgbTests
    {
        [Theory]
        [MemberData(nameof(ColorSamples.HsvToRgbPairs), MemberType = typeof(ColorSamples))]
        public void ShouldConvertFromHsvToRgb(HSV hsv, RGB expected)
        {
            var actual = ColorSpaceConverter.Hsv2Rgb(hsv);
            actual.ShouldSatisfyAllConditions(
                () => actual.R.ShouldBe(expected.R, ColorSamples.Tolerance),
                () => actual.G.ShouldBe(expected.G, ColorSamples.Tolerance),
                () => actual.B.ShouldBe(expected.B, ColorSamples.Tolerance)
            );
        }
    }

    public sealed class Hsv2HwbTests
    {
        [Theory]
        [MemberData(nameof(ColorSamples.HsvToHwbPairs), MemberType = typeof(ColorSamples))]
        public void ShouldConvertFromHsvToHwb(HSV hsv, HWB expected)
        {
            var actual = ColorSpaceConverter.Hsv2Hwb(hsv);
            actual.ShouldSatisfyAllConditions(
                () => actual.H.ShouldBe(expected.H, ColorSamples.Tolerance),
                () => actual.W.ShouldBe(expected.W, ColorSamples.Tolerance),
                () => actual.B.ShouldBe(expected.B, ColorSamples.Tolerance)
            );
        }
    }

    public sealed class Hwb2HsvTests
    {
        [Theory]
        [MemberData(nameof(ColorSamples.HsvToHwbPairs), MemberType = typeof(ColorSamples))]
        public void ShouldConvertFromHwbToHsv(HSV expected, HWB hwb)
        {
            var actual = ColorSpaceConverter.Hwb2Hsv(hwb);
            actual.ShouldSatisfyAllConditions(
                () => actual.H.ShouldBe(expected.H, ColorSamples.Tolerance),
                () => actual.S.ShouldBe(expected.S, ColorSamples.Tolerance),
                () => actual.V.ShouldBe(expected.V, ColorSamples.Tolerance)
            );
        }
    }

    public sealed class Rgb2YuvTests
    {
        [Theory]
        [MemberData(nameof(ColorSamples.RgbToYuvPairs), MemberType = typeof(ColorSamples))]
        public void ShouldConvertFromRgbToYuv(RGB rgb, YUV expected)
        {
            var actual = ColorSpaceConverter.Rgb2Yuv(rgb);
            actual.ShouldSatisfyAllConditions(
                () => actual.Y.ShouldBe(expected.Y, 1e-6),
                () => actual.U.ShouldBe(expected.U, 1e-4),
                () => actual.V.ShouldBe(expected.V, 1e-6)
            );
        }
    }

    public sealed class Yuv2RgbTests
    {
        [Theory]
        [MemberData(nameof(ColorSamples.RgbToYuvPairs), MemberType = typeof(ColorSamples))]
        public void ShouldConvertFromYuvToRgb(RGB expected, YUV yuv)
        {
            var actual = ColorSpaceConverter.Yuv2Rgb(yuv);
            actual.ShouldSatisfyAllConditions(
                () => actual.R.ShouldBe(expected.R, 1e-3),
                () => actual.G.ShouldBe(expected.G, 1e-3),
                () => actual.B.ShouldBe(expected.B, 1e-3)
            );
        }
    }

    public sealed class Rgb2YCbCrTests
    {
        [Theory]
        [MemberData(nameof(ColorSamples.RgbToYCbCrPairs), MemberType = typeof(ColorSamples))]
        public void ShouldConvertFromRgbToYCbCr(RGB rgb, YCbCr expected)
        {
            var actual = ColorSpaceConverter.Rgb2YCbCr(rgb);
            actual.ShouldSatisfyAllConditions(
                () => actual.Y.ShouldBe(expected.Y, ColorSamples.Tolerance),
                () => actual.Cb.ShouldBe(expected.Cb, ColorSamples.Tolerance),
                () => actual.Cr.ShouldBe(expected.Cr, ColorSamples.Tolerance)
            );
        }
    }

    public sealed class YCbCr2RgbTests
    {
        [Theory]
        [MemberData(nameof(ColorSamples.RgbToYCbCrPairs), MemberType = typeof(ColorSamples))]
        public void ShouldConvertFromYCbCrToRgb(RGB expected, YCbCr ycbcr)
        {
            var actual = ColorSpaceConverter.YCbCr2Rgb(ycbcr);
            actual.ShouldSatisfyAllConditions(
                () => actual.R.ShouldBe(expected.R, ColorSamples.Tolerance),
                () => actual.G.ShouldBe(expected.G, ColorSamples.Tolerance),
                () => actual.B.ShouldBe(expected.B, ColorSamples.Tolerance)
            );
        }
    }

    public sealed class Lch2LuvTests
    {
        [Theory]
        [MemberData(nameof(ColorSamples.LchToLuvPairs), MemberType = typeof(ColorSamples))]
        public void ShouldConvertFromLchToLuv(LCH lch, LUV expected)
        {
            var actual = ColorSpaceConverter.Lch2Luv(lch);
            actual.ShouldSatisfyAllConditions(
                () => actual.L.ShouldBe(expected.L, ColorSamples.Tolerance),
                () => actual.U.ShouldBe(expected.U, ColorSamples.Tolerance),
                () => actual.V.ShouldBe(expected.V, ColorSamples.Tolerance)
            );
        }
    }

    public sealed class Luv2LchTests
    {
        [Theory]
        [MemberData(nameof(ColorSamples.LuvToLchPairs), MemberType = typeof(ColorSamples))]
        public void ShouldConvertFromLuvToLch(LUV luv, LCH expected)
        {
            var actual = ColorSpaceConverter.Luv2Lch(luv);
            actual.ShouldSatisfyAllConditions(
                () => actual.L.ShouldBe(expected.L, ColorSamples.Tolerance),
                () => actual.C.ShouldBe(expected.C, ColorSamples.Tolerance),
                () => actual.H.ShouldBe(expected.H, ColorSamples.Tolerance)
            );
        }
    }

    public sealed class Hsluv2LchTests
    {
        [Theory]
        [MemberData(nameof(ColorSamples.HsluvBoundary), MemberType = typeof(ColorSamples))]
        public void ShouldHandleHsluvBoundary(HSLuv hsluv, LCH expected)
        {
            var actual = ColorSpaceConverter.Hsluv2Lch(hsluv);
            actual.ShouldSatisfyAllConditions(
                () => actual.L.ShouldBe(expected.L, ColorSamples.Tolerance),
                () => actual.C.ShouldBe(expected.C, ColorSamples.Tolerance),
                () => actual.H.ShouldBe(expected.H, ColorSamples.Tolerance)
            );
        }
    }

    public sealed class Hpluv2LchTests
    {
        [Theory]
        [MemberData(nameof(ColorSamples.HpluvBoundary), MemberType = typeof(ColorSamples))]
        public void ShouldHandleHpluvBoundary(HPLuv hpluv, LCH expected)
        {
            var actual = ColorSpaceConverter.Hpluv2Lch(hpluv);
            actual.ShouldSatisfyAllConditions(
                () => actual.L.ShouldBe(expected.L, ColorSamples.Tolerance),
                () => actual.C.ShouldBe(expected.C, ColorSamples.Tolerance),
                () => actual.H.ShouldBe(expected.H, ColorSamples.Tolerance)
            );
        }
    }
}