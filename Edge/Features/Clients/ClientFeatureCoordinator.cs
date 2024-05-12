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
    private readonly IPlayerManager _playerManager;


    public ClientFeatureCoordinator(IFeatureFlagger featureFlagger,
        IHubContext<ClientHub> hubContext,
        ILogger<ClientFeatureCoordinator> logger,
        IPlayerManager playerManager)
    {
        _featureFlagger = featureFlagger;
        _hubContext = hubContext;
        _logger = logger;
        _playerManager = playerManager;

        _featureFlagger.OnFeaturesUpdated += HandleFeaturesUpdate;
        _playerManager.OnPlayerConnected += HandleOnPlayerConnected;
    }


    private void HandleFeaturesUpdate()
    {
        foreach (var player in _playerManager.ConnectedPlayers)
        {
            var features = _featureFlagger.GetPlayerFeatures(player).ToArray();
            _hubContext.Clients.Clients(player.ConnectionIds).SendAsync("OnFeaturesUpdated",
                new OnFeaturesUpdatedNotification()
                {
                    Features = features
                });

            _logger.LogDebug("Sent updated features to {playerId}.", player.Id);
        }
    }

    private void HandleOnPlayerConnected(Player player, string connectionId)
    {
        var features = _featureFlagger.GetPlayerFeatures(player).ToArray();
        _hubContext.Clients.Clients(player.ConnectionIds).SendAsync("OnFeaturesUpdated",
            new OnFeaturesUpdatedNotification()
            { 
                Features = features 
            });

        _logger.LogDebug("Sent updated features to {playerId}.", player.Id);
    }
}
