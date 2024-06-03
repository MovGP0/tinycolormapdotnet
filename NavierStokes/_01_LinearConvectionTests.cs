using NavierStokes.Solvers;

namespace NavierStokes;

[TestFixture]
public sealed class _01_LinearConvectionTests
{
    public _01_LinearConvectionTests()
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
    }

    [Test]
    public void LinearWavePropagation()
    {
        const int nx = 121;
        Quantity nx_ = new(nx, _m);
        var u = LinearConvergence.Calculate(nx);
        PlotHelper.PlotDataToFile(u, nx_);
    }
}