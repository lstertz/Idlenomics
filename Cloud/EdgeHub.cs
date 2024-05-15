using Cloud.Tracking;
using Cloud.World;
using Microsoft.AspNetCore.SignalR;

namespace Cloud;

/// <summary>
/// Manages the connections/disconnections and the messages of Edges to this Cloud.
/// </summary>
public partial class EdgeHub(ILogger<EdgeHub> _logger, IWorldUpdater _worldUpdater) : Hub
{
    /// <inheritdoc/>
    public override Task OnConnectedAsync()
    {
        _logger.LogDebug("Edge connected.");

        return base.OnConnectedAsync();
    }
}
