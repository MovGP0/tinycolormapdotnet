namespace Physics;

public partial class QuantityVector : IEquatable<QuantityVector>
{
    private readonly int _hashCode;

    public override int GetHashCode() => _hashCode;

    private int GenerateHashCode() => HashCode.Combine(Unit, Amount);

    public bool Equals(QuantityVector? other)
    {
        return other != null
               && _coherent.Unit == other._coherent.Unit
               && _coherent.Amount == other._coherent.Amount;
    }

    public override bool Equals(object? obj)
        => obj is QuantityVector quantityVector && Equals(quantityVector);

    public static bool operator ==(QuantityVector? quantity1, QuantityVector? quantity2)
    {
        if (ReferenceEquals(quantity1, quantity2))
        {
            return true;
        }

        return quantity1 != null && quantity1.Equals(quantity2);
    }

    public static bool operator !=(QuantityVector? quantity1, QuantityVector? quantity2)
        => !(quantity1 == quantity2);
}