using Edge.Users;
using Microsoft.AspNetCore.SignalR;

namespace Edge;

/// <summary>
/// Manages the connections/disconnections and requests of a user connecting 
/// to this Edge through a Client.
/// </summary>
public class ClientHub(IUserManager _userManager, ILogger<ClientHub> _logger) : Hub
{
    /// <inheritdoc/>
    public override Task OnConnectedAsync()
    {
        _logger.LogDebug("Client connected.");

        // TODO :: Provide a unique non personally identifiable user ID upon connection.

        _userManager.RegisterUser("");
        _userManager.ConnectUser("", Context.ConnectionId);

        return base.OnConnectedAsync();
    }

    /// <inheritdoc/>
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _userManager.DisconnectUser("", Context.ConnectionId);

        return base.OnDisconnectedAsync(exception);
    }
}
