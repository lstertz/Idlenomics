using Shared.Players;

namespace Cloud.Tracking;

/// <inheritdoc cref="IPlayerTracker"/>
public class PlayerTracker : IPlayerTracker
{
    public readonly Dictionary<string, PlayerUpdate> _playerData = new();


    /// <inheritdoc/>
    public void UpdatePlayerData(PlayerUpdate update)
    {
        // TEMP :: Track the player upon receiving their first update, this should later 
        //          occur from an explicit message from an Edge when the player is registered.

        _playerData[update.PlayerId] = update;
    }
}
