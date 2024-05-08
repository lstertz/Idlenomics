namespace Edge.Players;

/// <summary>
/// Represents a player that has most recently connected to this Edge.
/// </summary>
/// <remarks>
/// The most recent Edge that a player connects to is responsible for updating 
/// that player's game state. If the player is currently connected, then the Edge is 
/// also responsible to updating the player's client and handling the player's requests.
/// </remarks>
/// <param name="_id">The ID of the player.</param>
public class Player(string _id)
{
    /// <summary>
    /// The businesses owned by this player.
    /// </summary>
    public IEnumerable<Business> Businesses => _businesses;
    private List<Business> _businesses = new()
    { 
        new Business()  // TEMP :: Create a default initial business for every player.
    };

    /// <summary>
    /// The IDs of the player's current connections.
    /// </summary>
    public IEnumerable<string> ConnectionIds => _connectionIds;
    private readonly HashSet<string> _connectionIds = new(1);

    /// <summary>
    /// Specifies whether the player is currently connected.
    /// </summary>
    public bool IsConnected => _connectionIds.Count() > 0;

    /// <summary>
    /// Specifies when the player's game state was last updated.
    /// </summary>
    /// <remarks>
    /// This may be expanded upon later, but is intended to be used as a means to determine 
    /// when a player's state was last updated for either this Edge or any Edge that this player 
    /// is transferred to.
    /// </remarks>
    public DateTime LastUpdatedOn { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The ID of the player.
    /// </summary>
    public string Id => _id;


    /// <summary>
    /// Adds a connection for this player.
    /// </summary>
    /// <remarks>
    /// Adding the connection signifies that the player has connected to this Edge from a Client.
    /// </remarks>
    /// <param name="connectionId">The ID representing the connection.</param>
    public void AddConnection(string connectionId) => 
        _connectionIds.Add(connectionId);

    /// <summary>
    /// Removes a connection for this player.
    /// </summary>
    /// <remarks>
    /// Removing the connection signifies that the player has disconnected for this Edge, 
    /// at least for a specific Client. The player may still be connected through additional Clients.
    /// </remarks>
    /// <param name="connectionId">The ID representing the connection.</param>
    /// <returns>Whether the player still has an active connection.</returns>
    public bool RemoveConnection(string connectionId)
    {
        _connectionIds.Remove(connectionId);
        return IsConnected;
    }
}