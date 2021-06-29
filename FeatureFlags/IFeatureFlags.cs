using System.Collections.Generic;

namespace dotnet_grpc_splitio_sample.FeatureFlags
{
    public interface IFeatureFlags
    {
        public bool IsEnabled(string feature, string key, Dictionary<string, string>? context);

        public string GetTreatment(string feature, string key, Dictionary<string, string>? context);

    }
}
