using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Splitio.Services.Client.Classes;
using Splitio.Services.Client.Interfaces;

namespace dotnet_grpc_splitio_sample.FeatureFlags
{
    public class ClientNotInitializedError : Exception
    {
    }

    public class SplitFeatureFlags : IFeatureFlags
    {
        public bool IsInitialized { get; private set; }
        private const string SplitOn = "on";
        private const int SplitInitializationWait = 10000;
        private readonly ISplitClient _sdk;

        public SplitFeatureFlags(string splitIoApiKey, ConfigurationOptions config)
        {
            var factory = new SplitFactory(apiKey: splitIoApiKey, options: config);
            _sdk = factory.Client();
        }

        public Task Initialize()
        {
            return Task.Run(
                    () => _sdk.BlockUntilReady(SplitInitializationWait))
                .ContinueWith((antecedent) => IsInitialized = antecedent.IsCompletedSuccessfully);
        }

        public bool IsEnabled(string feature, string key, Dictionary<string, string>? context = null)
        {
            return IsOnFromSplitSdk(feature, key, context);
        }

        public string GetTreatment(string feature, string key, Dictionary<string, string>? context = null)
        {
            return GetSplitSdkTreatment(feature, key, context);
        }

        private string GetSplitSdkTreatment(string feature, string key, Dictionary<string, string>? context)
        {
            if (!IsInitialized)
            {
                throw new ClientNotInitializedError();
            }

            string treatment;

            if (context != null)
            {
                var splitResult = _sdk.GetTreatmentWithConfig(key, feature,
                    new Dictionary<string, object>(MakeDictionaryGeneric(context!)));
                treatment = splitResult.Treatment;
            }
            else
            {
                treatment = _sdk.GetTreatment(key, feature);
            }

            return treatment;
        }

        private bool IsOnFromSplitSdk(string feature, string key, Dictionary<string, string>? context)
        {
            return GetSplitSdkTreatment(feature, key, context) == SplitOn;
        }

        private Dictionary<string, object> MakeDictionaryGeneric(Dictionary<string, string> dict)
        {
            return dict.ToDictionary<KeyValuePair<string, string>, string, object>(
                entry => entry.Key,
                entry => entry.Value);
        }
    }
}
