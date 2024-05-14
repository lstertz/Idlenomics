namespace Cloud.Updating;

/// <summary>
/// Updates and maintains the state of the world.
/// </summary>
public interface IWorldUpdater
{
    /// <summary>
    /// The current state of the world.
    /// </summary>
    double CurrentWorldState { get; }

    /// <summary>
    /// Queues a change to the world state to be processed on the next update.
    /// </summary>
    /// <param name="diff">The difference from the last known state to what should 
    /// be the current state.</param>
    void QueueDiff(UpdateDiff diff);
}