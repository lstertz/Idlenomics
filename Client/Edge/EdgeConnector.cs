using Microsoft.AspNetCore.SignalR.Client;
using Shared.Features;

namespace Client.Edge
{

    /// <inheritdoc cref="IEdgeConnector"/>
    public class EdgeConnector(IConfiguration _configuration) : IEdgeConnector
    {
        private HubConnection? _hubConnection;


        /// <inheritdoc/>
        public async Task Connect(string? userId = null)
        {
            var edgeUrl = _configuration["OverrideEdgeUrl"];
            if (string.IsNullOrEmpty(userId))
                userId = _configuration["DefaultUserId"];

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(new Uri($"{edgeUrl}/clientHub?userId={userId}"))
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
        }

        /// <inheritdoc/>
        public async Task Disconnect()
        {
            if (_hubConnection != null)
                await _hubConnection.DisposeAsync();
        }


        private void Subscribe()
        {
            _hubConnection?.On<OnFeaturesUpdatedNotification>("OnFeaturesUpdated", notification =>
            {
                Console.WriteLine("Received updated features: ");

                foreach (var feature in notification.Features)
                    Console.WriteLine($"Feature: {feature.Name}");
            });
        }
    }
}
