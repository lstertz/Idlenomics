using Edge.Players;
using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;

namespace Edge.Cloud;

/// <summary>
/// Represents this Edge as a client to the Cloud.
/// </summary>
public partial class CloudClient(ILogger<ClientHub> _logger,
    IPlayerManager _playerManager) : ICloudClient, IHostedService
{
    private const int UpdatesPerSecond = 30;

    /// <summary>
    /// The maximum time between stream updates.
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

        await _connection.SendAsync("HandleStreamedPlayerUpdates", StreamPlayerUpdates(), 
            _streamCancellationToken);
    }

    /// <inheritdoc/>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _streamCancellationTokenSource?.Cancel();

        if (_connection != null)
            await _connection.StopAsync(cancellationToken);
    }
}
