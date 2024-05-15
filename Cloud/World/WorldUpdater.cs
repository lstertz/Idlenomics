using System.Collections.Concurrent;
using System.Diagnostics;

namespace Cloud.World;

/// <inheritdoc cref="IWorldUpdater"/>
public class WorldUpdater(ILogger<WorldUpdater> _logger) : BackgroundService, IWorldUpdater
{
    private const int UpdatesPerSecond = 30;

    /// <summary>
    /// The maximum time between updates.
    /// </summary>
    private static readonly TimeSpan UpdateRate =
        TimeSpan.FromSeconds(1.0 / UpdatesPerSecond);

    private readonly Stopwatch _stopwatch = new();
    private readonly ConcurrentQueue<WorldStateDiff> _diffQueue = new();

    /// <inheritdoc/>
    public WorldState CurrentWorldState => _currentWorldState;
    private volatile WorldState _currentWorldState = new();


    /// <inheritdoc/>
    public void QueueDiff(WorldStateDiff diff) =>
        _diffQueue.Enqueue(diff);


    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            Update();

            TimeSpan delayTime = UpdateRate - _stopwatch.Elapsed;
            if (delayTime > TimeSpan.Zero)
                await Task.Delay(delayTime);
            else
                _logger.LogWarning("World update took {overtime} seconds longer than " +
                    "the expected update rate ({rate} seconds).", -delayTime.TotalSeconds,
                    UpdateRate.TotalSeconds);
        }
    }

    private void Update()
    {
        _stopwatch.Restart();

        double finalDiff = 0;
        while (_diffQueue.TryDequeue(out var diff))
            finalDiff += diff.ValueChange;

        WorldState updatedState = new()
        {
            Value = _currentWorldState.Value + finalDiff
        };
        _currentWorldState = updatedState;

        _stopwatch.Stop();
    }
}
