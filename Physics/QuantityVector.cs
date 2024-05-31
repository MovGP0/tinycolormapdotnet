using MathNet.Numerics.LinearAlgebra;

namespace Physics;

public partial class QuantityVector
{
    private readonly QuantityVector _coherent;

    public QuantityVector(Vector<double> amount, Unit unit)
    {
        Amount = amount ?? throw new ArgumentNullException(nameof(amount));
        Unit = unit;
        _coherent = ToCoherent();
        _hashCode = GenerateHashCode();
    }

    private QuantityVector(Vector<double> amount, Unit unit, QuantityVector coherent)
    {
        Amount = amount;
        Unit = unit;
        _coherent = coherent;
        _hashCode = GenerateHashCode();
    }

    public Vector<double> Amount { get; }
    public Unit Unit { get; }

    public QuantityVector Convert(Unit unit)
    {
        if (Unit == unit)
            return this;
        Check.UnitsAreSameDimension(Unit, unit);
        return new QuantityVector(_coherent.Amount / unit.Factor, unit, _coherent);
    }

    public static QuantityVector operator +(QuantityVector q1, QuantityVector q2)
    {
        Check.UnitsAreSameDimension(q1.Unit, q2.Unit);
        return new QuantityVector(
            q1._coherent.Amount + q2._coherent.Amount,
            q1._coherent.Unit);
    }

    public static QuantityVector operator -(QuantityVector q1, QuantityVector q2)
    {
        Check.UnitsAreSameDimension(q1.Unit, q2.Unit);
        return new QuantityVector(
            q1._coherent.Amount - q2._coherent.Amount,
            q1._coherent.Unit);
    }

    public static QuantityVector operator *(QuantityVector q, double factor)
        => new(q._coherent.Amount * factor, q._coherent.Unit);

    public static QuantityVector operator *(double factor, QuantityVector q)
        => new(q._coherent.Amount * factor, q._coherent.Unit);

    public static QuantityVector operator *(Quantity factor, QuantityVector q)
    {
        var coherentFactor = factor.ToCoherent();
        return new QuantityVector(
            q._coherent.Amount * coherentFactor.Amount, 
            q._coherent.Unit * coherentFactor.Unit);
    }

    public static QuantityVector operator /(QuantityVector q, double factor)
        => new(q._coherent.Amount / factor, q._coherent.Unit);

    public static QuantityVector operator /(QuantityVector q, Quantity factor)
    {
        var coherentFactor = factor.ToCoherent();
        return new QuantityVector(
            q._coherent.Amount / coherentFactor.Amount, 
            q._coherent.Unit / coherentFactor.Unit);
    }

    public static QuantityVector operator -(QuantityVector q) => q * -1;

    internal QuantityVector ToCoherent()
    {
        var unit = Unit;

        if (unit.IsCoherent)
        {
            return this;
        }

        var coherentUnit = unit.System.MakeCoherent(unit);

        return new QuantityVector(unit.Factor * Amount, coherentUnit);
    }

    public QuantityVector Clone() => new(Amount.Clone(), Unit, _coherent);
}
