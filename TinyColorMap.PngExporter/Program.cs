using System.CommandLine;
using System.Reflection;
using SkiaSharp;
using TinyColorMap;
using TinyColorMap.PngExporter;
using TinyColorMap.Skia;

var pathOption = new Option<string>(
    name: "--path",
    description: "The path to save the image to",
    getDefaultValue: () =>
    {
        var location = Assembly.GetExecutingAssembly().Location;
        return Path.GetPathRoot(location)!;
    });

var withOption = new Option<int>(
    name: "--width",
    description: "The width of the image",
    getDefaultValue: () => 300);

var heightOption = new Option<int>(
    name: "--height",
    description: "The height of the image",
    getDefaultValue: () => 30);

RootCommand rootCommand = new();
rootCommand.AddOption(pathOption);
rootCommand.AddOption(withOption);
rootCommand.AddOption(heightOption);

rootCommand.SetHandler((path, width, height) =>
{
    var matrix = MatrixFactory.Create(width, height);
    foreach (var colorMap in ColormapTypeExtensions.GetValues())
    {
        var fileName = $"{colorMap.ToStringFast()}.png";
        var filePath = Path.Combine(path, fileName);
        using var bitmap = MatrixRenderer.CreateMatrixVisualization(matrix, colorMap);
        using var image = SKImage.FromBitmap(bitmap);
        using var encoded = image.Encode(SKEncodedImageFormat.Png, 100);
        using FileStream fileStream = new(filePath, FileMode.Create, FileAccess.Write);
        encoded.SaveTo(fileStream);
    }
}, pathOption, withOption, heightOption);

await rootCommand.InvokeAsync(args);
