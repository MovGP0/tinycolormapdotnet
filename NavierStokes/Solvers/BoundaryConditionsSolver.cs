using NavierStokes.Entities;

namespace NavierStokes.Solvers;

public static class BoundaryConditionsSolver
{
    /// <summary>
    /// Unit of speed [m/s]
    /// </summary>
    private static Unit _mps => _m / _s;

    /// <summary>
    /// Velocity of 0 [m/s]
    /// </summary>
    private static Quantity v0 => new(0.0, _mps);

    /// <summary>
    /// Apply boundary conditions
    /// </summary>
    public static void Apply(NavierStokesVariables variables)
    {
        var velocityX = variables.U;
        var velocityY = variables.V;
        var solidWall = variables.SolidWall;
        var outflow = variables.OF;

        int sizeX = velocityX.GetLength(0) - 2;
        int sizeY = velocityX.GetLength(1) - 2;

        for (int x = 1; x <= sizeX; x++)
        for (int y = 1; y <= sizeY; y++)
        {
            UpdateForSolidWall(solidWall, x, y, velocityX, velocityY);
            UpdateForOutflow(outflow, x, y, velocityX, velocityY, sizeX, sizeY);
        }
    }

    private static void UpdateForOutflow(
        bool[,] outflow,
        int x,
        int y,
        Quantity[,] velocityX,
        Quantity[,] velocityY,
        int sizeX,
        int sizeY)
    {
        if (!outflow[x, y]) return;

        if (x == 1) // Top boundary
        {
            velocityX[x, y] = velocityX[x + 1, y];
            velocityY[x, y] = velocityY[x + 1, y];
        }

        if (x == sizeX + 1) // Bottom boundary
        {
            velocityX[x, y] = velocityX[x - 1, y];
            velocityY[x, y] = velocityY[x - 1, y];
        }

        if (y == 1) // Left boundary
        {
            velocityX[x, y] = velocityX[x, y + 1];
            velocityY[x, y] = velocityY[x, y + 1];
        }

        if (y == sizeY + 1) // Right boundary
        {
            velocityX[x, y] = velocityX[x, y - 1];
            velocityY[x, y] = velocityY[x, y - 1];
        }
    }

    private static void UpdateForSolidWall(
        bool[,] solidWall,
        int x,
        int y,
        Quantity[,] velocityX,
        Quantity[,] velocityY)
    {
        if (solidWall[x, y]) return;

        if (solidWall[x - 1, y]) // Wall above
        {
            velocityX[x, y] = v0;
            velocityY[x, y] = new(-velocityY[x - 1, y].Amount, _mps);
        }

        if (solidWall[x + 1, y]) // Wall below
        {
            velocityX[x + 1, y] = v0;
            velocityY[x + 1, y] = new(-velocityY[x + 2, y].Amount, _mps);
        }

        if (solidWall[x, y - 1]) // Wall to the left
        {
            velocityX[x, y] = new(-velocityX[x, y - 1].Amount, _mps);
            velocityY[x, y] = v0;
        }

        if (solidWall[x, y + 1]) // Wall to the right
        {
            velocityX[x, y + 1] = new(-velocityX[x, y + 2].Amount, _mps);
            velocityY[x, y + 1] = v0;
        }
    }
}