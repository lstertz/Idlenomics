using Microsoft.AspNetCore.SignalR;
using Shared.Players;

namespace Cloud;

public class EdgeHub(ILogger<EdgeHub> _logger) : Hub
{
    public override Task OnConnectedAsync()
    {
        _logger.LogDebug("Edge connected.");

        return base.OnConnectedAsync();
    }

    public async Task ReceiveStreamedSimulationUpdates(IAsyncEnumerable<PlayerUpdate> updates)
    {
        await foreach (var update in updates)
        {
            _logger.LogDebug("Received a simulation update for player, {playerId}: {update}", 
                update.PlayerId, update.SimulationUpdate.Value);
        }
    }
}
