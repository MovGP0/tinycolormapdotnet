namespace NavierStokes.Entities;

/// <summary>
/// This class holds the parameters for a fluid dynamics simulation scenario.
/// </summary>
public class ScenarioParameters
{
    /// <summary>
    /// The file name of the scenario image.
    /// </summary>
    public string ScenarioFile { get; set; }

    /// <summary>
    /// The length of the domain in the x-axis (meters).
    /// </summary>
    public Quantity DomainX { get; set; }

    /// <summary>
    /// The number of nodes across the x-component of the domain.
    /// </summary>
    public int Xinc { get; set; }

    /// <summary>
    /// The time step (seconds).
    /// </summary>
    public Quantity Dt { get; set; }

    /// <summary>
    /// The number of time steps to perform calculations.
    /// </summary>
    public int MI { get; set; }

    /// <summary>
    /// The y-component velocity of the region with constant velocity (meters/second).
    /// </summary>
    public Quantity Velyi { get; set; }

    /// <summary>
    /// The x-component velocity of the region with constant velocity (meters/second).
    /// </summary>
    public Quantity Velxi { get; set; }

    /// <summary>
    /// The density (kg/m^3).
    /// </summary>
    public Quantity Density { get; set; }

    /// <summary>
    /// The dynamic viscosity (Pa*s).
    /// </summary>
    public Quantity Mu { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ScenarioParameters"/> class with default values.
    /// </summary>
    public ScenarioParameters(string fileName)
    {
        ScenarioFile = fileName;
        DomainX = new(2.0, _m);
        Xinc = 200;
        Dt = new(0.0015, _s);
        MI = 3000;
        Velyi = new(0.0, _m/_s);
        Velxi = new(1.0, _m/_s);
        Density = new(1.0, _kg/_m*_m*_m);
        Mu = new(1 / 1000.0, _Pa/_s);
    }
}