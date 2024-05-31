namespace Physics;

public static class UnitSystemFactory
{
    [Pure]
    public static IUnitSystem CreateSystem(string name)
        => new UnitSystem(name);
}