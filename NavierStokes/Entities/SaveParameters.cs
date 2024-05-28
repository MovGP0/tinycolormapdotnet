namespace NavierStokes;

/// <summary>
/// Holds the parameters related to saving data during the fluid dynamics simulation.
/// </summary>
public sealed class SaveParameters
{
    /// <summary>
    /// Gets or sets the limit for hard drive space usage (in gigabytes) for externally saved data.
    /// </summary>
    public double SpaceLimit { get; set; } = 5;

    /// <summary>
    /// Gets or sets the dimensions of the data chunks to be saved.
    /// The array represents the size of each chunk in a multidimensional format.
    /// </summary>
    public int[] ChunkDimensions { get; set; } = [100, 100, 500];
}