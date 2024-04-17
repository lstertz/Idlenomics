using Microsoft.AspNetCore.SignalR;

namespace Cloud
{
    public class EdgeHub(ILogger<EdgeHub> _logger) : Hub
    {
        public override Task OnConnectedAsync()
        {
            _logger.LogDebug("Edge connected.");

            return base.OnConnectedAsync();
        }
    }
}
