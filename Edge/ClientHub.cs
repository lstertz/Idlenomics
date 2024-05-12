using Edge.Players;
using Microsoft.AspNetCore.SignalR;

namespace Edge;

/// <summary>
/// Manages the connections/disconnections and requests of a player connecting 
/// to this Edge through a Client.
/// </summary>
public partial class ClientHub(IPlayerManager _playerManager, ILogger<ClientHub> _logger) : Hub
{
    private const string PlayerIdKey = "playerId";

    private string PlayerId
    {
        get
        {
            if (Context.Items.TryGetValue(PlayerIdKey, out var id))
                return (string)id!;

            var query = Context.GetHttpContext()!.Request.Query[PlayerIdKey];
            if (query.Count() > 0)
                id = query[0];
            else
                id = string.Empty;

            Context.Items.Add(PlayerIdKey, id);

            return (string)id!;
        }
    }


    /// <inheritdoc/>
    public override Task OnConnectedAsync()
    {
        var playerId = PlayerId;

        _playerManager.RegisterPlayer(playerId);
        _playerManager.ConnectPlayer(playerId, Context.ConnectionId);

        _logger.LogDebug($"Client connected with player ID: {playerId}");

        return base.OnConnectedAsync();
    }

    /// <inheritdoc/>
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var playerId = PlayerId;
        _playerManager.DisconnectPlayer(playerId, Context.ConnectionId);

        _logger.LogDebug($"Client disconnected with player ID: {playerId}");

        return base.OnDisconnectedAsync(exception);
    }
}
