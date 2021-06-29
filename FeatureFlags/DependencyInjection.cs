using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Splitio.Services.Client.Classes;

namespace dotnet_grpc_splitio_sample.FeatureFlags
{
    public static class FeatureFlags
    {
        public static void AddSplitIo(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = new SplitIoSettings();
            configuration.Bind(nameof(SplitIoSettings), settings);
            settings.Validate();

            if (!settings.UseMock)
            {
                var featureFlagService = new SplitFeatureFlags(
                    splitIoApiKey: settings.SplitIoApiKey,
                    config: settings.SplitIoConfigurationOptions ?? new ConfigurationOptions());

                featureFlagService.Initialize().Wait();

                services.AddSingleton<IFeatureFlags>(featureFlagService);
            }
            else
            {
                services.AddSingleton<IFeatureFlags, NoopFeatureFlags>();
            }
        }
    }
}
