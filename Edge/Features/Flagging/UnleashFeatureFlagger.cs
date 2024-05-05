using Edge.Users;
using Microsoft.Extensions.Options;
using Shared.Features;
using Unleash;
using Unleash.ClientFactory;
using Unleash.Internal;

namespace Edge.Features.Flagging;

/// <inheritdoc cref="IFeatureFlagger"/>
/// <param name="_config">The configuration of the feature flagger.</param>
/// <param name="_logger">The logger of the server.</param>
public class UnleashFeatureFlagger(IOptions<FeatureFlaggerConfig> _config,
    ILogger<UnleashFeatureFlagger> _logger) : IFeatureFlagger, IHostedService
{
    /// <inheritdoc/>
    public event Action? OnFeaturesUpdated;

    private string[] _featureNames = Array.Empty<string>();
    private IUnleash? _unleash;


    /// <inheritdoc/>
    public async Task StartAsync(CancellationToken cancellationToken)
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

        _unleash.ConfigureEvents(cfg =>
        {
            cfg.ImpressionEvent = evt => { _logger.LogDebug($"{evt.FeatureName}: {evt.Enabled}"); };
            cfg.ErrorEvent = evt => { _logger.LogError($"{evt.ErrorType}: {evt.Error}"); };
            cfg.TogglesUpdatedEvent = evt => HandleTogglesUpdatedEvent(evt);
        });
        
        _featureNames = _unleash == null ? Array.Empty<string>() :
            _unleash.FeatureToggles.Where(x => x.Enabled).Select(x => x.Name).ToArray();
    }

    /// <inheritdoc/>
    public async Task StopAsync(CancellationToken cancellationToken) => _unleash?.Dispose();


    /// <inheritdoc/>
    public IEnumerable<Feature> GetUserFeatures(User user)
    {
        if (_unleash == null)
        {
            _logger.LogWarning("Failed to provide user feature flags as Unleash " +
                "has not been initialized.");
            yield break;
        }

        var context = new UnleashContext()
        {
            UserId = user.Id
        };

        foreach (var name in _featureNames)
        {
            var variant = _unleash.GetVariant(name, context, Variant.DISABLED_VARIANT);
            if (!variant.IsEnabled)
            {
                yield return new Feature()
                {
                    Name = name,
                    Type = FeatureType.Flag,
                    Value = null
                };
                continue;
            }

            FeatureType type;
            switch (variant.Payload.Type)
            {
                case "string":
                    type = FeatureType.String;
                    break;
                case "json":
                    type = FeatureType.Json;
                    break;
                case "csv":
                    type = FeatureType.List;
                    break;
                case "number":
                    type = FeatureType.Number;
                    break;
                default:
                    type = FeatureType.Flag;
                    break;
            }

            yield return new Feature()
            {
                Name = name,
                Type = type,
                Value = type == FeatureType.Flag ? null : variant.Payload.Value
            };
        }
    }


    private void HandleTogglesUpdatedEvent(TogglesUpdatedEvent evt)
    {
        _logger.LogDebug($"Toggles updated on: {evt.UpdatedOn}");
        _featureNames = _unleash == null ? Array.Empty<string>() :
            _unleash.FeatureToggles.Where(x => x.Enabled).Select(x => x.Name).ToArray();

        OnFeaturesUpdated?.Invoke();
    }
}