using Microsoft.AspNetCore.SignalR.Client;
using Shared.Features;

namespace Client.Edge
{

    /// <inheritdoc cref="IEdgeConnector"/>
    public class EdgeConnector(IConfiguration _configuration) : IEdgeConnector
    {
        /// <inheritdoc/>
        public event Action<double>? OnSimulationUpdate;

        private HubConnection? _hubConnection;
        private IAsyncEnumerable<double>? _simulationUpdateStream;


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

            await ReadSimulationUpdate();
        }

        /// <inheritdoc/>
        public async Task Disconnect()
        {
            if (_hubConnection != null)
                await _hubConnection.DisposeAsync();
        }


        private async Task ReadSimulationUpdate()
        {
            if (_simulationUpdateStream == null)
            {
                Console.WriteLine("Simulation update stream failed to initialize.");
                return;
            }

            // TODO :: Encapsulate the update in a formal data structure.
            await foreach (var update in _simulationUpdateStream)
            {
                OnSimulationUpdate?.Invoke(update);
            }
        }

        private void Subscribe()
        {
            _simulationUpdateStream = _hubConnection!.StreamAsync<double>("OnSimulationUpdate");

            _hubConnection!.On<OnFeaturesUpdatedNotification>("OnFeaturesUpdated", notification =>
            {
                Console.WriteLine("Received updated features: ");

                foreach (var feature in notification.Features)
                    Console.WriteLine($"Feature: {feature.Name}");
            });
        }
    }
}
