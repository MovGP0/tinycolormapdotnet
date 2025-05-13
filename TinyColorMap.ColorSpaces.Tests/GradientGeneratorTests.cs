using Shouldly;

namespace TinyColorMap.ColorSpaces.Tests;

public static class GradientGeneratorTests
{
    public sealed class CreateGradientTests
    {
        [Fact]
        public void ShouldYieldCorrectCountAndPositions()
        {
            int n = 5;
            var list = GradientGenerator
                .CreateGradient(
                    GradientGeneralType.Continuous,
                    GradientJoiningType.No,
                    hue: 123,
                    saturation: 0.5,
                    numberOfColors: n)
                .ToList();

            // count
            list.Count.ShouldBe(n);

            // positions = i/n
            for (int i = 0; i < n; i++)
            {
                list[i].position.ShouldBe((double)i / n, 1e-6);
            }
        }

        [Fact]
        public void ShouldSwitchToComplementaryHue_AfterMidpoint_ForDiverging()
        {
            int n = 4;
            double hue = 10, sat = 0.5;
            double comp = (10 + 180) % 360;

            var list = GradientGenerator
                .CreateGradient(
                    GradientGeneralType.Continuous,
                    GradientJoiningType.Diverging,
                    hue, sat, n)
                .ToList();

            // first two use hue=10 (positions 0 and 0.25 → mh stays hue)
            // last two use complementary (positions 0.5 and 0.75 → mh = comp)
            // we can't directly get mh, but we can assert that
            // the colors at i=0 and i=3 are different when hue != comp
            var c0 = list[0].color;
            var c3 = list[3].color;

            c0.ShouldNotBe(c3);
        }

        [Fact]
        public void ShouldProduceMonotonicLightness_WhenNoJoiningAndContinuous_SaturationZero()
        {
            int n = 6;
            var list = GradientGenerator
                .CreateGradient(
                    GradientGeneralType.Continuous,
                    GradientJoiningType.No,
                    hue: 0,
                    saturation: 0,
                    numberOfColors: n)
                .Select(t => t.color)
                .ToList();

            // saturation=0 → grayscale → R=G=B = f(position)
            var lum = list.Select(c => c.R).ToArray();

            // ensure strictly increasing
            for (int i = 1; i < n; i++)
            {
                lum[i].ShouldBeGreaterThan(lum[i-1]);
            }
        }
    }
}