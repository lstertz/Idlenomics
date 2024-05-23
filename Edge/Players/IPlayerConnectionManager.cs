namespace Edge.Players;

/// <summary>
/// Manages the connections of players.
/// </summary>
public interface IPlayerConnectionManager
{
    /// <summary>
    /// The players known to this manager that currently have 
    /// at least one connection with this Edge, keyed by their connection ID.
    /// </summary>
    IEnumerator<KeyValuePair<string, PlayerConnection>> PlayerConnections { get; }


    /// <summary>
    /// The callback invoked whenever the player manager becomes aware of a player 
    /// connecting to this Edge.
    /// </summary>
    event Action<PlayerConnection> OnPlayerConnected;

    /// <summary>
    /// The callback invoked whenever the player manager becomes aware of a player 
    /// disconnecting from this Edge.
    /// </summary>
    event Action<PlayerConnection> OnPlayerDisconnected;


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
    /// Provides the connections of the identified player.
    /// </summary>
    /// <param name="playerId">The player whose connections will be provided.</param>
    /// <returns>The connections of the identified player. This may be an empty 
    /// collection if the player is not currently connected.</returns>
    PlayerConnection[] GetPlayerConnections(string playerId);

    /// <summary>
    /// Specifies whether the identified player is currently connected to this Edge.
    /// </summary>
    /// <param name="playerId">The ID of the player to be checked for.</param>
    /// <returns>Whether the identified player is currently connected to this Edge.</returns>
    bool IsConnected(string playerId);
}
