namespace Physics;

public sealed class Dimension : ImmutableCollection<float>
{
    internal Dimension(params float[] exponents)
        : base(Trim(exponents))
    {
        Check.Argument(exponents, nameof(exponents)).IsNotNull();
    }

    public static Dimension DimensionLess { get; } = new();

    public static Dimension operator *(Dimension dimension1, Dimension dimension2)
    {
        Check.Argument(dimension1, nameof(dimension1)).IsNotNull();
        Check.Argument(dimension2, nameof(dimension2)).IsNotNull();

        return new Dimension(dimension1.Merge(dimension2, (x, y) => x + y).ToArray());
    }

    public static Dimension operator /(Dimension dimension1, Dimension dimension2)
    {
        Check.Argument(dimension1, nameof(dimension1)).IsNotNull();
        Check.Argument(dimension2, nameof(dimension2)).IsNotNull();

        return new Dimension(dimension1.Merge(dimension2, (x, y) => x - y).ToArray());
    }

    public static Dimension operator ^(Dimension dimension, float exponent)
    {
        Check.Argument(dimension, nameof(dimension)).IsNotNull();

        return new Dimension(dimension.Select(e => e*exponent).ToArray());
    }

    private static float[] Trim(float[] exponents)
    {
        if (exponents.Length == 0) return exponents;

        var index = exponents.Length - 1;

        while (index >= 0 && exponents[index] == 0)
        {
            index--;
        }

        Array.Resize(ref exponents, index + 1);

        return exponents;
    }

    public override string ToString() => this.ToArray().ToString();
}