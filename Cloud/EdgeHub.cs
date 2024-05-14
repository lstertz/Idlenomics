using Cloud.Tracking;
using Cloud.Updating;
using Microsoft.AspNetCore.SignalR;
using Shared.Players;

namespace Cloud;

/// <summary>
/// Manages the connections/disconnections and the messages of Edges to this Cloud.
/// </summary>
public class EdgeHub(ILogger<EdgeHub> _logger, IPlayerTracker _playerTracker, 
    IWorldUpdater _worldUpdater) : Hub
{
    /// <inheritdoc/>
    public override Task OnConnectedAsync()
    {
        _logger.LogDebug("Edge connected.");

        return base.OnConnectedAsync();
    }


    /// <summary>
    /// Handles player updates streamed from an Edge.
    /// </summary>
    /// <param name="updates">The updates to be handled.</param>
    /// <returns>A task to await the asynchronous handling of the streamed updates.</returns>
    public async Task HandleStreamedPlayerUpdates(IAsyncEnumerable<PlayerUpdate> updates)
    {
        await foreach (var update in updates)
        {
            _logger.LogDebug("Received a simulation update for player, {playerId}: {update}", 
                update.PlayerId, update.SimulationUpdate.Value);

            _worldUpdater.QueueDiff(
                _playerTracker.UpdatePlayerData(update));
        }
    }


    // TODO :: Implement stream (at a specific stream rate) for the current world state.
}
