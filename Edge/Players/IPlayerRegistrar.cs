namespace Edge.Players;

/// <summary>
/// Manages the registration of players with this Edge.
/// </summary>
public interface IPlayerRegistrar
{
    /// <summary>
    /// The players known by this manager to be registered with this Edge.
    /// </summary>
    IEnumerator<KeyValuePair<string, Player>> RegisteredPlayers { get; }


    /// <summary>
    /// The callback invoked whenever a player has been registered for this manager.
    /// </summary>
    public event Action<Player>? OnPlayerDeregistered;

    /// <summary>
    /// The callback invoked whenever a player has been deregistered from this manager.
    /// </summary>
    public event Action<Player>? OnPlayerRegistered;


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


    /// <summary>
    /// Provides the registered player identified by the given ID.
    /// </summary>
    /// <param name="playerId">The ID of the player to be provided.</param>
    /// <returns>The identified player, or null if there is no such player 
    /// known to this Edge.</returns>
    Player? GetPlayer(string playerId);
}