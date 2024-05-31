namespace Physics;

public abstract class Unit : IEquatable<Unit>
{
    private readonly int _hashCode;

    internal Unit(IUnitSystem system, double factor, Dimension dimension)
    {
        Check.Argument(dimension, nameof(dimension)).IsNotNull();

        System = system;
        Dimension = dimension;
        Factor = factor;

        _hashCode = GenerateHashCode();
    }

    [Pure]
    internal IUnitSystem System { get; }

    [Pure]
    internal double Factor { get; }

    [Pure]
    internal Dimension Dimension { get; }

    [Pure]
    internal bool IsCoherent => Factor.Equals(1);

    [Pure]
    public bool Equals(Unit? other)
    {
        if (ReferenceEquals(this, other)) return true;
        if (other is null) return false;

        return HasSameDimension(other)
               && Factor.Equals(other.Factor);
    }

    [Pure]
    public bool HasSameSystem(Unit other) => System == other.System;

    [Pure]
    public bool HasSameDimension(Unit other)
    {
        return HasSameSystem(other)
               && Dimension.Equals(other.Dimension);
    }

    [Pure]
    public override int GetHashCode() => _hashCode;

    [Pure]
    public override bool Equals(object? obj) => obj is Unit unit && Equals(unit);

    [Pure]
    public static bool operator ==(Unit? unit1, Unit? unit2)
    {
        if (ReferenceEquals(unit1, unit2)) return true;
        if (unit1 is null || unit2 is null) return false;

        return unit1.HasSameDimension(unit2)
               && unit1.Factor.Equals(unit2.Factor);
    }

    [Pure]
    public static bool operator !=(Unit unit1, Unit unit2) => !(unit1 == unit2);

    [Pure]
    public static Unit operator *(Unit unit1, Unit unit2)
    {
        Check.Argument(unit1, nameof(unit1)).IsNotNull();
        Check.Argument(unit2, nameof(unit2)).IsNotNull();
        Check.UnitsAreFromSameSystem(unit1, unit2);

        return unit1.System.CreateUnit(unit1.Factor*unit2.Factor, unit1.Dimension*unit2.Dimension);
    }

    [Pure]
    public static Unit operator /(Unit unit1, Unit unit2)
    {
        Check.Argument(unit1, nameof(unit1)).IsNotNull();
        Check.Argument(unit2, nameof(unit2)).IsNotNull();
        Check.UnitsAreFromSameSystem(unit1, unit2);

        return unit1.System.CreateUnit(unit1.Factor/unit2.Factor, unit1.Dimension/unit2.Dimension);
    }

    [Pure]
    public static Unit operator ^(Unit unit, int exponent)
    {
        Check.Argument(unit, nameof(unit)).IsNotNull();

        return unit.System.CreateUnit(Math.Pow(unit.Factor, exponent), unit.Dimension ^ exponent);
    }

    [Pure]
    public static Unit operator *(Unit unit, double factor)
        => unit.System.CreateUnit(factor*unit.Factor, unit.Dimension);

    [Pure]
    public static Unit operator *(double factor, Unit unit)
        => unit.System.CreateUnit(factor*unit.Factor, unit.Dimension);

    [Pure]
    public static Unit operator /(Unit unit, double factor)
        => unit.System.CreateUnit(unit.Factor/factor, unit.Dimension);

    [Pure]
    public override string ToString() => System.Display(this);

    [Pure]
    private int GenerateHashCode() => HashCode.Combine(Dimension, Factor);
}