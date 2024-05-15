namespace Shared.World
{
    /// <summary>
    /// Holds the most recent state of the world.
    /// </summary>
    public struct WorldUpdate
    {
        /// <summary>
        /// The time of the update.
        /// </summary>
        public DateTime UpdateTime { get; init; }

        /// <summary>
        /// The current world state value.
        /// </summary>
        public double Value { get; init; }
    }
}
