using Shared.World;
using System.Runtime.CompilerServices;

namespace Cloud;

public partial class EdgeHub
{
    private const int UpdatesPerSecond = 60;

    /// <summary>
    /// The time between stream updates.
    /// </summary>
    private static readonly TimeSpan StreamRate =
        TimeSpan.FromSeconds(1.0 / UpdatesPerSecond);


    /// <summary>
    /// Streams world updates to the edge.
    /// </summary>
    /// <param name="cancellationToken">The token to cancel updates.</param>
    /// <returns>An async enumerable of the updated game world data.</returns>
    public async IAsyncEnumerable<WorldUpdate> StreamWorldUpdates(
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var currentState = _worldUpdater.CurrentWorldState;

            yield return new()
            {
                Value = currentState.Value,
                UpdateTime = currentState.CreationTime
            };

            await Task.Delay(StreamRate);
        }
    }
}