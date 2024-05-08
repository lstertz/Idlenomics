namespace Edge.Players;

/// <summary>
/// Manages players, including their registration with this Edge and their current 
/// connection state.
/// </summary>
public interface IPlayerManager
{
    /// <summary>
    /// The players known to this manager that are currently have 
    /// at least one connection with this Edge.
    /// </summary>
    IEnumerable<Player> ConnectedPlayers { get; }

    /// <summary>
    /// The players known to this manager.
    /// </summary>
    IEnumerable<Player> Players { get; }


    /// <summary>
    /// The callback invoked whenever the player manager becomes aware of a player 
    /// connecting to this Edge.
    /// </summary>
    /// <remarks>
    /// Provides the connecting player and the ID of the new connection.
    /// </remarks>
    event Action<Player, string> OnPlayerConnected;

    /// <summary>
    /// The callback invoked whenever the player manager becomes aware of a player 
    /// disconnecting from this Edge.
    /// </summary>
    event Action<Player> OnPlayerDisconnected;

    /// <summary>
    /// The callback invoked whenever the set of players registered by this manager has changed.
    /// </summary>
    /// <remarks>
    /// This occurs when a new player has been registered or a player has been deregistered.
    /// The parameter is the new set of players.
    /// </remarks>
    event Action<IEnumerable<Player>> OnPlayersChanged;


    /// <summary>
    /// Informs the player manager that the identified player has connected to this Edge.
    /// </summary>
    /// <param name="playerId">The ID of the connecting player.</param>
    /// <param name="connectionId">The ID of the player's connection.</param>
    void ConnectPlayer(string playerId, string connectionId);

    /// <summary>
    /// Informs the player manager that the identified player has disconnected from this Edge.
    /// </summary>
    /// <param name="playerId">The ID of the disconnecting player.</param>
    /// <param name="connectionId">The ID of the player's connection.</param>
    void DisconnectPlayer(string playerId, string connectionId);


    /// <summary>
    /// Deregisters the identifier player from the player manager.
    /// </summary>
    /// <param name="playerId">The ID of the player to be deregistered.</param>
    void DeregisterPlayer(string playerId);

    /// <summary>
    /// Registers a new player with the player manager, for the given player ID.
    /// </summary>
    /// <remarks>
    /// Player registrations are maintained, even after a player disconnects, as they define 
    /// which player game state this Edge is responsible for updating.
    /// </remarks>
    /// <param name="playerId">The ID of the new player to be registered.</param>
    void RegisterPlayer(string playerId);
}