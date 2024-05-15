using Shared.World;

namespace Edge.World
{
    /// <summary>
    /// Maintains the current world state and accommodates the retrieval of 
    /// state specific to the requirements of a client.
    /// </summary>
    public interface IWorldStateManager
    {
        /// <summary>
        /// Provides the current world value.
        /// </summary>
        /// <returns>The current world value.</returns>
        double GetValue();

        /// <summary>
        /// Updates the world state based on the provided update.
        /// </summary>
        /// <param name="update">The update to become the current world state.</param>
        void UpdateState(WorldUpdate update);
    }
}
