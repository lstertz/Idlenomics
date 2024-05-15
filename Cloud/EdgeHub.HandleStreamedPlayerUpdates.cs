using Cloud.Tracking;
using Shared.Players;

namespace Cloud;

public partial class EdgeHub
{
    /// <summary>
    /// Handles player updates streamed from an Edge.
    /// </summary>
    /// <param name="updates">The updates to be handled.</param>
    /// <returns>A task to await the asynchronous handling of the streamed updates.</returns>
    public async Task HandleStreamedPlayerUpdates(IPlayerTracker _playerTracker,
        IAsyncEnumerable<PlayerUpdate> updates)
    {
        await foreach (var update in updates)
            _worldUpdater.QueueDiff(
                _playerTracker.UpdatePlayerData(update));
    }
}
