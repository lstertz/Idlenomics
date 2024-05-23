using Shared.Players;
using System.Diagnostics;

namespace Edge.Cloud
{
    public partial class CloudClient
    {
        private const int UpdatesPerSecond = 30;

        /// <summary>
        /// The maximum time between stream updates.
        /// </summary>
        private static readonly TimeSpan StreamRate =
            TimeSpan.FromSeconds(1.0 / UpdatesPerSecond);

        private readonly Stopwatch _stopwatch = new();


        /// <summary>
        /// Stream player updates to the cloud.
        /// </summary>
        /// <returns>An async enumerable of a player's updated game simulation data.</returns>
        private async IAsyncEnumerable<PlayerUpdate> StreamPlayerUpdates()
        {
            while (!_connectionCancellationToken.IsCancellationRequested)
            {
                _stopwatch.Restart();

                // TODO :: #17 : Manage a local copy of players that is only updated intentionally 
                //          before an update cycle begins.

                var players = _playerRegistrar.RegisteredPlayers;
                while (players.MoveNext())
                {
                    var player = players.Current.Value;

                    yield return new()
                    {
                        PlayerId = player.Id,
                        UpdateTime = player.LastUpdatedOn,
                        Value = player.Businesses.ToArray()[0].Value
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
