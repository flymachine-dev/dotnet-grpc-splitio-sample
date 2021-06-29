using System.Collections.Generic;

namespace dotnet_grpc_splitio_sample.FeatureFlags
{
    // A no-op implementation of IFeatureFlags that always returns false
    public class NoopFeatureFlags : IFeatureFlags
    {
        public NoopFeatureFlags()
        {
        }

        public bool IsEnabled(string feature, string key, Dictionary<string, string>? context = null)
        {
            return false;
        }

        public string GetTreatment(string feature, string key, Dictionary<string, string>? context = null)
        {
            return "on";
        }

    }
}
