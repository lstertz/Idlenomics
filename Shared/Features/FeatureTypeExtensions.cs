namespace Shared.Features;

/// <summary>
/// Extensions for <see cref="FeatureType"/>.
/// </summary>
public static class FeatureTypeExtensions
{
    /// <summary>
    /// Parses the provided string as its matching feature type.
    /// </summary>
    /// <remarks>
    /// Any unmatching string will be defaulted to <see cref="FeatureType.Flag"/>.
    /// </remarks>
    /// <param name="type">The stringified type to be parsed.</param>
    /// <returns>The matching feature type, or a default feature type.</returns>
    public static FeatureType ParseToFeatureType(this string type) =>
        type switch
        {
            "string" => FeatureType.String,
            "json" => FeatureType.Json,
            "csv" => FeatureType.List,
            "number" => FeatureType.Number,
            _ => FeatureType.Flag,
        };
}
