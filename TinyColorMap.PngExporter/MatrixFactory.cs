namespace TinyColorMap.PngExporter;

public static class MatrixFactory
{
    public static double[,] Create(int width, int height)
    {
        double[,] matrix = new double[width, height];
        for (int x = 0; x < width; ++x)
        {
            var value = (double)x / (double)width - 1.0;
            for (int y = 0; y < height; ++y)
            {
                matrix[x, y] = value;
            }
        }
        return matrix;
    }
}
