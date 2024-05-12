using Shared.Players;

namespace Cloud.Tracking;

/// <summary>
/// Tracks the existence of a player and their latest data.
/// </summary>
public interface IPlayerTracker
{
    /// <summary>
    /// Updates the tracked player data.
    /// </summary>
    /// <param name="update">The latest data for the player.</param>
    void UpdatePlayerData(PlayerUpdate update);
}
