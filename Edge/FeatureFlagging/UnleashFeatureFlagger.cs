using Microsoft.Extensions.Options;
using Unleash;
using Unleash.ClientFactory;

namespace Edge.FeatureFlagging;

/// <inheritdoc cref="IFeatureFlagger"/>
/// <param name="_config">The configuration of the feature flagger.</param>
/// <param name="_logger">The logger of the server.</param>
public class UnleashFeatureFlagger(IOptions<FeatureFlaggerConfig> _config, 
    ILogger<UnleashFeatureFlagger> _logger) : IFeatureFlagger
{
    private IUnleash _unleash;

    /// <inheritdoc/>
    public async Task Initialize()
    {
        var config = _config.Value;
        var settings = new UnleashSettings()
        {
            AppName = config.AppName,
            UnleashApi = new Uri(config.Api),
            CustomHttpHeaders = new Dictionary<string, string>()
            {
                { "Authorization", config.Authorization }
            }
        };

        var factory = new UnleashClientFactory();
        _unleash = await factory.CreateClientAsync(settings, synchronousInitialization: true);

        _logger.LogDebug("Received feature flag for TestFlag: {flag}",
            _unleash.IsEnabled("TestFlag"));
    }

    /// <inheritdoc/>
    public void TearDown() => _unleash?.Dispose();
}