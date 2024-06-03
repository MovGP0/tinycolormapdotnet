using NavierStokes.Solvers;

namespace NavierStokes;

[TestFixture]
public sealed class _02_NonLinearConvectionTests
{
    public _02_NonLinearConvectionTests()
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
    }

    [Test]
    public void CalculateNonLinearConvection()
    {
        const int nx = 25;
        Quantity nx_ = new(nx, _m);
        var u = NonLinearConvection.Calculate(nx);
        PlotHelper.PlotDataToFile(u, nx_);
    }
}