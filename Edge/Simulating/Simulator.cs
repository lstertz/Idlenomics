using Edge.Users;
using System.Diagnostics;

namespace Edge.Simulating
{
    public class Simulator : BackgroundService
    {
        private const int UpdatesPerSecond = 30;

        private static readonly TimeSpan OneSecond = TimeSpan.FromSeconds(1);

        /// <summary>
        /// The maximum time between simulation updates.
        /// </summary>
        private static readonly TimeSpan SimulationRate = 
            TimeSpan.FromSeconds(1.0 / UpdatesPerSecond);

        private readonly Stopwatch _stopwatch = new();

        private readonly ILogger<Simulator> _logger;
        private readonly IUserManager _userManager;

        private User[] _users = Array.Empty<User>();

        public Simulator(ILogger<Simulator> logger,
            IUserManager userManager)
        {
            _logger = logger;
            _userManager = userManager;

            _userManager.OnUsersChanged += users => _users = users.ToArray();
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

            foreach (var user in _users)
            {
                // This temporarily replaces the more granular calculations specific to 
                // the user's business.

                var now = DateTime.UtcNow;
                var change = (now - user.LastUpdatedOn).TotalSeconds;

                user.LastUpdatedOn = now;

                var businesses = user.Businesses.ToArray();
                foreach (var business in businesses)
                {
                    business.Value += change;
                    _logger.LogDebug("Updated {user}'s business value, {value}", 
                        user.Id, business.Value);
                }
            }

            _stopwatch.Stop();
        }
    }
}
