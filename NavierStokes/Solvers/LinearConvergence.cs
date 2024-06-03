using static TorchSharp.torch;

namespace NavierStokes.Solvers;

public static class LinearConvergence
{
    /// <summary>
    /// Calculate the linear convergence of the wave propagation
    /// </summary>
    /// <param name="nx">The number of grid points in x-direction</param>
    /// <param name="nt">The number of time-steps we want to calculate</param>
    /// <returns></returns>
    public static Tensor Calculate(int nx = 41, int nt = 25, double sigma = 0.5)
    {
        const ScalarType dtype = ScalarType.Float64;

        // set up variables
        Quantity nx_ = new(nx, _m);
        Quantity dx = 2d / (nx_ - new Quantity(1, _m)); // distance between grid points
        Quantity c = new(1 /*299_792_458*/, _m / _s); // speed of wave in the given medium
        Quantity sigma_ = new(sigma, _m * _s);

        var dt = sigma_ * dx;

        // velocities u [m/s]
        var u = ones((int)nx_.Amount, dtype: dtype);

        // set up initial condition of the velocities u[m/s]
        var rangeStart = (int)(.5d / dx.Amount);
        var rangeEnd = (int)(1d / dx.Amount + 1d);
        u[rangeStart..rangeEnd] = 2f;

        var σ = c * dt / dx; // courant number

        for (var n = 0; n < nt; n++)
        {
            var i = 1..(int)nx_.Amount;
            var iPrev = ..((int)nx_.Amount - 1);
            u[i] -= σ.Amount * (u[i] - u[iPrev]);
        }

        return u;
    }
}