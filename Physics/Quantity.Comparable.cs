namespace Physics;

public partial class Quantity : IComparable<Quantity>
{
    public int CompareTo(Quantity? other)
    {
        if (this < other) return -1;
        if (this > other) return 1;

        return 0;
    }

    public static bool operator >(Quantity? quantity1, Quantity? quantity2)
    {
        if (ReferenceEquals(quantity1, quantity2)) return false;
        if (quantity1 is null) return false;
        if (quantity2 is null) return true;

        return quantity1._coherent.Amount > quantity2._coherent.Amount
               && quantity1._coherent.Unit == quantity2._coherent.Unit;
    }

    public static bool operator <(Quantity? quantity1, Quantity? quantity2)
    {
        if (ReferenceEquals(quantity1, quantity2)) return false;
        if (quantity1 is null) return true;
        if (quantity2 is null) return false;

        return quantity1._coherent.Amount < quantity2._coherent.Amount
               && quantity1._coherent.Unit == quantity2._coherent.Unit;
    }

    public static bool operator >=(Quantity? quantity1, Quantity? quantity2)
    {
        if (ReferenceEquals(quantity1, quantity2)) return true;
        if (quantity1 is null) return false;
        if (quantity2 is null) return true;

        return quantity1._coherent.Amount >= quantity2._coherent.Amount
               && quantity1._coherent.Unit == quantity2._coherent.Unit;
    }

    public static bool operator <=(Quantity? quantity1, Quantity? quantity2)
    {
        if (ReferenceEquals(quantity1, quantity2)) return true;
        if (quantity1 is null) return true;
        if (quantity2 is null) return false;

        return quantity1._coherent.Amount <= quantity2._coherent.Amount
               && quantity1._coherent.Unit == quantity2._coherent.Unit;
    }
}