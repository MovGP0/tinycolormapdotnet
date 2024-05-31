using MathNet.Numerics.LinearAlgebra;

namespace Physics;

public partial class QuantityMatrix
{
    private readonly QuantityMatrix _coherent;

    public QuantityMatrix(Matrix<double> amount, Unit unit)
    {
        Amount = amount ?? throw new ArgumentNullException(nameof(amount));
        Unit = unit;
        _coherent = ToCoherent();
        _hashCode = GenerateHashCode();
    }

    private QuantityMatrix(Matrix<double> amount, Unit unit, QuantityMatrix coherent)
    {
        Amount = amount;
        Unit = unit;
        _coherent = coherent;
        _hashCode = GenerateHashCode();
    }

    public Matrix<double> Amount { get; }
    public Unit Unit { get; }

    public QuantityMatrix Convert(Unit unit)
    {
        if (Unit == unit)
            return this;
        Check.UnitsAreSameDimension(Unit, unit);
        return new QuantityMatrix(_coherent.Amount / unit.Factor, unit, _coherent);
    }

    public static QuantityMatrix operator +(QuantityMatrix q1, QuantityMatrix q2)
    {
        Check.UnitsAreSameDimension(q1.Unit, q2.Unit);
        return new QuantityMatrix(q1._coherent.Amount + q2._coherent.Amount, q1._coherent.Unit);
    }

    public static QuantityMatrix operator -(QuantityMatrix q1, QuantityMatrix q2)
    {
        Check.UnitsAreSameDimension(q1.Unit, q2.Unit);
        return new QuantityMatrix(q1._coherent.Amount - q2._coherent.Amount, q1._coherent.Unit);
    }

    public static QuantityMatrix operator *(QuantityMatrix q, double factor)
        => new(q._coherent.Amount * factor, q._coherent.Unit);

    public static QuantityMatrix operator *(double factor, QuantityMatrix q)
        => new(q._coherent.Amount * factor, q._coherent.Unit);

    public static QuantityMatrix operator /(QuantityMatrix q, double factor)
        => new(q._coherent.Amount / factor, q._coherent.Unit);

    internal QuantityMatrix ToCoherent()
    {
        var unit = Unit;

        if (unit.IsCoherent)
        {
            return this;
        }

        var coherentUnit = unit.System.MakeCoherent(unit);

        return new QuantityMatrix(unit.Factor * Amount, coherentUnit);
    }

    private int GenerateHashCode() => HashCode.Combine(Unit, Amount);

    public QuantityMatrix Clone() => new(Amount.Clone(), Unit, _coherent);
}
