using Shared.Players;

namespace Edge.Cloud
{
    public partial class CloudClient
    {
        /// <summary>
        /// Stream simulation updates to the cloud.
        /// </summary>
        /// <returns>An async enumerable of a player's updated game simulation data.</returns>
        private async IAsyncEnumerable<PlayerUpdate> StreamPlayerUpdates()
        {
            while (!_streamCancellationToken.IsCancellationRequested)
            {
                _stopwatch.Restart();

                foreach (var player in _playerManager.Players)
                {
                    yield return new()
                    {
                        PlayerId = player.Id,
                        SimulationUpdate = new()
                        {
                            UpdateTime = player.LastUpdatedOn,
                            Value = player.Businesses.ToArray()[0].Value
                        }
                    };
                }

                _stopwatch.Stop();

                TimeSpan delayTime = StreamRate - _stopwatch.Elapsed;
                if (delayTime > TimeSpan.Zero)
                    await Task.Delay(delayTime);
                else
                    _logger.LogWarning("Streaming updates to Cloud took {overtime} seconds longer " +
                        "than the stream rate ({rate} seconds).", -delayTime.TotalSeconds,
                        StreamRate.TotalSeconds);
            }
        }
    }
}
