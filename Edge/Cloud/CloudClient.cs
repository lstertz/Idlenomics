using Edge.Players;
using Edge.World;
using Microsoft.AspNetCore.SignalR.Client;

namespace Edge.Cloud;

/// <summary>
/// Represents this Edge as a client to the Cloud.
/// </summary>
public partial class CloudClient(ILogger<ClientHub> _logger,
    IPlayerRegistrar _playerRegistrar, IWorldStateManager _worldStateManager) : 
    ICloudClient, IHostedService
{
    private HubConnection? _connection;

    private CancellationToken _connectionCancellationToken;
    private CancellationTokenSource? _connectionCancellationTokenSource;


    /// <inheritdoc/>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7060/edgeHub")
            .Build();

        await _connection.StartAsync(cancellationToken);

        _connectionCancellationTokenSource = new();
        _connectionCancellationToken = _connectionCancellationTokenSource.Token;

        _ = _connection.SendAsync("HandleStreamedPlayerUpdates", StreamPlayerUpdates(),
            _connectionCancellationToken);

        SetUpWorldUpdateStream();
    }

    /// <inheritdoc/>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _connectionCancellationTokenSource?.Cancel();

        if (_connection != null)
            await _connection.StopAsync(cancellationToken);
    }


    partial void SetUpWorldUpdateStream();
}
