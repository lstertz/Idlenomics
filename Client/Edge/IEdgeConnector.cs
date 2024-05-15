using Shared.Clients;

namespace Client.Edge
{
    /// <summary>
    /// Connects to the Edge that will manage this Client's state.
    /// </summary>
    public interface IEdgeConnector
    {
        /// <summary>
        /// The callback invoked whenever there is a update for this client to hanble.
        /// </summary>
        event Action<ClientUpdate>? OnClientUpdate;


        /// <summary>
        /// Initiates the connection.
        /// </summary>
        /// <param name="playerId">The ID of the connecting player.</param>
        /// <returns>A Task to await the connection.</returns>
        Task Connect(string? playerId);

        /// <summary>
        /// Stops and disposes of the connection.
        /// </summary>
        /// <returns>A Task to await the disconnection.</returns>
        Task Disconnect();
    }
}
