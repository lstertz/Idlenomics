namespace Edge.Users;

/// <summary>
/// Manages users, including their registration with this Edge and their current 
/// connection state.
/// </summary>
public interface IUserManager
{
    /// <summary>
    /// The users known to this manager that are currently have 
    /// at least one connection with this Edge.
    /// </summary>
    IEnumerable<User> ConnectedUsers { get; }

    /// <summary>
    /// The users known to this manager.
    /// </summary>
    IEnumerable<User> Users { get; }


    /// <summary>
    /// The callback invoked whenever the user manager becomes aware of a user 
    /// connecting to this Edge.
    /// </summary>
    /// <remarks>
    /// Provides the connecting user and the ID of the new connection.
    /// </remarks>
    event Action<User, string> OnUserConnected;

    /// <summary>
    /// The callback invoked whenever the user manager becomes aware of a user 
    /// disconnecting from this Edge.
    /// </summary>
    event Action<User> OnUserDisconnected;


    /// <summary>
    /// Informs the user manager that the identified user has connected to this Edge.
    /// </summary>
    /// <param name="userId">The ID of the connecting user.</param>
    /// <param name="connectionId">The ID of the user's connection.</param>
    void ConnectUser(string userId, string connectionId);

    /// <summary>
    /// Informs the user manager that the identified user has disconnected from this Edge.
    /// </summary>
    /// <param name="userId">The ID of the disconnecting user.</param>
    /// <param name="connectionId">The ID of the user's connection.</param>
    void DisconnectUser(string userId, string connectionId);


    /// <summary>
    /// Deregisters the identifier user from the user manager.
    /// </summary>
    /// <param name="userId">The ID of the user to be deregistered.</param>
    void DeregisterUser(string userId);

    /// <summary>
    /// Registers a new user with the user manager, for the given user ID.
    /// </summary>
    /// <remarks>
    /// User registrations are maintained, even after a user disconnects, as they define 
    /// which user game state this Edge is responsible for updating.
    /// </remarks>
    /// <param name="userId">The ID of the new user to be registered.</param>
    void RegisterUser(string userId);
}