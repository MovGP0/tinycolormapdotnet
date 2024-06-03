using static TorchSharp.torch;

namespace NavierStokes.Solvers;

public static class DiffusionEquation
{
    public static Tensor Calculate()
    {
        Quantity nx = new(41, _m);
        Quantity dx = 2d / (nx - new Quantity(1, _m)); // distance between grid points
        double nt = 20; // The number of time-steps we want to calculate
        Quantity nu = new(0.3, _Pa * _s); // The value of viscosity
        Quantity sigma = new(0.2, _m * _s);
        var dt = sigma * dx * dx / nu;

        var u = ones((int)nx.Amount);

        var inMin = (int)(.5d / dx.Amount);
        var inMax = (int)(1d / dx.Amount + 1d);
        u[inMin..inMax] = 2;

        for (var n = 0; n < nt; n++)
        {
            var un = u.clone();
            for (var i = 1; i < nx.Amount - 1; i++)
            {
                var s = (nu * dt / (dx * dx)).Amount;
                u[i] = un[i] + s * (un[i + 1] - 2d * un[i] + un[i - 1]);
            }
        }

        return u;
    }
}