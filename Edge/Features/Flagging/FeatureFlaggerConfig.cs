namespace Edge.Features.Flagging;

/// <summary>
/// Defines the config for the feature flagger.
/// </summary>
public class FeatureFlaggerConfig
{
    /// <summary>
    /// The URL of the feature flag service.
    /// </summary>
    public string Api { get; set; } = string.Empty;

    /// <summary>
    /// The identifier for this app that is connecting to the service.
    /// </summary>
    public string AppName { get; set; } = string.Empty;

    /// <summary>
    /// The API token required to connect to the feature flag service.
    /// </summary>
    public string Authorization { get; set; } = string.Empty;
}
