namespace Edge.Players;

/// <summary>
/// Associates a <see cref="Player"/> that has connected to this Edge with its connection ID.
/// </summary>
public class PlayerConnection
{
    /// <summary>
    /// The connection ID of this connected player.
    /// </summary>
    public required string ConnectionId { get; set; }

    /// <summary>
    /// The connected player.
    /// </summary>
    public required Player Player { get; init; }
}
