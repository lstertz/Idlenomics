using Microsoft.AspNetCore.SignalR.Client;

namespace Edge.Cloud;

/// <summary>
/// Represents this Edge as a client to the Cloud.
/// </summary>
public class CloudClient(ILogger<ClientHub> _logger) : ICloudClient, IHostedService
{
    private HubConnection? _connection;

    /// <inheritdoc/>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7060/edgeHub")
            .Build();

        await _connection.StartAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_connection != null)
            await _connection.StopAsync(cancellationToken);
    }
}
