using Edge.Players;
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
        private readonly IPlayerManager _playerManager;

        private Player[] _players = Array.Empty<Player>();

        public Simulator(ILogger<Simulator> logger,
            IPlayerManager playerManager)
        {
            _logger = logger;
            _playerManager = playerManager;

            _playerManager.OnPlayersChanged += players => _players = players.ToArray();
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

            foreach (var player in _players)
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
