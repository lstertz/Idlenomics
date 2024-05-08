﻿namespace Client.Edge
{
    /// <summary>
    /// Connects to the Edge that will manage this Client's state.
    /// </summary>
    public interface IEdgeConnector
    {
        /// <summary>
        /// The callback invoked whenever there is a update for the game simulation.
        /// </summary>
        event Action<double>? OnSimulationUpdate;


        /// <summary>
        /// Initiates the connection.
        /// </summary>
        /// <param name="userId">The ID of the connecting user.</param>
        /// <returns>A Task to await the connection.</returns>
        Task Connect(string? userId);

        /// <summary>
        /// Stops and disposes of the connection.
        /// </summary>
        /// <returns>A Task to await the disconnection.</returns>
        Task Disconnect();
    }
}
