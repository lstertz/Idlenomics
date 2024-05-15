namespace Cloud.World;

/// <summary>
/// The change from a previous world state to the next updated world state.
/// </summary>
public class WorldStateDiff
{
    /// <summary>
    /// The change in the world value.
    /// </summary>
    public double ValueChange { get; init; }
}
