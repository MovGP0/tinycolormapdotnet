// ReSharper disable InconsistentNaming
namespace Physics;

public sealed class UnitPrefix
{
    public static readonly IDictionary<string, UnitPrefix> Prefixes;
    public static readonly UnitPrefix Z = new("Z", "zetta", 21);
    public static readonly UnitPrefix E = new("E", "exa", 18);
    public static readonly UnitPrefix P = new("P", "peta", 15);
    public static readonly UnitPrefix T = new("T", "tera", 12);
    public static readonly UnitPrefix G = new("G", "giga", 9);
    public static readonly UnitPrefix M = new("M", "mega", 6);
    public static readonly UnitPrefix k = new("k", "kilo", 3);
    public static readonly UnitPrefix h = new("h", "hecto", 2);
    public static readonly UnitPrefix d = new("d", "deca", 1);
    public static readonly UnitPrefix c = new("c", "centi", -2);
    public static readonly UnitPrefix m = new("m", "milli", -3);
    public static readonly UnitPrefix µ = new("µ", "micro", -6);
    public static readonly UnitPrefix n = new("n", "nano", -9);
    public static readonly UnitPrefix p = new("p", "pico", -12);
    public static readonly UnitPrefix f = new("f", "femto", -15);
    public static readonly UnitPrefix a = new("a", "atto", -18);
    public static readonly UnitPrefix z = new("z", "zepto", -21);
    public static readonly UnitPrefix y = new("y", "yocto", -24);

    static UnitPrefix()
    {
        Prefixes = new Dictionary<string, UnitPrefix>
        {
            {Z.Symbol, Z},
            {E.Symbol, E},
            {P.Symbol, P},
            {T.Symbol, T},
            {G.Symbol, G},
            {M.Symbol, M},
            {k.Symbol, k},
            {d.Symbol, d},
            {c.Symbol, c},
            {m.Symbol, m},
            {µ.Symbol, µ},
            {n.Symbol, n},
            {p.Symbol, p},
            {f.Symbol, f},
            {a.Symbol, a},
            {z.Symbol, z},
            {y.Symbol, y}
        };
    }

    public UnitPrefix(string symbol, string name, int exponent)
    {
        Symbol = symbol;
        Name = name;
        Factor = Math.Pow(10, exponent);
    }

    public string Symbol { get; }
    public string Name { get; }
    public double Factor { get; }

    public override string ToString()
    {
        return Symbol;
    }

    public static implicit operator double(UnitPrefix prefix) => prefix.Factor;
}