namespace TinyColorMap;

[NetEscapades.EnumGenerators.EnumExtensions]
public enum ColormapType
{
#region Matlab
    Parula = 0,
    Heat = 1,
    Jet = 3,
    Hot = 2,
    Gray = 4,
    HSV = 5,
#endregion

#region Matplotlib
    Magma = 6,
    Inferno = 7,
    Plasma = 8,
    Viridis = 9,
    Cividis = 10,
#endregion

    Github = 11,
    Turbo = 12,
    Cubehelix = 13,
}
