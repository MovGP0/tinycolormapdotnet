using NavierStokes.Entities;

namespace NavierStokes.Solvers;

public sealed class VelocityCorrectionSolver
{
    /// <summary>
    /// Correct velocities with pressure gradient
    /// </summary>
    public static void Solve(NavierStokesVariables vars, ScenarioParameters scenarioParameters, int sizeX, int sizeY)
    {
        Quantity[,] velocityX = vars.U;
        Quantity[,] velocityY  = vars.V;
        Quantity[,] initialPressure = vars.P0;
        bool[,] wallCondition = vars.SolidWall;
        Quantity dt = scenarioParameters.Dt;
        Quantity density = scenarioParameters.Density;

        // Grid spacing, assuming a unit domain size
        Quantity dx = new(1.0 / sizeX, _m);
        Quantity dy = new(1.0 / sizeY, _m);

        for (int x = 1; x <= sizeX; x++)
        for (int y = 1; y <= sizeY; y++)
        {
            if (wallCondition[x, y])
            {
                // Only correct velocities for fluid cells, not solid walls
                continue;
            }

            // Correct the x-component of the velocity
            velocityX[x, y] -= dt * (initialPressure[x, y + 1] - initialPressure[x, y]) / (density * dx);

            // Correct the y-component of the velocity
            velocityY[x, y] -= dt * (initialPressure[x + 1, y] - initialPressure[x, y]) / (density * dy);
        }
    }
}