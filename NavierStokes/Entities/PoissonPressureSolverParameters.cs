namespace NavierStokes;

/// <summary>
/// The <c>PoissonPressureSolverParameters</c> class holds the parameters
/// for the Poisson pressure solver used in fluid dynamics simulations.
/// </summary>
public sealed class PoissonPressureSolverParameters
{
    /// <summary>
    /// Gets or sets the tolerance of error for convergence of the Poisson solver
    /// of the pressure field. A good starting value is 0.001, which works for most
    /// incompressible flow applications.
    /// </summary>
    public double Error { get; set; } = 0.001;

    /// <summary>
    /// Gets or sets the maximum number of iterations allowed for the Poisson solver.
    /// Increasing this value allows for further convergence of the pressure solver.
    /// </summary>
    public int MaxIterations { get; set; } = 1000;

    /// <summary>
    /// Gets or sets the minimum number of iterations allowed for the Poisson solver.
    /// Increasing this value allows for further convergence of the pressure solver.
    /// Note that this value should be less than <see cref="MaxIterations"/>.
    /// </summary>
    public int MinIterations { get; set; } = 1;
}