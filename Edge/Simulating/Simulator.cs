using Edge.Players;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Edge.Simulating
{
    public class Simulator : BackgroundService
    {
        private const int UpdatesPerSecond = 30;

        /// <summary>
        /// The maximum time between simulation updates.
        /// </summary>
        private static readonly TimeSpan SimulationRate = 
            TimeSpan.FromSeconds(1.0 / UpdatesPerSecond);

        private readonly Stopwatch _stopwatch = new();

        private readonly ILogger<Simulator> _logger;
        private readonly IPlayerRegistrar _playerRegistrar;

        private ConcurrentDictionary<Player, object?> _players = new();

        public Simulator(ILogger<Simulator> logger,
            IPlayerRegistrar playerRegistrar)
        {
            _logger = logger;
            _playerRegistrar = playerRegistrar;

            _playerRegistrar.OnPlayerRegistered += player => _players.TryAdd(player, null);
            _playerRegistrar.OnPlayerDeregistered += player => _players.TryRemove(player, out _);
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Update();

                TimeSpan delayTime = SimulationRate - _stopwatch.Elapsed;
                if (delayTime > TimeSpan.Zero)
                    await Task.Delay(delayTime);
                else
                    _logger.LogWarning("Simulation update took {overtime} seconds longer than " +
                        "the simulation rate ({rate} seconds).", -delayTime.TotalSeconds, 
                        SimulationRate.TotalSeconds);
            }
        }

        private void Update()
        {
            _stopwatch.Restart();

            foreach (var player in _players.Keys)
            {
                // This temporarily replaces the more granular calculations specific to 
                // the player's business.

                var now = DateTime.UtcNow;
                var change = (now - player.LastUpdatedOn).TotalSeconds;

                player.LastUpdatedOn = now;

                var businesses = player.Businesses.ToArray();
                foreach (var business in businesses)
                {
                    business.Value += change;
                }
            }

            _stopwatch.Stop();
        }
    }
}
