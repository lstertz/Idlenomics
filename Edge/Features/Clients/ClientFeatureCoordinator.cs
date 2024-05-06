using Edge.Features.Flagging;
using Edge.Users;
using Microsoft.AspNetCore.SignalR;
using Shared.Features;

namespace Edge.Features.Clients;

/// <inheritdoc cref="IClientFeatureCoordinator"/>
public class ClientFeatureCoordinator : IClientFeatureCoordinator
{
    private readonly IFeatureFlagger _featureFlagger;
    private readonly IHubContext<ClientHub> _hubContext;
    private readonly ILogger<ClientFeatureCoordinator> _logger;
    private readonly IUserManager _userManager;


    public ClientFeatureCoordinator(IFeatureFlagger featureFlagger,
        IHubContext<ClientHub> hubContext,
        ILogger<ClientFeatureCoordinator> logger,
        IUserManager userManager)
    {
        _featureFlagger = featureFlagger;
        _hubContext = hubContext;
        _logger = logger;
        _userManager = userManager;

        _featureFlagger.OnFeaturesUpdated += HandleFeaturesUpdate;
        _userManager.OnUserConnected += HandleOnUserConnected;
    }


    private void HandleFeaturesUpdate()
    {
        foreach (var user in _userManager.ConnectedUsers)
        {
            var features = _featureFlagger.GetUserFeatures(user).ToArray();
            _hubContext.Clients.Clients(user.ConnectionIds).SendAsync("OnFeaturesUpdated",
                new OnFeaturesUpdatedNotification()
                {
                    Features = features
                });

            _logger.LogDebug("Sent updated features to {userId}.", user.Id);
        }
    }

    private void HandleOnUserConnected(User user, string connectionId)
    {
        var features = _featureFlagger.GetUserFeatures(user).ToArray();
        _hubContext.Clients.Clients(user.ConnectionIds).SendAsync("OnFeaturesUpdated",
            new OnFeaturesUpdatedNotification()
            { 
                Features = features 
            });

        _logger.LogDebug("Sent updated features to {userId}.", user.Id);
    }
}
