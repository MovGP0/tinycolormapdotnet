using System.Diagnostics.CodeAnalysis;

namespace NavierStokes;

/// <summary>
/// Represents the International System of Units (SI) and defines base and Units.
/// </summary>
/// <remarks>
/// Units are prefixed with '_' to avoid conflicts with variables.
/// </remarks>
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class SIUnits
{
    /// <summary>
    /// The SI unit system.
    /// </summary>
    private static readonly IUnitSystem System = UnitSystemFactory.CreateSystem("SI");

    /// <summary>
    /// Unit of length: metre (m).
    /// </summary>
    public static readonly Unit _m;

    /// <summary>
    /// Unit of mass: kilogram (kg).
    /// </summary>
    public static readonly Unit _kg;

    /// <summary>
    /// Unit of time: second (s).
    /// </summary>
    public static readonly Unit _s;

    /// <summary>
    /// Unit of electric current: ampere (A).
    /// </summary>
    public static readonly Unit _A;

    /// <summary>
    /// Unit of thermodynamic temperature: kelvin (K).
    /// </summary>
    public static readonly Unit _K;

    /// <summary>
    /// Unit of amount of substance: mole (mol).
    /// </summary>
    public static readonly Unit _mol;

    /// <summary>
    /// Unit of luminous intensity: candela (cd).
    /// </summary>
    public static readonly Unit _cd;

    /// <summary>
    /// Unit of frequency: hertz (Hz).
    /// </summary>
    public static readonly Unit _Hz;

    /// <summary>
    /// Unit of force: newton (N).
    /// </summary>
    public static readonly Unit _N;

    /// <summary>
    /// Unit of pressure: pascal (Pa).
    /// </summary>
    public static readonly Unit _Pa;

    /// <summary>
    /// Unit of energy: joule (J).
    /// </summary>
    public static readonly Unit _J;

    /// <summary>
    /// Unit of power: watt (W).
    /// </summary>
    public static readonly Unit _W;

    /// <summary>
    /// Unit of electric charge: coulomb (C).
    /// </summary>
    public static readonly Unit _C;

    /// <summary>
    /// Unit of electric potential: volt (V).
    /// </summary>
    public static readonly Unit _V;

    /// <summary>
    /// Unit of capacitance: farad (F).
    /// </summary>
    public static readonly Unit _F;

    /// <summary>
    /// Unit of electrical resistance: ohm (Ω).
    /// </summary>
    public static readonly Unit _Ω;

    /// <summary>
    /// Unit of electrical conductance: siemens (S).
    /// </summary>
    public static readonly Unit _S;

    /// <summary>
    /// Unit of magnetic flux: weber (Wb).
    /// </summary>
    public static readonly Unit _Wb;

    /// <summary>
    /// Unit of magnetic flux density: tesla (T).
    /// </summary>
    public static readonly Unit _T;

    /// <summary>
    /// Unit of inductance: henry (H).
    /// </summary>
    public static readonly Unit _H;

    /// <summary>
    /// Unit of illuminance: lux (lx).
    /// </summary>
    public static readonly Unit _lx;

    /// <summary>
    /// Unit of ionizing radiation dose: sievert (Sv).
    /// </summary>
    public static readonly Unit _Sv;

    /// <summary>
    /// Unit of catalytic activity: katal (kat).
    /// </summary>
    public static readonly Unit _kat;

    /// <summary>
    /// Initializes static members of the <see cref="SIUnits"/> class.
    /// </summary>
    static SIUnits()
    {
        //Base Units
        _m = System.AddBaseUnit("m", "metre");
        _kg = System.AddBaseUnit("kg", "kilogram", true); // flag indicates that kilogram has an inherent prefix
        _s = System.AddBaseUnit("s", "second");
        _A = System.AddBaseUnit("A", "ampere");
        _K = System.AddBaseUnit("K", "kelvin");
        _mol = System.AddBaseUnit("mol", "mole");
        _cd = System.AddBaseUnit("cd", "candela");

        //Derived Units
        _Hz = System.AddDerivedUnit("Hz", "hertz", _s ^ -1);
        _N = System.AddDerivedUnit("N", "newton", _kg * _m * (_s ^ -2));
        _Pa = System.AddDerivedUnit("Pa", "pascal", _N * (_m ^ -2));
        _J = System.AddDerivedUnit("J", "joule", _N * _m);
        _W = System.AddDerivedUnit("W", "watt", _J / _s);
        _C = System.AddDerivedUnit("C", "coulomb", _s * _A);
        _V = System.AddDerivedUnit("V", "volt", _W / _A);
        _F = System.AddDerivedUnit("F", "farad", _C / _V);
        _Ω = System.AddDerivedUnit("Ω", "joule", _V / _A);
        _S = System.AddDerivedUnit("S", "siemens", _A / _V);
        _Wb = System.AddDerivedUnit("Wb", "weber", _V * _s);
        _T = System.AddDerivedUnit("T", "tesla", _Wb * (_s ^ -2));
        _H = System.AddDerivedUnit("H", "inductance", _Wb / _A);
        _lx = System.AddDerivedUnit("lx", "immulinance", (_m ^ -2) * _cd);
        _Sv = System.AddDerivedUnit("Sv", "sievert", _J / _kg);
        _kat = System.AddDerivedUnit("kat", "katal", (_s ^ -1) * _mol);
    }
}