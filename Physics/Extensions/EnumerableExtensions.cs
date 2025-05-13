namespace Physics;

internal static class EnumerableExtensions
{
    [Pure]
    public static IEnumerable<TOut> Merge<TIn1, TIn2, TOut>(
        this IEnumerable<TIn1> collection1,
        IEnumerable<TIn2> collection2,
        Func<TIn1?, TIn2?, TOut> aggregation,
        bool matchShortest = false)
    {
        if (matchShortest)
        {
            return collection1.Merge(collection2, aggregation, (more1, more2) => more1 && more2);
        }

        return collection1.Merge(collection2, aggregation, (more1, more2) => more1 || more2);
    }

    [Pure]
    private static IEnumerable<TOut> Merge<TIn1, TIn2, TOut>(
        this IEnumerable<TIn1> collection1,
        IEnumerable<TIn2> collection2,
        Func<TIn1?, TIn2?, TOut> aggregation,
        Func<bool, bool, bool> check)
    {
        using var enumerator1 = collection1.GetEnumerator();
        using var enumerator2 = collection2.GetEnumerator();

        var more1 = enumerator1.MoveNext();
        var more2 = enumerator2.MoveNext();

        while (check(more1, more2))
        {
            yield return aggregation(
                more1 ? enumerator1.Current : default,
                more2 ? enumerator2.Current : default);

            more1 = enumerator1.MoveNext();
            more2 = enumerator2.MoveNext();
        }
    }

    [Pure]
    public static int Hash<T>(this IEnumerable<T> collection)
    {
        var hash = new HashCode();

        foreach (var item in collection)
        {
            hash.Add(item);
        }

        return hash.ToHashCode();
    }
}