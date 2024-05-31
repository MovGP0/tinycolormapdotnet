namespace Physics;

public partial class Quantity : IEquatable<Quantity>
{
    private readonly int _hashCode;

    public override int GetHashCode() => _hashCode;

    private int GenerateHashCode()
        => HashCode.Combine(Unit, Amount);

    public bool Equals(Quantity? other)
    {
        if (other is null) return false;

        return _coherent.Unit == other._coherent.Unit
               && _coherent.Amount == other._coherent.Amount;
    }

    public override bool Equals(object? obj)
        => Equals(obj as Quantity);

    public static bool operator ==(Quantity? quantity1, Quantity? quantity2)
    {
        if (ReferenceEquals(quantity1, quantity2)) return true;
        if (quantity1 is null) return false;

        return quantity1.Equals(quantity2);
    }

    public static bool operator !=(Quantity? quantity1, Quantity? quantity2)
        => !(quantity1 == quantity2);
}