namespace Shared.Simulation
{
    public struct SimulationUpdate
    {
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
