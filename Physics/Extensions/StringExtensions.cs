namespace Physics;

internal static class StringExtensions
{
    [Pure]
    public static string FormatWith(this string format, params object[] args)
        => string.Format(format, args);
}