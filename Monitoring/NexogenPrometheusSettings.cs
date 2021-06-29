using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace dotnet_grpc_splitio_sample.Monitoring
{
    internal class NexogenPrometheusValidator : AbstractValidator<NexogenPrometheus?>
    {
        internal NexogenPrometheusValidator()
        {
            RuleFor(settings => settings!.Port).GreaterThan(0);
        }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class NexogenPrometheus
    {
        public int Port { get; set; }

        public IConfigurationSection ToConfigurationSection(IConfiguration configuration)
        {
            return configuration.GetSection($"{nameof(MonitoringSettings)}:{GetType().Name}");
        }
    }
}
