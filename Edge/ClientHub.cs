using Edge.Users;
using Microsoft.AspNetCore.SignalR;

namespace Edge;

/// <summary>
/// Manages the connections/disconnections and requests of a user connecting 
/// to this Edge through a Client.
/// </summary>
public class ClientHub(IUserManager _userManager, ILogger<ClientHub> _logger) : Hub
{
    private const string UserIdKey = "userId";

    private string UserId
    {
        get
        {
            if (Context.Items.TryGetValue(UserIdKey, out var id))
                return (string)id!;

            var query = Context.GetHttpContext()!.Request.Query[UserIdKey];
            if (query.Count() > 0)
                id = query[0];
            else
                id = string.Empty;

            Context.Items.Add(UserIdKey, id);

            return (string)id!;
        }
    }


    /// <inheritdoc/>
    public override Task OnConnectedAsync()
    {
        var userId = UserId;
        _logger.LogDebug($"Client connected with user ID: {userId}");

        _userManager.RegisterUser(userId);
        _userManager.ConnectUser(userId, Context.ConnectionId);

        return base.OnConnectedAsync();
    }

    /// <inheritdoc/>
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _userManager.DisconnectUser("", Context.ConnectionId);

        return base.OnDisconnectedAsync(exception);
    }
}
