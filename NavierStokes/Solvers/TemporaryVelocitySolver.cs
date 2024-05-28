using NavierStokes.Entities;

namespace NavierStokes.Solvers;

public static class TemporaryVelocitySolver
{
    /// <summary>
    /// Calculate temporary velocities using convective and diffusive terms
    /// </summary>
    public static void Solve(NavierStokesVariables vars, ScenarioParameters scenarioParameters, int sizeX, int sizeY)
    {
        Quantity[,] velocityX = vars.U;
        Quantity[,] velocityY = vars.V;
        bool[,] solidWallIndicator = vars.SolidWall;
        Quantity dt = scenarioParameters.Dt;
        Quantity mu = scenarioParameters.Mu;
        Quantity dens = scenarioParameters.Density;

        // Constants for finite difference calculations
        Quantity L = mu / dens; // Kinematic viscosity
        Quantity dx = new(1.0 / sizeX, _m); // Grid spacing, assuming a unit domain size

        // Temporary velocity arrays
        Quantity[,] Utemp = new Quantity[sizeY + 2, sizeX + 2];
        Quantity[,] Vtemp = new Quantity[sizeY + 2, sizeX + 2];

        for (int x = 1; x <= sizeY; x++)
        for (int y = 1; y <= sizeX; y++)
        {
            if (solidWallIndicator[x, y]) // If the cell is not a solid wall
            {
                // Convective terms
                var convU = ((velocityX[x, y] + velocityX[x, y + 1]) / 2 * (velocityX[x, y + 1] - velocityX[x, y]) / dx +
                             (velocityX[x, y] + velocityX[x + 1, y]) / 2 * (velocityX[x + 1, y] - velocityX[x, y]) / dx);

                var convV = ((velocityY[x, y] + velocityY[x, y + 1]) / 2 * (velocityY[x, y + 1] - velocityY[x, y]) / dx +
                             (velocityY[x, y] + velocityY[x + 1, y]) / 2 * (velocityY[x + 1, y] - velocityY[x, y]) / dx);

                // Diffusive terms
                var diffU = L * ((velocityX[x, y + 1] - 2 * velocityX[x, y] + velocityX[x, y - 1]) / (dx * dx) +
                                 (velocityX[x + 1, y] - 2 * velocityX[x, y] + velocityX[x - 1, y]) / (dx * dx));

                var diffV = L * ((velocityY[x, y + 1] - 2 * velocityY[x, y] + velocityY[x, y - 1]) / (dx * dx) +
                                                       (velocityY[x + 1, y] - 2 * velocityY[x, y] + velocityY[x - 1, y]) / (dx * dx));

                // Update temporary velocities
                Utemp[x, y] = velocityX[x, y] + dt * new Quantity(-convU.Amount + diffU.Amount, _m/(_s*_s));
                Vtemp[x, y] = velocityY[x, y] + dt * new Quantity(-convV.Amount + diffV.Amount, _m/(_s*_s));
            }
        }

        // Copy temporary velocities back to original arrays
        for (int x = 1; x <= sizeY; x++)
        for (int y = 1; y <= sizeX; y++)
        {
            velocityX[x, y] = Utemp[x, y];
            velocityY[x, y] = Vtemp[x, y];
        }
    }
}