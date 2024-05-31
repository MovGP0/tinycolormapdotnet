namespace Physics;

public partial class Quantity
{
    private readonly Quantity _coherent;

    public Quantity(double amount, Unit unit)
    {
        Amount = amount;
        Unit = unit;
        _coherent = ToCoherent();
        _hashCode = GenerateHashCode();
    }

    private Quantity(double amount, Unit unit, Quantity coherent)
    {
        Amount = amount;
        Unit = unit;
        _coherent = coherent;
        _hashCode = GenerateHashCode();
    }

    public double Amount { get; }
    public Unit Unit { get; }

    public Quantity Convert(Unit unit)
    {
        if (Unit == unit) return this;

        Check.UnitsAreSameDimension(Unit, unit);
        return new Quantity(_coherent.Amount/unit.Factor, unit, _coherent);
    }

    public static Quantity operator +(Quantity q1, Quantity q2)
    {
        Check.UnitsAreSameDimension(q1.Unit, q2.Unit);
        return new Quantity(q1._coherent.Amount + q2._coherent.Amount, q1._coherent.Unit);
    }

    public static Quantity operator -(Quantity q1, Quantity q2)
    {
        Check.UnitsAreSameDimension(q1.Unit, q2.Unit);
        return new Quantity(q1._coherent.Amount - q2._coherent.Amount, q1._coherent.Unit);
    }

    public static Quantity operator *(Quantity q1, Quantity q2)
    {
        return new Quantity(q1._coherent.Amount*q2._coherent.Amount, q1._coherent.Unit*q2._coherent.Unit);
    }

    public static Quantity operator *(Quantity q, double factor)
    {
        return new Quantity(q._coherent.Amount*factor, q._coherent.Unit);
    }

    public static Quantity operator *(double factor, Quantity q)
    {
        return new Quantity(q._coherent.Amount*factor, q._coherent.Unit);
    }

    public static Quantity operator /(Quantity q1, Quantity q2)
    {
        return new Quantity(q1._coherent.Amount/q2._coherent.Amount, q1._coherent.Unit/q2._coherent.Unit);
    }

    public static Quantity operator /(Quantity q, double factor)
    {
        return new Quantity(q._coherent.Amount/factor, q._coherent.Unit);
    }

    public static Quantity operator /(double factor, Quantity q)
    {
        return new Quantity(q._coherent.Amount/factor, q._coherent.Unit);
    }

    public static Quantity operator ^(Quantity q, int exponent)
    {
        return new Quantity(Math.Pow(q._coherent.Amount, exponent), q._coherent.Unit ^ exponent);
    }

    internal Quantity ToCoherent()
    {
        var unit = Unit;

        if (unit.IsCoherent)
        {
            return this;
        }

        var coherentUnit = unit.System.MakeCoherent(unit);

        return new Quantity(unit.Factor * Amount, coherentUnit);
    }
}