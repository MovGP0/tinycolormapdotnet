namespace TinyColorMap.Skia;

public static class ColorExtensions
{
    [Pure]
    public static SKColor ToSkiaColor(this Color color) =>
        new(color.Ri, color.Gi, color.Bi);
}