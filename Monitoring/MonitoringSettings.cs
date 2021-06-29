using FluentValidation;

#pragma warning disable CS8618

namespace dotnet_grpc_splitio_sample.Monitoring
{
    public class MonitoringSettingsValidator : AbstractValidator<MonitoringSettings>
    {
        public MonitoringSettingsValidator()
        {
            RuleFor(settings => settings.PrometheusNet).SetValidator(new PrometheusNetValidator())
                .When(s => s.PrometheusNet != null);
            RuleFor(settings => settings.NexogenPrometheus).SetValidator(new NexogenPrometheusValidator())
                .When(s => s.NexogenPrometheus != null);
        }
    }

    public class MonitoringSettings
    {
        public PrometheusNet? PrometheusNet { get; set; }
        public NexogenPrometheus? NexogenPrometheus { get; set; }

        public void Validate()
        {
            var validator = new MonitoringSettingsValidator();
            validator.ValidateAndThrow(this);
        }
    }
}
