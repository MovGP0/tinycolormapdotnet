using System.Reflection;
using NavierStokes.Entities;
using NavierStokes.Solvers;
using SkiaSharp;

namespace NavierStokes;

[TestFixture]
public sealed class Tests
{
    [Test]
    public void ExecuteScenario()
    {
        // load Scenario
        var scenarioParameters = new ScenarioParameters("Scenarios/scenario_driven_lid.png");
        NavierStokesVariables vars = LoadScenarioFile(scenarioParameters);
        var xinc = scenarioParameters.Xinc;

        // Poisson Pressure solver parameters
        var ppsp = new PoissonPressureSolverParameters();

        // Save parameters
        var saveParameters = new SaveParameters();

        // Main calculations loop
        Quantity time = new(0, _s);
        int T = 0;
        int T2 = 0;
        int TLS = 1;
        int T0 = 1;
        int ts = (int)Math.Round((double)(saveParameters.ChunkDimensions[0] * saveParameters.ChunkDimensions[1] * saveParameters.ChunkDimensions[2]) / (xinc * xinc));
        int XI = xinc;
        int YI = xinc;
        int nsave = 0;

        while (T < scenarioParameters.MI)
        {
            BoundaryConditionsSolver.Apply(vars);
            TemporaryVelocitySolver.Solve(vars, scenarioParameters, XI, YI);
            PoissonPressureSolver.Solve(vars, ppsp, XI, YI);
            VelocityCorrectionSolver.Solve(vars, scenarioParameters, XI, YI);

            // Save velocities periodically
            if (++T2 >= ts)
            {
                SaveVelocities(vars.velx, vars.vely, nsave++);
                T2 = 0;
            }

            T++;
            time += scenarioParameters.Dt;
        }

        Console.WriteLine("Calculations are 100% complete!");
    }

    /// <summary>
    /// Save velocities to external files
    /// </summary>
    static void SaveVelocities(Quantity[,] velx, Quantity[,] vely, int nsave)
    {
        // TODO: Implement this method
    }

    /// <summary>
    /// Loads the specified scenario image as a SkiaSharp SKCanvas.
    /// </summary>
    /// <param name="scenarioParameters">The parameters for the current scenario</param>
    /// <returns>The loaded image as an SKCanvas.</returns>
    private static NavierStokesVariables LoadScenarioFile(ScenarioParameters scenarioParameters)
    {
        // Get the location of the current NUnit test DLL
        string dllLocation = Assembly.GetExecutingAssembly().Location;
        string directory = Path.GetDirectoryName(dllLocation);

        // Construct the full file path
        string filePath = Path.Combine(directory, scenarioParameters.ScenarioFile);

        // Load the PNG file as an SKBitmap
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Scenario file not found", filePath);
        }

        using var stream = File.OpenRead(filePath);
        var bitmap = SKBitmap.Decode(stream);

        var vars = new NavierStokesVariables(scenarioParameters.Xinc, scenarioParameters.Xinc);

        // TODO: decode the image and extract the scenario parameters

        return vars;
    }
}