using Cloud.World;
using Shared.Players;

namespace Cloud.Tracking;

/// <summary>
/// Tracks the existence of a player and their latest data.
/// </summary>
public interface IPlayerTracker
{
    /// <summary>
    /// Updates the tracked player data, providing a diff of what changed between 
    /// the last known player data and and the updated player data.
    /// </summary>
    /// <param name="update">The latest data for the player.</param>
    /// <return>
    /// A diff for the change in player data.
    /// </return>
    WorldStateDiff UpdatePlayerData(PlayerUpdate update);
}
