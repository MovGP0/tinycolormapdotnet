namespace TinyColorMap.ColorSpaces.Tests;

public static class ColorSamples
{
    public static readonly RGB Black = new(0, 0, 0);
    public static readonly RGB White = new(1, 1, 1);
    public static readonly RGB Red = new(1, 0, 0);
    public static readonly RGB Green = new(0, 1, 0);
    public static readonly RGB Blue = new(0, 0, 1);
    public static readonly RGB Magenta = new(1, 0, 1);
    public static readonly RGB Cyan = new(0, 1, 1);
    public static readonly RGB Yellow = new(1, 1, 0);

    public static readonly RGB[] All = [ Black, White, Red, Green, Blue, Magenta, Cyan, Yellow ];

    public const double Tolerance = 1e-6;

    // CMY: expected = (1-R,1-G,1-B)
    public static IEnumerable<object[]> RgbToCmyPairs => new[]
    {
        new object[]{ Black, new CMY(1,1,1) },
        new object[]{ White, new CMY(0,0,0) },
        new object[]{ Red,   new CMY(0,1,1) },
        new object[]{ Green, new CMY(1,0,1) },
        new object[]{ Blue,  new CMY(1,1,0) },
    };

    // CMYK: skip Black (division by zero), include White & primaries
    public static IEnumerable<object[]> RgbToCmykPairs => new[]
    {
        new object[]{ White, new CMYK(0,0,0,0) },
        new object[]{ Red,   new CMYK(0,1,1,0) },
        new object[]{ Green, new CMYK(1,0,1,0) },
        new object[]{ Blue,  new CMYK(1,1,0,0) },
    };

    // HSV: h normalized [0,1), s & v as usual
    public static IEnumerable<object[]> RgbToHsvPairs => new[]
    {
        new object[]{ Black,   new HSV(0,   0, 0) },
        new object[]{ White,   new HSV(0,   0, 1) },
        new object[]{ Red,     new HSV(0,   1, 1) },
        new object[]{ Green,   new HSV(1.0/3,1, 1) },
        new object[]{ Blue,    new HSV(2.0/3,1, 1) },
        new object[]{ Yellow,  new HSV(1.0/6,1, 1) },
        new object[]{ Cyan,    new HSV(0.5,  1, 1) },
        new object[]{ Magenta, new HSV(5.0/6,1, 1) },
    };

    public static IEnumerable<object[]> HsvToRgbPairs => new[]
    {
        new object[]{ new HSV(0,   0, 0), Black },
        new object[]{ new HSV(0,   0, 1), White },
        new object[]{ new HSV(0,   1, 1), Red   },
        new object[]{ new HSV(1.0/3,1, 1), Green },
        new object[]{ new HSV(2.0/3,1, 1), Blue  },
        new object[]{ new HSV(1.0/6,1, 1), Yellow},
        new object[]{ new HSV(0.5,  1, 1), Cyan  },
        new object[]{ new HSV(5.0/6,1, 1), Magenta},
    };

    // Hsv→Hwb: w = 1 - s*v, b = 1 - v
    public static IEnumerable<object[]> HsvToHwbPairs => new[]
    {
        new object[]{ new HSV(0.25, 0.4, 0.8), new HWB(0.25, 1-0.4*0.8, 1-0.8) },
        new object[]{ new HSV(0,    1,   1),   new HWB(0,    0,         0)     },
        new object[]{ new HSV(0,    0,   0),   new HWB(0,    1,         1)     },
    };

    // YUV: y=0.299r+0.587g+0.114b; u=...; v=...
    public static IEnumerable<object[]> RgbToYuvPairs => new[]
    {
        new object[]{ Black, new YUV(0,        0,         0)       },
        new object[]{ White, new YUV(1,        0,         0)       },
        new object[]{ Red,   new YUV(0.299,   -0.14713,  0.615)    },
    };

    // YCbCr: y=0.299r+0.587g+0.114b; cb=...; cr=...
    public static IEnumerable<object[]> RgbToYCbCrPairs => new[]
    {
        new object[]{ Black, new YCbCr(0,     0.5, 0.5) },
        new object[]{ White, new YCbCr(1,     0.5, 0.5) },
        new object[]{ Red,   new YCbCr(0.299, 0.331264, 1.0) },
    };

    // LCH→LUV few sample points
    public static IEnumerable<object[]> LchToLuvPairs => new[]
    {
        new object[]{ new LCH(50, 10, 30),
            new LUV(50,
                    10 * Math.Cos(30*Math.PI/180),
                    10 * Math.Sin(30*Math.PI/180))
        },
        new object[]{ new LCH(75, 20, 180),
            new LUV(75,
                    20 * Math.Cos(Math.PI),
                    20 * Math.Sin(Math.PI))
        },
    };

    // LUV→LCH couple
    public static IEnumerable<object[]> LuvToLchPairs => new[]
    {
        new object[]{ new LUV(0,0,0),   new LCH(0,0,0) },
        new object[]{ new LUV(60,3,4),
            new LCH(60, Math.Sqrt(9+16), Math.Atan2(4,3)*180/Math.PI)
        },
    };

    // HSLuv/LCH boundary
    public static IEnumerable<object[]> HsluvBoundary => new[]
    {
        new object[]{ new HSLuv(10,50,0),   new LCH(0,0,10) },
        new object[]{ new HSLuv(20,75,100), new LCH(100,0,20) },
    };

    // HPLuv/LCH boundary
    public static IEnumerable<object[]> HpluvBoundary => new[]
    {
        new object[]{ new HPLuv(30,20,0),   new LCH(0,0,30) },
        new object[]{ new HPLuv(40,60,100), new LCH(100,0,40) },
    };
}