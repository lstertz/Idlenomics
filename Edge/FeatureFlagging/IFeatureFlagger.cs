namespace Edge.FeatureFlagging;

/// <summary>
/// Integrates with a feature flag service to provide feature flags to 
/// subscribing systems.
/// </summary>
public interface IFeatureFlagger
{
    /// <summary>
    /// Initializes the feature flagger, which connects to the service.
    /// </summary>
    /// <returns>A Task to await the completed connection.</returns>
    Task Initialize();

    /// <summary>
    /// Tears down the feature flagger, which disconnects from the service.
    /// </summary>
    void TearDown();
}
