namespace Edge.Users;

/// <summary>
/// Represents a user that has most recently connected to this Edge.
/// </summary>
/// <remarks>
/// The most recent Edge that a user connects to is responsible for updating 
/// that user's game state. If the user is currently connected, then the Edge is 
/// also responsible to updating the user's client and handling the user's requests.
/// </remarks>
/// <param name="_id">The ID of the user.</param>
public class User(string _id)
{
    /// <summary>
    /// The IDs of the user's current connections.
    /// </summary>
    public IEnumerable<string> ConnectionIds => _connectionIds;
    private readonly HashSet<string> _connectionIds = new(1);

    /// <summary>
    /// Specifies whether the user is currently connected.
    /// </summary>
    public bool IsConnected => _connectionIds.Count() > 0;

    /// <summary>
    /// Specifies when the user's game state was last updated.
    /// </summary>
    /// <remarks>
    /// This may be expanded upon later, but is intended to be used as a means to determine 
    /// when a user's state was last updated for either this Edge or any Edge that this user 
    /// is transferred to.
    /// </remarks>
    public DateTime LastUpdatedOn { get; }

    /// <summary>
    /// The ID of the user.
    /// </summary>
    public string Id => _id;


    /// <summary>
    /// Adds a connection for this user.
    /// </summary>
    /// <remarks>
    /// Adding the connection signifies that the user has connected to this Edge from a Client.
    /// </remarks>
    /// <param name="connectionId">The ID representing the connection.</param>
    public void AddConnection(string connectionId) => 
        _connectionIds.Add(connectionId);

    /// <summary>
    /// Removes a connection for this user.
    /// </summary>
    /// <remarks>
    /// Removing the connection signifies that the user has disconnected for this Edge, 
    /// at least for a specific Client. The user may still be connected through additional Clients.
    /// </remarks>
    /// <param name="connectionId">The ID representing the connection.</param>
    /// <returns>Whether the user still has an active connection.</returns>
    public bool RemoveConnection(string connectionId)
    {
        _connectionIds.Remove(connectionId);
        return IsConnected;
    }
}