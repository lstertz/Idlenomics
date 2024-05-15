namespace Cloud.World;

/// <summary>
/// A state of the world.
/// </summary>
public class WorldState
{
    /// <summary>
    /// The time at which this state was created.
    /// </summary>
    public DateTime CreationTime { get; } = DateTime.UtcNow;

    /// <summary>
    /// The value of the world for this state.
    /// </summary>
    public double Value { get; init; }
}
