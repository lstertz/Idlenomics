using Edge.Features.Flagging;
using Edge.Players;
using Microsoft.AspNetCore.SignalR;
using Shared.Features;

namespace Edge.Features.Clients;

/// <inheritdoc cref="IClientFeatureCoordinator"/>
public class ClientFeatureCoordinator : IClientFeatureCoordinator
{
    private readonly IFeatureFlagger _featureFlagger;
    private readonly IHubContext<ClientHub> _hubContext;
    private readonly ILogger<ClientFeatureCoordinator> _logger;
    private readonly IPlayerConnectionManager _playerConnectionManager;


    public ClientFeatureCoordinator(IFeatureFlagger featureFlagger,
        IHubContext<ClientHub> hubContext,
        ILogger<ClientFeatureCoordinator> logger,
        IPlayerConnectionManager playerConnectionManager)
    {
        _featureFlagger = featureFlagger;
        _hubContext = hubContext;
        _logger = logger;
        _playerConnectionManager = playerConnectionManager;

        _featureFlagger.OnFeaturesUpdated += HandleFeaturesUpdate;
        _playerConnectionManager.OnPlayerConnected += HandleOnPlayerConnected;
    }


    private void HandleFeaturesUpdate()
    {
        var connections = _playerConnectionManager.PlayerConnections;
        while (connections.MoveNext())
        {
            var connection = connections.Current.Value;
            var features = _featureFlagger.GetPlayerFeatures(connection.Player).ToArray();
            _hubContext.Clients.Client(connection.ConnectionId).SendAsync("OnFeaturesUpdated",
                new OnFeaturesUpdatedNotification()
                {
                    Features = features
                });

            _logger.LogDebug("Sent updated features to {playerId}.", connection.Player.Id);
        }
    }

    private void HandleOnPlayerConnected(PlayerConnection connection)
    {
        var features = _featureFlagger.GetPlayerFeatures(connection.Player).ToArray();
        _hubContext.Clients.Client(connection.ConnectionId).SendAsync("OnFeaturesUpdated",
            new OnFeaturesUpdatedNotification()
            { 
                Features = features 
            });

        _logger.LogDebug("Sent updated features to {playerId}.", connection.Player.Id);
    }
}
