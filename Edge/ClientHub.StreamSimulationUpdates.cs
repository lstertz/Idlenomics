using System.Runtime.CompilerServices;

namespace Edge;

public partial class ClientHub
{
    /// <summary>
    /// Streams simulation updates to the client.
    /// </summary>
    /// <param name="cancellationToken">The token to cancel updates.</param>
    /// <returns>An async enumerable of the updated game simulation data.</returns>
    public async IAsyncEnumerable<double> StreamSimulationUpdates(
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            yield return 0;

            await Task.Delay(1000);
        }
    }
}
