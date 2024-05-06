namespace Shared.Features;

/// <summary>
/// A notification to inform a Client of updated features.
/// </summary>
public class OnFeaturesUpdatedNotification
{
    /// <summary>
    /// The updated features.
    /// </summary>
    /// <remarks>
    /// Any feature that is not present here is disabled.
    /// </remarks>
    public required Feature[] Features { get; init; }
}
