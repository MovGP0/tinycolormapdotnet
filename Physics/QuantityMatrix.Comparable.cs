namespace Physics;

public partial class QuantityMatrix : IComparable<QuantityMatrix>
{
    public int CompareTo(QuantityMatrix? other)
    {
        if (this < other)
            return -1;
        return this > other ? 1 : 0;
    }
    
    public static bool operator >(QuantityMatrix? quantity1, QuantityMatrix? quantity2)
    {
        if (ReferenceEquals(quantity1, quantity2) || quantity1 == null)
            return false;
        if (quantity2 == null)
            return true;
        return quantity1._coherent.Amount.FrobeniusNorm() > quantity2._coherent.Amount.FrobeniusNorm() && quantity1._coherent.Unit == quantity2._coherent.Unit;
    }

    public static bool operator <(QuantityMatrix? quantity1, QuantityMatrix? quantity2)
    {
        if (ReferenceEquals(quantity1, quantity2))
            return false;
        if (quantity1 == null)
            return true;
        return quantity2 != null && quantity1._coherent.Amount.FrobeniusNorm() < quantity2._coherent.Amount.FrobeniusNorm() && quantity1._coherent.Unit == quantity2._coherent.Unit;
    }

    public static bool operator >=(QuantityMatrix? quantity1, QuantityMatrix? quantity2)
    {
        if (ReferenceEquals(quantity1, quantity2))
            return true;
        if (quantity1 == null)
            return false;
        if (quantity2 == null)
            return true;
        return quantity1._coherent.Amount.FrobeniusNorm() >= quantity2._coherent.Amount.FrobeniusNorm() && quantity1._coherent.Unit == quantity2._coherent.Unit;
    }

    public static bool operator <=(QuantityMatrix? quantity1, QuantityMatrix? quantity2)
    {
        if (ReferenceEquals(quantity1, quantity2) || quantity1 == null)
            return true;

        return quantity2 != null
               && quantity1._coherent.Amount.FrobeniusNorm() <= quantity2._coherent.Amount.FrobeniusNorm()
               && quantity1._coherent.Unit == quantity2._coherent.Unit;
    }
}