using Shared.World;

namespace Edge.World
{
    /// <inheritdoc cref="IWorldStateManager"/>
    public class WorldStateManager : IWorldStateManager
    {
        private double _currentValue;


        /// <inheritdoc/>
        public double GetValue() => _currentValue;

        /// <inheritdoc/>
        public void UpdateState(WorldUpdate update) => 
            Interlocked.Exchange(ref _currentValue, update.Value);
    }
}
