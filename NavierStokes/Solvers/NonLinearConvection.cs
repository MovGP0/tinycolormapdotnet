using static TorchSharp.torch;

namespace NavierStokes.Solvers;

public static class NonLinearConvection
{
    public static Tensor Calculate(int nx = 41, int nt = 25)
    {
        const ScalarType dtype = ScalarType.Float64;

        // set up variables
        Quantity nx_ = new(nx, _m); // number of grid points in x-direction
        Quantity dx = 2d / (nx_ - new Quantity(1, _m)); // distance between grid points
        Quantity dt = new(250, UnitPrefix.m * _s); // dt is the amount of time each timestep covers (delta t)
        Quantity c = new(1 /*299_792_458*/, _m / _s); // speed of wave in the given medium

        // velocities u [m/s]
        var u = ones((int)nx_.Amount, dtype: dtype);

        // set up initial condition of the velocities u[m/s]
        var rangeStart = (int)(.5d / dx.Amount);
        var rangeEnd = (int)(1d / dx.Amount + 1d);
        u[rangeStart..rangeEnd] = 2f;

        var p = dt / dx;

        for (var n = 0; n < nt; n++)
        {
            var i = 1..(int)nx_.Amount;
            var iPrev = ..((int)nx_.Amount - 1);

            var σ = u[i] * p.Amount; // courant number
            u[i] -= σ * (u[i] - u[iPrev]);
        }

        return u;
    }
}