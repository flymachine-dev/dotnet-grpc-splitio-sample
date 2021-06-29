using FluentValidation;
using Prometheus.DotNetRuntime;

namespace dotnet_grpc_splitio_sample.Monitoring
{
    internal class PrometheusNetValidator : AbstractValidator<PrometheusNet?>
    {
        internal PrometheusNetValidator()
        {
            RuleFor(settings => settings!.Port).GreaterThan(0);
        }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class PrometheusNet
    {
        public int Port { get; set; }

        public bool ExceptionStats { get; set; }
        public bool GcStats { get; set; }
        public bool ThreadPoolStats { get; set; }
        public bool ContentionStats { get; set; }
        public bool JitStats { get; set; }

        public CaptureLevel? ExceptionStatsLevel { get; set; }
        public CaptureLevel? GcStatsLevel { get; set; }
        public CaptureLevel? ThreadPoolLevel { get; set; }
        public CaptureLevel? ContentionStatsLevel { get; set; }

    }
}
