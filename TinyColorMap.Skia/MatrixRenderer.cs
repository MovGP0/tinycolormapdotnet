namespace TinyColorMap;

public static class MatrixRenderer
{
    [Pure]
    public static SKBitmap CreateMatrixVisualization(
        double[,] matrix,
        ColormapType type = ColormapType.Viridis,
        uint quantization = uint.MaxValue)
    {
        bool doQuantization = quantization != uint.MaxValue;
        var width = matrix.GetLength(0);
        var height = matrix.GetLength(1);
        var (minCoeff, maxCoeff) = FindMinMax(matrix);
        var range = maxCoeff - minCoeff;

        SKBitmap bitmap = new(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);

        for (var x = 0; x < width; ++x)
        for (var y = 0; y < height; ++y)
        {
            var normalizedValue = (matrix[x, y] - minCoeff) / range;
            var color = doQuantization
                ? Color.GetColor(normalizedValue, quantization, type)
                : Color.GetColor(normalizedValue, type);
            bitmap.SetPixel(x, y, color.ToSkiaColor());
        }

        return bitmap;
    }

    [Pure]
    private static (double min, double max) FindMinMax(double[,] matrix)
    {
        var min = double.MaxValue;
        var max = double.MinValue;

        foreach (var value in matrix)
        {
            if (value < min)
                min = value;
            if (value > max)
                max = value;
        }

        return (min, max);
    } 
}
