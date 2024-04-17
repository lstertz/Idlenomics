using Microsoft.AspNetCore.SignalR.Client;

namespace Edge;

public class CloudClient(ILogger<ClientHub> _logger) : IClient
{
    private HubConnection _connection;

    public async Task StartAsync()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7060/edgeHub")
            .Build();

        await _connection.StartAsync();
    }
}
