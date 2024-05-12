﻿using Shared.Simulation;
using System.Runtime.CompilerServices;

namespace Edge;

public partial class ClientHub
{
    private const int UpdatesPerSecond = 30;

    /// <summary>
    /// The time between stream updates.
    /// </summary>
    private static readonly TimeSpan StreamRate =
        TimeSpan.FromSeconds(1.0 / UpdatesPerSecond);


    /// <summary>
    /// Streams simulation updates to the client.
    /// </summary>
    /// <param name="cancellationToken">The token to cancel updates.</param>
    /// <returns>An async enumerable of the updated game simulation data.</returns>
    public async IAsyncEnumerable<SimulationUpdate> StreamSimulationUpdates(
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var player = _playerManager.GetPlayer(PlayerId);
        if (player == null)
        {
            _logger.LogWarning("Failed to provide simulation updates for player, {playerId}, " +
                "as the player is not known to this edge.", PlayerId);
            yield break;
        }

        while (!cancellationToken.IsCancellationRequested)
        {
            yield return new()
            {
                UpdateTime = player.LastUpdatedOn,
                Value = player.Businesses.ToArray()[0].Value
            };

            await Task.Delay(StreamRate);
        }
    }
}