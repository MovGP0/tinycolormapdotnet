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
    /// Unit of length: metre (m).
    /// </summary>
    [Pure]
    public static Unit _m => Physics.Systems.SI.m;

    /// <summary>
    /// Unit of mass: kilogram (kg).
    /// </summary>
    [Pure]
    public static Unit _kg => Physics.Systems.SI.kg;

    /// <summary>
    /// Unit of time: second (s).
    /// </summary>
    [Pure]
    public static Unit _s => Physics.Systems.SI.s;

    /// <summary>
    /// Unit of electric current: ampere (A).
    /// </summary>
    [Pure]
    public static Unit _A => Physics.Systems.SI.A;

    /// <summary>
    /// Unit of thermodynamic temperature: kelvin (K).
    /// </summary>
    [Pure]
    public static Unit _K => Physics.Systems.SI.K;

    /// <summary>
    /// Unit of amount of substance: mole (mol).
    /// </summary>
    [Pure]
    public static Unit _mol => Physics.Systems.SI.mol;

    /// <summary>
    /// Unit of luminous intensity: candela (cd).
    /// </summary>
    [Pure]
    public static Unit _cd => Physics.Systems.SI.cd;

    /// <summary>
    /// Unit of frequency: hertz (Hz).
    /// </summary>
    [Pure]
    public static Unit _Hz => Physics.Systems.SI.Hz;

    /// <summary>
    /// Unit of force: newton (N).
    /// </summary>
    [Pure]
    public static Unit _N => Physics.Systems.SI.N;

    /// <summary>
    /// Unit of pressure: pascal (Pa).
    /// </summary>
    [Pure]
    public static Unit _Pa => Physics.Systems.SI.Pa;

    /// <summary>
    /// Unit of energy: joule (J).
    /// </summary>
    [Pure]
    public static Unit _J => Physics.Systems.SI.J;

    /// <summary>
    /// Unit of power: watt (W).
    /// </summary>
    [Pure]
    public static Unit _W => Physics.Systems.SI.W;

    /// <summary>
    /// Unit of electric charge: coulomb (C).
    /// </summary>
    [Pure]
    public static Unit _C => Physics.Systems.SI.C;

    /// <summary>
    /// Unit of electric potential: volt (V).
    /// </summary>
    [Pure]
    public static Unit _V => Physics.Systems.SI.V;

    /// <summary>
    /// Unit of capacitance: farad (F).
    /// </summary>
    [Pure]
    public static Unit _F => Physics.Systems.SI.F;

    /// <summary>
    /// Unit of electrical resistance: ohm (Ω).
    /// </summary>
    [Pure]
    public static Unit _Ω => Physics.Systems.SI.Ω;

    /// <summary>
    /// Unit of electrical conductance: siemens (S).
    /// </summary>
    [Pure]
    public static Unit _S => Physics.Systems.SI.S;

    /// <summary>
    /// Unit of magnetic flux: weber (Wb).
    /// </summary>
    [Pure]
    public static Unit _Wb => Physics.Systems.SI.Wb;

    /// <summary>
    /// Unit of magnetic flux density: tesla (T).
    /// </summary>
    [Pure]
    public static Unit _T => Physics.Systems.SI.T;

    /// <summary>
    /// Unit of inductance: henry (H).
    /// </summary>
    [Pure]
    public static Unit _H => Physics.Systems.SI.H;

    /// <summary>
    /// Unit of illuminance: lux (lx).
    /// </summary>
    [Pure]
    public static Unit _lx => Physics.Systems.SI.lx;

    /// <summary>
    /// Unit of ionizing radiation dose: sievert (Sv).
    /// </summary>
    [Pure]
    public static Unit _Sv => Physics.Systems.SI.Sv;

    /// <summary>
    /// Unit of catalytic activity: katal (kat).
    /// </summary>
    [Pure]
    public static Unit _kat  => Physics.Systems.SI.kat;
}