using Microsoft.AspNetCore.SignalR.Client;
using Shared.Features;
using Shared.Simulation;

namespace Client.Edge
{

    /// <inheritdoc cref="IEdgeConnector"/>
    public class EdgeConnector(IConfiguration _configuration) : IEdgeConnector
    {
        /// <inheritdoc/>
        public event Action<SimulationUpdate>? OnSimulationUpdate;

        private HubConnection? _hubConnection;
        private IAsyncEnumerable<SimulationUpdate>? _simulationUpdateStream;


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
            await HandleSimulationUpdates();
        }

        /// <inheritdoc/>
        public async Task Disconnect()
        {
            if (_hubConnection != null)
                await _hubConnection.DisposeAsync();
        }


        private async Task HandleSimulationUpdates()
        {
            if (_simulationUpdateStream == null)
            {
                Console.WriteLine("Simulation update stream failed to initialize.");
                return;
            }

            await foreach (var update in _simulationUpdateStream)
                OnSimulationUpdate?.Invoke(update);
        }

        private void Subscribe()
        {
            _simulationUpdateStream = _hubConnection!
                .StreamAsync<SimulationUpdate>("StreamSimulationUpdates");

            _hubConnection!.On<OnFeaturesUpdatedNotification>("OnFeaturesUpdated", notification =>
            {
                Console.WriteLine("Received updated features: ");

                foreach (var feature in notification.Features)
                    Console.WriteLine($"Feature: {feature.Name}");
            });
        }
    }
}
