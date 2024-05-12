using Microsoft.AspNetCore.SignalR;

namespace Cloud;

public class EdgeHub(ILogger<EdgeHub> _logger) : Hub
{
    public override Task OnConnectedAsync()
    {
        _logger.LogDebug("Edge connected.");

        return base.OnConnectedAsync();
    }

    public async Task ReceiveStreamedSimulationUpdates(IAsyncEnumerable<double> updates)
    {
        await foreach (var update in updates)
        {
            _logger.LogDebug("Received a simulation update: {update}", update);
        }
    }
}
