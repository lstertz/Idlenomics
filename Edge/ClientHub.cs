using Microsoft.AspNetCore.SignalR;

namespace Edge;

public class ClientHub(ILogger<ClientHub> _logger) : Hub
{
    public override Task OnConnectedAsync()
    {
        _logger.LogDebug("Client connected.");

        return base.OnConnectedAsync();
    }
}
