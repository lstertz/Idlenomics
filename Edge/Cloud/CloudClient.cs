using Edge.Players;
using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;

namespace Edge.Cloud;

/// <summary>
/// Represents this Edge as a client to the Cloud.
/// </summary>
public class CloudClient(ILogger<ClientHub> _logger,
    IPlayerManager _playerManager) : ICloudClient, IHostedService
{
    private const int UpdatesPerSecond = 30;

    /// <summary>
    /// The time between stream updates.
    /// </summary>
    private static readonly TimeSpan StreamRate =
        TimeSpan.FromSeconds(1.0 / UpdatesPerSecond);


    private HubConnection? _connection;

    private CancellationToken _streamCancellationToken;
    private CancellationTokenSource? _streamCancellationTokenSource;

    private readonly Stopwatch _stopwatch = new();



    /// <inheritdoc/>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7060/edgeHub")
            .Build();

        await _connection.StartAsync(cancellationToken);

        _streamCancellationTokenSource = new();
        _streamCancellationToken = _streamCancellationTokenSource.Token;

        await _connection.SendAsync("ReceiveStreamedSimulationUpdates", StreamSimulationUpdates(), 
            _streamCancellationToken);
    }

    /// <inheritdoc/>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _streamCancellationTokenSource?.Cancel();

        if (_connection != null)
            await _connection.StopAsync(cancellationToken);
    }

    
    private async IAsyncEnumerable<double> StreamSimulationUpdates()
    {
        while (!_streamCancellationToken.IsCancellationRequested)
        {
            _stopwatch.Restart();

            foreach (var player in _playerManager.Players)
            {
                yield return player.Businesses.ToArray()[0].Value;
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
