using NavierStokes.Entities;

namespace NavierStokes.Solvers;

public static class PoissonPressureSolver
{
    /// <summary>
    /// Solve Poisson equation for pressure
    /// </summary>
    public static void Solve(NavierStokesVariables vars, PoissonPressureSolverParameters parameters, int sizeX, int sizeY)
    {
        Quantity[,] U = vars.U;
        Quantity[,] V = vars.V;
        Quantity[,] p0 = vars.P0; // initial pressure
        Quantity[,] p1 = vars.P1; // pressure after solving Poisson equation
        double[,] f = vars.f;
        bool[,] solidWall = vars.SolidWall;

        double dt = parameters.Error;
        int maxIterations = parameters.MaxIterations;
        int minIterations = parameters.MinIterations;

        // Constants for finite difference calculations
        Quantity dx = new(1.0 / sizeX, _m); // Grid spacing
        Quantity dy = new(1.0 / sizeY, _m);

        Quantity dx2 = dx * dx;
        Quantity dy2 = dy * dy;
        Quantity denom = 2.0 * (dx2 + dy2);

        // Iteration counters and error tolerance
        int iterations = 0;
        double errorTolerance = 1e-4; // This value can be adjusted as needed
        double maxError;

        do
        {
            maxError = 0.0;

            for (int x = 1; x <= sizeX; x++)
            for (int y = 1; y <= sizeY; y++)
            {
                if (solidWall[x, y]) // Only solve for fluid cells, not solid walls
                {
                    var rhs = ((U[x, y + 1] - U[x, y]) / dx + (V[x + 1, y] - V[x, y]) / dy) / dt;

                    // Jacobi iteration for Poisson equation
                    p1[x, y] = ((p0[x + 1, y] + p0[x - 1, y]) * dy2 + (p0[x, y + 1] + p0[x, y - 1]) * dx2 - rhs * dx2 * dy2) / denom;

                    // Calculate the error for convergence check
                    double error = Math.Abs(p1[x, y].Amount - p0[x, y].Amount);
                    if (error > maxError)
                    {
                        maxError = error;
                    }
                }
            }

            // Copy P1 back to P0 for the next iteration
            for (int x = 1; x <= sizeX; x++)
            for (int y = 1; y <= sizeY; y++)
            {
                p0[x, y] = p1[x, y];
            }

            iterations++;
        }
        while (iterations < maxIterations && maxError > errorTolerance);

        // Ensure minimum number of iterations
        for (int minIter = 0; minIter < minIterations; minIter++)
        {
            for (int x = 1; x <= sizeX; x++)
            for (int y = 1; y <= sizeY; y++)
            {
                if (!solidWall[x, y]) // Only solve for fluid cells, not solid walls
                {
                    var rhs = ((U[x, y + 1] - U[x, y]) / dx + (V[x + 1, y] - V[x, y]) / dy) / dt;

                    // Jacobi iteration for Poisson equation
                    p1[x, y] = ((p0[x + 1, y] + p0[x - 1, y]) * dy2 + (p0[x, y + 1] + p0[x, y - 1]) * dx2 - rhs * dx2 * dy2) / denom;
                }
            }

            // Copy P1 back to P0 for the next iteration
            for (int x = 1; x <= sizeX; x++)
            for (int y = 1; y <= sizeY; y++)
            {
                p0[x, y] = p1[x, y];
            }
        }
    }
}