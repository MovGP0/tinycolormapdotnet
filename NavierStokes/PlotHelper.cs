using System.Diagnostics;
using System.Runtime.CompilerServices;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using static TorchSharp.torch;

namespace NavierStokes;

public static class PlotHelper
{
    public static void PlotDataToFile(
        Tensor u,
        Quantity nx,
        [CallerMemberName]string memberName = "")
    {
        string dir = Path.GetTempPath();
        string fileName = $"{nameof(_01_LinearConvectionTests)}.{memberName}.svg";
        var filePath = Path.Combine(dir, fileName);

        var data = u.data<double>()
            .Select(value => new Quantity(value, _m / _s))
            .ToArray();

        PlotData(data, nx, filePath);

        using var process = new Process();
        process.StartInfo = new()
        {
            FileName = filePath,
            UseShellExecute = true,
            Verb = "open"
        };
        process.Start();
    }

    private static void PlotData(Quantity[] data, Quantity nx, string filePath)
    {
        // create plot
        var dataPoints = data
            .Select((value, pos) => new DataPoint(pos, value.Amount))
            .ToArray();

        var series = new LineSeries();
        series.Points.AddRange(dataPoints);

        var plot = new PlotModel();

        plot.Axes.Add(new LinearAxis
        {
            Title = "Distance",
            Unit = nx.Unit.ToString(),
            Position = AxisPosition.Bottom
        });

        plot.Axes.Add(new LinearAxis
        {
            Title = "Velocity",
            Unit = data[0].Unit.ToString(),
            Position = AxisPosition.Left
        });

        plot.Series.Add(series);

        using var fileStream = File.OpenWrite(filePath);
        var exporter = new OxyPlot.SkiaSharp.SvgExporter
        {
            Height = 600,
            Width = 800
        };
        exporter.Export(plot, fileStream);
        fileStream.Flush();
    }
}