using Edge.Users;
using Shared.Features;

namespace Edge.Features.Flagging;

/// <summary>
/// Integrates with a feature flag service to provide feature flags to 
/// subscribing systems.
/// </summary>
public interface IFeatureFlagger
{
    /// <summary>
    /// The callback invoked whenever the feature flags are updated.
    /// </summary>
    event Action OnFeaturesUpdated;


    /// <summary>
    /// Provides the enabled features for the specified user.
    /// </summary>
    /// <param name="user">The user whose enabled features are to be provided.</param>
    /// <returns>The specified user's enabled features.</returns>
    IEnumerable<Feature> GetUserFeatures(User user);
}
