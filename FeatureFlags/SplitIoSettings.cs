using FluentValidation;
using Splitio.Services.Client.Classes;

#pragma warning disable CS8618

namespace dotnet_grpc_splitio_sample.FeatureFlags
{
    internal class SplitIoSettingsValidator : AbstractValidator<SplitIoSettings>
    {
        public SplitIoSettingsValidator()
        {
            RuleFor(settings => settings.SplitIoApiKey).NotEmpty();
        }
    }

    public class SplitIoSettings
    {
        public string SplitIoApiKey { get; set; }
        public bool UseMock { get; set; } = false;

        public ConfigurationOptions? SplitIoConfigurationOptions { get; set; }

        public void Validate()
        {
            var validator = new SplitIoSettingsValidator();
            validator.ValidateAndThrow(this);
        }
    }
}
