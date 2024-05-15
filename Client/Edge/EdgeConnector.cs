using Microsoft.AspNetCore.SignalR.Client;
using Shared.Clients;
using Shared.Features;

namespace Client.Edge
{

    /// <inheritdoc cref="IEdgeConnector"/>
    public class EdgeConnector(IConfiguration _configuration) : IEdgeConnector
    {
        /// <inheritdoc/>
        public event Action<ClientUpdate>? OnClientUpdate;

        private HubConnection? _hubConnection;
        private IAsyncEnumerable<ClientUpdate>? _clientUpdateStream;


        /// <inheritdoc/>
        public async Task Connect(string? playerId = null)
        {
            var edgeUrl = _configuration["OverrideEdgeUrl"];
            if (string.IsNullOrEmpty(playerId))
                playerId = _configuration["DefaultPlayerId"];

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(new Uri($"{edgeUrl}/clientHub?playerId={playerId}"))
                .Build();

            Subscribe();

            try
            {
                await _hubConnection.StartAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // TODO :: Add cancellation token support, to be cancelled on disconnect.
            _ = HandleClientUpdates();
        }

        /// <inheritdoc/>
        public async Task Disconnect()
        {
            if (_hubConnection != null)
                await _hubConnection.DisposeAsync();
        }


        private async Task HandleClientUpdates()
        {
            if (_clientUpdateStream == null)
            {
                Console.WriteLine("Client update stream failed to initialize.");
                return;
            }

            await foreach (var update in _clientUpdateStream)
                OnClientUpdate?.Invoke(update);
        }

        private void Subscribe()
        {
            _clientUpdateStream = _hubConnection!
                .StreamAsync<ClientUpdate>("StreamClientUpdates");

            _hubConnection!.On<OnFeaturesUpdatedNotification>("OnFeaturesUpdated", notification =>
            {
                Console.WriteLine("Received updated features: ");

                foreach (var feature in notification.Features)
                    Console.WriteLine($"Feature: {feature.Name}");
            });
        }
    }
}
