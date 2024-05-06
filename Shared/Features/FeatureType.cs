namespace Shared.Features;

/// <summary>
/// The types of features.
/// </summary>
public enum FeatureType
{
    /// <summary>
    /// A boolean (toggle) feature. It is either enabled or it is not, with no associated value.
    /// </summary>
    Flag,
    /// <summary>
    /// A feature with associated JSON.
    /// </summary>
    Json,
    /// <summary>
    /// A feature with an associated number.
    /// </summary>
    Number,
    /// <summary>
    /// A feature with an associated array of strings.
    /// </summary>
    List,
    /// <summary>
    /// A feature with an associated string.
    /// </summary>
    String
}