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
        /// The time of the update.
        /// </summary>
        public DateTime UpdateTime { get; init; }

        /// <summary>
        /// The player's current simulated value.
        /// </summary>
        public double Value { get; init; }
    }
}
