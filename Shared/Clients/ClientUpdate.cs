namespace Shared.Clients
{
    /// <summary>
    /// Provides an update composed specifically for the receiving client, 
    /// from the client's connected edge.
    /// </summary>
    public class ClientUpdate
    {
        /// <summary>
        /// The current value of the client's player.
        /// </summary>
        public double ClientPlayerValue { get; init; }

        /// <summary>
        /// The time of the update.
        /// </summary>
        public DateTime UpdateTime { get; } = DateTime.UtcNow;

        /// <summary>
        /// The current world value.
        /// </summary>
        public double WorldValue { get; init; }
    }
}
