using MathNet.Numerics.LinearAlgebra;

namespace Physics;

public static class VectorExtensions
{
    public static double EuclideanLengthSquared(this Vector<double> vector) => vector.DotProduct(vector);
}