using Edge.Features.Flagging;
using Edge.Users;

namespace Edge.Features.Clients;

/// <inheritdoc cref="IClientFeatureCoordinator"/>
public class ClientFeatureCoordinator : IClientFeatureCoordinator
{
    private readonly IFeatureFlagger _featureFlagger;
    private readonly ILogger<ClientFeatureCoordinator> _logger;
    private readonly IUserManager _userManager;


    public ClientFeatureCoordinator(IFeatureFlagger featureFlagger,
        ILogger<ClientFeatureCoordinator> logger,
        IUserManager userManager)
    {
        _featureFlagger = featureFlagger;
        _logger = logger;
        _userManager = userManager;

        _featureFlagger.OnFeaturesUpdated += HandleFeaturesUpdate;
        _userManager.OnUserConnected += HandleOnUserConnected;
    }


    private void HandleFeaturesUpdate()
    {
        foreach (var user in _userManager.ConnectedUsers)
        {
            _featureFlagger.GetUserFeatures(user);
            _logger.LogDebug("Sending updated feature information to {userId}.", user.Id);

            // TODO :: Build the feature notification.
        }
    }

    private void HandleOnUserConnected(User user, string connectionId)
    {
        var array = _featureFlagger.GetUserFeatures(user).ToArray();
        // TODO :: Build the feature notification.

        _logger.LogDebug("Sending feature information to {userId}.", user.Id);
    }
}
