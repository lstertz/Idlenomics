namespace Edge.Users;

/// <inheritdoc cref="IUserManager"/>
public class UserManager(ILogger<UserManager> _logger) : IUserManager
{
    /// <inheritdoc/>
    public IEnumerable<User> ConnectedUsers => _connectedUsers;
    private readonly HashSet<User> _connectedUsers = new();

    /// <inheritdoc/>
    public IEnumerable<User> Users => _users.Values;
    private readonly Dictionary<string, User> _users = new();

    /// <inheritdoc/>
    public event Action<User, string>? OnUserConnected;

    /// <inheritdoc/>
    public event Action<User>? OnUserDisconnected;


    /// <inheritdoc/>
    public void DeregisterUser(string userId)
    {
        if (!_users.TryGetValue(userId, out var user))
        {
            _logger.LogWarning("Attempted to deregister an unregistered user: {userId}", userId);
            return;
        }

        _connectedUsers.Remove(user);
        _users.Remove(userId);
    }

    /// <inheritdoc/>
    public void RegisterUser(string userId)
    {
        // TODO :: Verify with the Cloud whether the user exists on another Edge and the 
        //          responsibility for updating user state needs to be transferred.

        _users.TryAdd(userId, new(userId));
    }


    /// <inheritdoc/>
    public void ConnectUser(string userId, string connectionId)
    {
        if (!_users.TryGetValue(userId, out var user))
        {
            _logger.LogWarning("Attempted to connect an unregistered user: {userId}", userId);
            return;
        }

        if (!user.IsConnected)
            _connectedUsers.Add(user);
        user.AddConnection(connectionId);

        OnUserConnected?.Invoke(user, connectionId);
    }

    /// <inheritdoc/>
    public void DisconnectUser(string userId, string connectionId)
    {
        if (!_users.TryGetValue(userId, out var user))
        {
            _logger.LogWarning("Attempted to disconnect an unregistered user: {userId}", userId);
            return;
        }

        if (!user.RemoveConnection(connectionId))
            _connectedUsers.Remove(user);

        OnUserDisconnected?.Invoke(user);
    }
}
