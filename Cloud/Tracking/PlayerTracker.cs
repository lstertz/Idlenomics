using Cloud.World;
using Shared.Players;

namespace Cloud.Tracking;

/// <inheritdoc cref="IPlayerTracker"/>
public class PlayerTracker : IPlayerTracker
{
    public readonly Dictionary<string, PlayerUpdate> _playerData = new();


    /// <inheritdoc/>
    public WorldStateDiff UpdatePlayerData(PlayerUpdate update)
    {
        // TEMP :: Track the player upon receiving their first update, this should later 
        //          occur from an explicit message from an Edge when the player is registered.

        WorldStateDiff diff;
        if (_playerData.ContainsKey(update.PlayerId))
            diff = new()
            {
                ValueChange = update.Value -
                    _playerData[update.PlayerId].Value
            };
        else
            diff = new()
            {
                ValueChange = update.Value
            };

        _playerData[update.PlayerId] = update;
        return diff;
    }
}
