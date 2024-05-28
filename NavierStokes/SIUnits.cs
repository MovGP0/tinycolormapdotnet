using System.Diagnostics.CodeAnalysis;
using Physics;

namespace NavierStokes;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public sealed class SI
{
    private readonly IUnitSystem System = UnitSystemFactory.CreateSystem("SI");

    public SI()
    {
        //Base units
        Unit m = System.AddBaseUnit("m", "metre");
        Unit kg = System.AddBaseUnit("kg", "kilogram", true); // flag indicates that kilogram has an inherent prefix
        Unit s = System.AddBaseUnit("s", "second");
        Unit A = System.AddBaseUnit("A", "ampere");
        Unit K = System.AddBaseUnit("K", "kelvin");
        Unit mol = System.AddBaseUnit("mol", "mole");
        Unit cd = System.AddBaseUnit("cd", "candela");

        //Derived units
        Unit Hz = System.AddDerivedUnit("Hz", "hertz", s ^ -1);
        Unit N = System.AddDerivedUnit("N", "newton", kg * m * (s ^ -2));
        Unit Pa = System.AddDerivedUnit("Pa", "pascal", N * (m ^ -2));
        Unit J = System.AddDerivedUnit("J", "joule", N * m);
        Unit W = System.AddDerivedUnit("W", "watt", J / s);
        Unit C = System.AddDerivedUnit("C", "coulomb", s * A);
        Unit V = System.AddDerivedUnit("V", "volt", W / A);
        Unit F = System.AddDerivedUnit("F", "farad", C / V);
        Unit Ω = System.AddDerivedUnit("Ω", "joule", V / A);
        Unit S = System.AddDerivedUnit("S", "siemens", A / V);
        Unit Wb = System.AddDerivedUnit("Wb", "weber", V * s);
        Unit T = System.AddDerivedUnit("T", "tesla", Wb * (s ^ -2));
        Unit H = System.AddDerivedUnit("H", "inductance", Wb / A);
        Unit lx = System.AddDerivedUnit("lx", "immulinance", (m ^ -2) * cd);
        Unit Sv = System.AddDerivedUnit("Sv", "sievert", J / kg);
        Unit kat = System.AddDerivedUnit("kat", "katal", (s ^ -1) * mol);
    }
}