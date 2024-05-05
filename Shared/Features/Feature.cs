namespace Shared.Features;

/// <summary>
/// Represents a feature to be enabled on a Cloud, Edge, or Client by a flagger.
/// </summary>
public class Feature
{
    /// <summary>
    /// The name of the feature.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The type of the feature.
    /// </summary>
    public required FeatureType Type { get; init; }

    /// <summary>
    /// The value associated with the feature, if any.
    /// </summary>
    public string? Value { get; init; }
}
