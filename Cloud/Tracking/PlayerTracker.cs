using Cloud.Updating;
using Shared.Players;

namespace Cloud.Tracking;

/// <inheritdoc cref="IPlayerTracker"/>
public class PlayerTracker : IPlayerTracker
{
    public readonly Dictionary<string, PlayerUpdate> _playerData = new();


    /// <inheritdoc/>
    public UpdateDiff UpdatePlayerData(PlayerUpdate update)
    {
        // TEMP :: Track the player upon receiving their first update, this should later 
        //          occur from an explicit message from an Edge when the player is registered.

        UpdateDiff diff;
        if (_playerData.ContainsKey(update.PlayerId))
            diff = new()
            {
                ValueChange = update.SimulationUpdate.Value -
                    _playerData[update.PlayerId].SimulationUpdate.Value
            };
        else
            diff = new()
            {
                ValueChange = update.SimulationUpdate.Value
            };

        _playerData[update.PlayerId] = update;
        return diff;
    }
}
