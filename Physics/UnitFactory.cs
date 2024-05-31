namespace Physics;

internal class UnitFactory : IUnitFactory
{
    [Pure]
    public KnownUnit CreateUnit(
        IUnitSystem system,
        double factor,
        Dimension dimension,
        string symbol,
        string name,
        bool inherentPrefix)
    {
        return new(system, factor, dimension, symbol, name, inherentPrefix);
    }

    [Pure]
    public Unit CreateUnit(IUnitSystem system, double factor, Dimension dimension)
    {
        Check.SystemKnowsDimension(system, dimension);

        return new DerivedUnit(system, factor, dimension);
    }
}