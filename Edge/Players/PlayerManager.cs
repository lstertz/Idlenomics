using System.Collections.Concurrent;

namespace Edge.Players;


/// <inheritdoc cref="IPlayerManager"/>
public class PlayerManager(ILogger<PlayerManager> _logger) : 
    IPlayerConnectionManager, IPlayerRegistrar
{
    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<string, PlayerConnection>> PlayerConnections => 
        _playerConnections.GetEnumerator();
    // A mapping of connection ID to a player connection.
    private readonly ConcurrentDictionary<string, PlayerConnection> _playerConnections = new();

    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<string, Player>> RegisteredPlayers => _players.GetEnumerator();
    // A mapping of player ID to a Player.
    private readonly ConcurrentDictionary<string, Player> _players = new();


    /// <inheritdoc/>
    public event Action<PlayerConnection>? OnPlayerConnected;

    /// <inheritdoc/>
    public event Action<PlayerConnection>? OnPlayerDisconnected;


    /// <inheritdoc/>
    public event Action<Player>? OnPlayerDeregistered;

    /// <inheritdoc/>
    public event Action<Player>? OnPlayerRegistered;


    /// <inheritdoc/>
    public void DeregisterPlayer(string playerId)
    {
        if (!_players.TryGetValue(playerId, out var player))
        {
            _logger.LogWarning("Attempted to deregister an unregistered player: " +
                "{playerId}", playerId);
            return;
        }

        foreach (var connectionId in player.ConnectionIds)
            if (_playerConnections.TryRemove(connectionId, out var playerConnection))
                OnPlayerDisconnected?.Invoke(playerConnection);

        if (_players.TryRemove(playerId, out _))
            OnPlayerDeregistered?.Invoke(player);
    }

    /// <inheritdoc/>
    public void RegisterPlayer(string playerId)
    {
        // TODO :: Verify with the Cloud whether the player exists on another Edge and the 
        //          responsibility for updating player state needs to be transferred.

        Player player = new(playerId);
        if (_players.TryAdd(playerId, player))
            OnPlayerRegistered?.Invoke(player);
    }


    /// <inheritdoc/>
    public void ConnectPlayer(string playerId, string connectionId)
    {
        if (!_players.TryGetValue(playerId, out var player))
        {
            _logger.LogWarning("Attempted to connect an unregistered player: " +
                "{playerId}", playerId);
            return;
        }

        PlayerConnection playerConnection = new()
        {
            ConnectionId = connectionId,
            Player = player
        };
        _playerConnections.TryAdd(connectionId, playerConnection);
        player.AddConnection(connectionId);

        OnPlayerConnected?.Invoke(playerConnection);
    }

    /// <inheritdoc/>
    public void DisconnectPlayer(string playerId, string connectionId)
    {
        if (!_players.TryGetValue(playerId, out var player))
        {
            _logger.LogWarning("Attempted to disconnect an unregistered player: " +
                "{playerId}", playerId);
            return;
        }

        player.RemoveConnection(connectionId);
        if (_playerConnections.TryRemove(connectionId, out var playerConnection))
            OnPlayerDisconnected?.Invoke(playerConnection);
    }


    /// <inheritdoc/>
    public Player? GetPlayer(string playerId) =>
        _players.GetValueOrDefault(playerId);

    /// <inheritdoc/>
    public PlayerConnection[] GetPlayerConnections(string playerId)
    {
        if (!_players.TryGetValue(playerId, out var player))
            return Array.Empty<PlayerConnection>();

        List<PlayerConnection> connections = new();
        foreach (var id in player.ConnectionIds)
            if (_playerConnections.TryGetValue(id, out var playerConnection))
                connections.Add(playerConnection);

        return connections.ToArray();
    }

    /// <inheritdoc/>
    public bool IsConnected(string playerId)
    {
        if (!_players.TryGetValue(playerId, out var player))
            return false;

        return player.IsConnected;
    }
}
