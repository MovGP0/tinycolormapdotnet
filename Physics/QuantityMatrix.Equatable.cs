namespace Physics;

public partial class QuantityMatrix : IEquatable<QuantityMatrix>
{
    private readonly int _hashCode;

    public override int GetHashCode() => _hashCode;

    public bool Equals(QuantityMatrix? other)
    {
        return other != null
               && _coherent.Unit == other._coherent.Unit
               && _coherent.Amount == other._coherent.Amount;
    }

    public override bool Equals(object? obj)
        => obj is QuantityMatrix quantityMatrix && Equals(quantityMatrix);

    public static bool operator ==(QuantityMatrix? quantity1, QuantityMatrix? quantity2)
    {
        if (ReferenceEquals(quantity1, quantity2))
        {
            return true;
        }

        return quantity1 != null && quantity1.Equals(quantity2);
    }

    public static bool operator !=(QuantityMatrix? quantity1, QuantityMatrix? quantity2)
        => !(quantity1 == quantity2);
}