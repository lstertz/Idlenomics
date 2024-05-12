using Shared.Simulation;

namespace Shared.Players
{
    /// <summary>
    /// Encapsulates an identified player's updated simulation data.
    /// </summary>
    public class PlayerUpdate
    {
        /// <summary>
        /// The identifier of the player whose simulated data has been updated.
        /// </summary>
        public string PlayerId { get; init; } = string.Empty;

        /// <summary>
        /// The updated simulation data of the player.
        /// </summary>
        public SimulationUpdate SimulationUpdate { get; init; }
    }
}
