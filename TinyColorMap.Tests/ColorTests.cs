using Shouldly;

namespace TinyColorMap.Tests;

public static class ColorTests
{
    public sealed class ConstructorsTests
    {
        [Fact]
        public void ShouldInitializeRgbConstructorCorrectly()
        {
            var c = new Color(0.1, 0.2, 0.3);
            c.R.ShouldBe(0.1);
            c.G.ShouldBe(0.2);
            c.B.ShouldBe(0.3);
        }

        [Fact]
        public void ShouldInitializeGrayConstructorCorrectly()
        {
            var gray = 0.4;
            var c = new Color(gray);
            c.R.ShouldBe(gray);
            c.G.ShouldBe(gray);
            c.B.ShouldBe(gray);
        }
    }

    public sealed class ColorConstantsTests
    {
        [Fact]
        public void ShouldDefineRedConstant()
        {
            Color.Red.ShouldSatisfyAllConditions(
                () => Color.Red.R.ShouldBe(1.0),
                () => Color.Red.G.ShouldBe(0.0),
                () => Color.Red.B.ShouldBe(0.0)
            );
        }

        [Fact]
        public void ShouldDefineGreenConstant()
        {
            Color.Green.ShouldSatisfyAllConditions(
                () => Color.Green.R.ShouldBe(0.0),
                () => Color.Green.G.ShouldBe(1.0),
                () => Color.Green.B.ShouldBe(0.0)
            );
        }

        [Fact]
        public void ShouldDefineBlueConstant()
        {
            Color.Blue.ShouldSatisfyAllConditions(
                () => Color.Blue.R.ShouldBe(0.0),
                () => Color.Blue.G.ShouldBe(0.0),
                () => Color.Blue.B.ShouldBe(1.0)
            );
        }
    }

    public sealed class ChannelByteConversionTests
    {
        [Theory]
        [InlineData(0.0,    0)]
        [InlineData(1.0,  255)]
        [InlineData(0.5,  127)]    // 0.5*255 = 127.5 → byte truncates to 127
        [InlineData(1.5,  255)]    // clamps above 1.0
        [InlineData(-0.5,   0)]    // clamps below 0.0
        public void ShouldClampAndConvertToBytes(double component, byte expected)
        {
            var cR = new Color(component, 0, 0);
            var cG = new Color(0, component, 0);
            var cB = new Color(0, 0, component);

            cR.Ri.ShouldBe(expected);
            cG.Gi.ShouldBe(expected);
            cB.Bi.ShouldBe(expected);
        }
    }

    public sealed class OperatorTests
    {
        [Fact]
        public void ShouldAddTwoColorsComponentwise()
        {
            var a = new Color(0.1, 0.2, 0.3);
            var b = new Color(0.4, 0.5, 0.6);
            var sum = a + b;

            sum.ShouldSatisfyAllConditions(
                () => sum.R.ShouldBe(0.5, 1e-6),
                () => sum.G.ShouldBe(0.7, 1e-6),
                () => sum.B.ShouldBe(0.9, 1e-6)
            );
        }

        [Fact]
        public void ShouldAddScalarToColor()
        {
            var c = new Color(0.1, 0.2, 0.3);
            var result = c + 0.5;

            result.ShouldSatisfyAllConditions(
                () => result.R.ShouldBe(0.6),
                () => result.G.ShouldBe(0.7),
                () => result.B.ShouldBe(0.8)
            );
        }

        [Fact]
        public void ShouldMultiplyColorByScalar_LeftAndRight()
        {
            var c = new Color(0.2, 0.3, 0.4);
            var left  = 2.0 * c;
            var right = c * 2.0;

            left .ShouldSatisfyAllConditions(
                () => left.R .ShouldBe(0.4),
                () => left.G .ShouldBe(0.6),
                () => left.B .ShouldBe(0.8)
            );

            right.ShouldSatisfyAllConditions(
                () => right.R.ShouldBe(0.4),
                () => right.G.ShouldBe(0.6),
                () => right.B.ShouldBe(0.8)
            );
        }

        [Fact]
        public void ShouldDivideColorByScalar()
        {
            var c = new Color(0.4, 0.6, 0.8);
            var result = c / 2.0;

            result.ShouldSatisfyAllConditions(
                () => result.R.ShouldBe(0.2),
                () => result.G.ShouldBe(0.3),
                () => result.B.ShouldBe(0.4)
            );
        }
    }

    public sealed class DeconstructTests
    {
        [Fact]
        public void ShouldDeconstructToByteChannels()
        {
            var c = new Color(0.1, 0.5, 1.2);
            c.Deconstruct(out var r, out var g, out var b);

            // 0.1*255=25.5→25, 0.5*255=127.5→127, 1.2→clamped→1*255=255
            r.ShouldBe((byte)25);
            g.ShouldBe((byte)127);
            b.ShouldBe((byte)255);
        }
    }
}