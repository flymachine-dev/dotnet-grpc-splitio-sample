using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexogen.Libraries.Metrics.Prometheus.Standalone;
using Prometheus;
using Prometheus.DotNetRuntime;

namespace dotnet_grpc_splitio_sample.Monitoring
{
    public static class Monitoring
    {
        public static IServiceCollection AddPrometheusMonitoring(this IServiceCollection services,
            IConfiguration configuration)
        {

            var settings = new MonitoringSettings();
            configuration.Bind(nameof(MonitoringSettings), settings);
            settings.Validate();

            if (settings.NexogenPrometheus != null)
            {
                // Prometheus configuration
                services.AddPrometheusStandalone(settings.NexogenPrometheus.ToConfigurationSection(configuration));
            }

            if (settings.PrometheusNet != null)
            {
                var metricServer = new KestrelMetricServer(port: settings.PrometheusNet.Port);
                metricServer.Start();
                CreateCollector(settings.PrometheusNet);
            }

            return services;
        }

        private static void CreateCollector(PrometheusNet settings)
        {
            var builder = DotNetRuntimeStatsBuilder.Customize();

            if (settings.GcStats)
            {
                builder = settings.GcStatsLevel != null
                    ? builder.WithGcStats(settings.GcStatsLevel.Value)
                    : builder.WithGcStats();
            }

            if (settings.ExceptionStats)
            {
                builder = settings.ExceptionStatsLevel != null
                    ? builder.WithExceptionStats(settings.ExceptionStatsLevel.Value)
                    : builder.WithExceptionStats();
            }

            if (settings.ContentionStats)
            {
                builder = settings.ContentionStatsLevel != null
                    ? builder.WithContentionStats(settings.ContentionStatsLevel.Value)
                    : builder.WithContentionStats();
            }

            if (settings.JitStats)
            {
                builder = builder.WithJitStats();
            }

            if (settings.ThreadPoolStats)
            {
                builder = settings.ThreadPoolLevel != null
                    ? builder.WithThreadPoolStats(settings.ThreadPoolLevel.Value)
                    : builder.WithThreadPoolStats();
            }

            builder.StartCollecting();
        }
    }
}
