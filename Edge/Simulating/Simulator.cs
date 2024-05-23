using Edge.Players;
using System.Diagnostics;

namespace Edge.Simulating
{
    public class Simulator(ILogger<Simulator> _logger,
        IPlayerRegistrar _playerRegistrar) : BackgroundService
    {
        private const int UpdatesPerSecond = 30;

        /// <summary>
        /// The maximum time between simulation updates.
        /// </summary>
        private static readonly TimeSpan SimulationRate = 
            TimeSpan.FromSeconds(1.0 / UpdatesPerSecond);

        private readonly Stopwatch _stopwatch = new();


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

            var players = _playerRegistrar.RegisteredPlayers;
            while (players.MoveNext())
            {
                var player = players.Current.Value;
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
