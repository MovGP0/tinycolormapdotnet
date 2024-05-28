namespace NavierStokes.Entities;

/// <summary>
/// This class holds all the necessary variables for the 2D Navier-Stokes incompressible flow simulation.
/// </summary>
public sealed class NavierStokesVariables
{
    /// <summary>
    /// The x-component of the velocity field on a staggered grid.
    /// </summary>
    public Quantity[,] U { get; set; }

    /// <summary>
    /// The y-component of the velocity field on a staggered grid.
    /// </summary>
    public Quantity[,] V { get; set; }

    /// <summary>
    /// The initial pressure field.
    /// </summary>
    public Quantity[,] P0 { get; set; }

    /// <summary>
    /// An intermediate pressure field used in calculations.
    /// </summary>
    public Quantity[,] P01 { get; set; }

    /// <summary>
    /// The updated pressure field after solving the Poisson equation.
    /// </summary>
    public Quantity[,] P1 { get; set; }

    /// <summary>
    /// The right-hand side of the Poisson equation, derived from the continuity equation.
    /// </summary>
    public double[,] f { get; set; }

    /// <summary>
    /// An indicator for solid walls or constant velocity regions; 1 if not a solid wall node, otherwise 0.
    /// </summary>
    public bool[,] SolidWall { get; set; }

    /// <summary>
    /// An indicator for outflow nodes; 1 if the node is an outflow node, otherwise 0.
    /// </summary>
    public bool[,] OF { get; set; }

    /// <summary>
    /// The x-component of the velocity field on an unstaggered grid.
    /// </summary>
    public Quantity[,] velx { get; set; }

    /// <summary>
    /// The y-component of the velocity field on an unstaggered grid.
    /// </summary>
    public Quantity[,] vely { get; set; }

    /// <summary>
    /// The initial x-component of the velocity field used for visualization.
    /// </summary>
    public Quantity[,] LRFX { get; set; }

    /// <summary>
    /// The initial y-component of the velocity field used for visualization.
    /// </summary>
    public Quantity[,] LRFY { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="NavierStokesVariables"/> class with specified grid dimensions.
    /// </summary>
    /// <param name="xinc">The number of grid points in the x-direction.</param>
    /// <param name="yinc">The number of grid points in the y-direction.</param>
    public NavierStokesVariables(int xinc, int yinc)
    {
        U = new Quantity[yinc + 2, xinc + 2];
        V = new Quantity[yinc + 2, xinc + 2];
        P0 = new Quantity[yinc + 2, xinc + 2];
        P01 = new Quantity[yinc + 2, xinc + 2];
        P1 = new Quantity[yinc + 2, xinc + 2];
        f = new double[yinc + 2, xinc + 2];
        SolidWall = new bool[yinc + 2, xinc + 2];
        OF = new bool[yinc + 2, xinc + 2];
        velx = new Quantity[yinc, xinc];
        vely = new Quantity[yinc, xinc];
        LRFX = new Quantity[yinc, xinc];
        LRFY = new Quantity[yinc, xinc];
    }
}