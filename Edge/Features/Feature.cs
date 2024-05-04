using Edge.Features.Flagging;

namespace Edge.Features;

/// <summary>
/// Represents a feature, as flagged as enabled by an <see cref="IFeatureFlagger"/>.
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
