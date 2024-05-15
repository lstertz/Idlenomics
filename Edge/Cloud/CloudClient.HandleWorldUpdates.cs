using Microsoft.AspNetCore.SignalR.Client;
using Shared.World;

namespace Edge.Cloud
{
    public partial class CloudClient
    {
        private IAsyncEnumerable<WorldUpdate>? _worldUpdateStream;


        partial void SetUpWorldUpdateStream()
        {
            _worldUpdateStream = _connection!.StreamAsync<WorldUpdate>("StreamWorldUpdates");

            _ = HandleWorldUpdates();
        }

        private async Task HandleWorldUpdates()
        {
            if (_worldUpdateStream == null)
            {
                Console.WriteLine("World update stream failed to initialize.");
                return;
            }

            await foreach (var update in _worldUpdateStream)
            {
                if (_connectionCancellationToken.IsCancellationRequested)
                    return;

                // TODO :: Handle the update.
            }
        }
    }
}
