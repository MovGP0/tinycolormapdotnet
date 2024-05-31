using System.Globalization;

namespace Physics;

public partial class Quantity : IFormattable
{
    public override string ToString()
    {
        return ToString(null, NumberFormatInfo.CurrentInfo);
    }

    public string ToString(string format)
    {
        return ToString(format, NumberFormatInfo.CurrentInfo);
    }

    public string ToString(IFormatProvider formatProvider)
    {
        return ToString(null, formatProvider);
    }

    public string ToString(Unit unit)
    {
        return Convert(unit).ToString(null, NumberFormatInfo.CurrentInfo);
    }

    public string ToString(string format, Unit unit)
        => Convert(unit).ToString(format, NumberFormatInfo.CurrentInfo);

    public string ToString(IFormatProvider formatProvider, Unit unit)
        => Convert(unit).ToString(null, formatProvider);

    public string ToString(string format, IFormatProvider formatProvider, Unit unit)
        => Convert(unit).ToString(format, formatProvider);

    public string ToString(string? format, IFormatProvider? formatProvider)
        => "{0} {1}".FormatWith(Amount.ToString(format, formatProvider), Unit);

}