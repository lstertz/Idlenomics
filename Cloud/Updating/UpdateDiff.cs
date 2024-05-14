namespace Cloud.Updating;

/// <summary>
/// Encapsulates the change from a previous state to the next updated state.
/// </summary>
public class UpdateDiff
{
    /// <summary>
    /// The change in the temporary world value.
    /// </summary>
    public double ValueChange { get; init; }
}
