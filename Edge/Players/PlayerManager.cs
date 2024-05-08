namespace Edge.Players;

/// <inheritdoc cref="IPlayerManager"/>
public class PlayerManager(ILogger<PlayerManager> _logger) : IPlayerManager
{
    /// <inheritdoc/>
    public IEnumerable<Player> ConnectedPlayers => _connectedPlayers;
    private readonly HashSet<Player> _connectedPlayers = new();

    /// <inheritdoc/>
    public IEnumerable<Player> Players => _players.Values;
    private readonly Dictionary<string, Player> _players = new();

    /// <inheritdoc/>
    public event Action<Player, string>? OnPlayerConnected;

    /// <inheritdoc/>
    public event Action<Player>? OnPlayerDisconnected;

    /// <inheritdoc/>
    public event Action<IEnumerable<Player>>? OnPlayersChanged;


    /// <inheritdoc/>
    public void DeregisterPlayer(string playerId)
    {
        if (!_players.TryGetValue(playerId, out var player))
        {
            _logger.LogWarning("Attempted to deregister an unregistered player: " +
                "{playerId}", playerId);
            return;
        }

        _connectedPlayers.Remove(player);
        if (_players.Remove(playerId))
            OnPlayersChanged?.Invoke(_players.Values);
    }

    /// <inheritdoc/>
    public void RegisterPlayer(string playerId)
    {
        // TODO :: Verify with the Cloud whether the player exists on another Edge and the 
        //          responsibility for updating player state needs to be transferred.

        if (_players.TryAdd(playerId, new(playerId)))
            OnPlayersChanged?.Invoke(_players.Values);
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

        if (!player.IsConnected)
            _connectedPlayers.Add(player);
        player.AddConnection(connectionId);

        OnPlayerConnected?.Invoke(player, connectionId);
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

        if (!player.RemoveConnection(connectionId))
            _connectedPlayers.Remove(player);

        OnPlayerDisconnected?.Invoke(player);
    }
}
