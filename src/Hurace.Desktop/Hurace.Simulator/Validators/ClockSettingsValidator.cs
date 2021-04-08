using System;
using FluentValidation;
using Hurace.Simulator.Models;

namespace Hurace.Simulator.Validators
{
    public class ClockSettingsValidator : AbstractValidator<ClockSettings>
    {
        public ClockSettingsValidator()
        {
            this.RuleFor(c => c.MinInterval).InclusiveBetween(ms(100), s(20));
            this.RuleFor(c => c.MaxInterval)
                .InclusiveBetween(ms(100), s(20))
                .GreaterThanOrEqualTo(c => c.MinInterval);

            this.RuleFor(c => c.MinRealInterval).InclusiveBetween(s(1), s(100));
            this.RuleFor(c => c.MaxRealInterval)
                .InclusiveBetween(s(1), s(100))
                .GreaterThanOrEqualTo(c => c.MinRealInterval);

            this.RuleFor(c => c.SensorAmount).InclusiveBetween(2, 20);

            this.RuleFor(c => c.TriggerSuccessWeight).GreaterThanOrEqualTo(0);
            this.RuleFor(c => c.DoNotTriggerWeight).GreaterThanOrEqualTo(0);
            this.RuleFor(c => c.SetWrongSensorIdWeight).GreaterThanOrEqualTo(0);
            this.RuleFor(c => c.SetWrongDeltaTimeWeight).GreaterThanOrEqualTo(0);
            this.RuleFor(c => c.TriggerMultipleWeight).GreaterThanOrEqualTo(0);
        }

        private static int ms(int milliseconds)
            => (int)TimeSpan.FromMilliseconds(milliseconds).TotalMilliseconds;

        private static int s(int seconds)
            => (int)TimeSpan.FromSeconds(seconds).TotalMilliseconds;
    }
}
