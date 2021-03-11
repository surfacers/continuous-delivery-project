using System;
using FluentValidation;
using Hurace.Simulator.Models;

namespace Hurace.Simulator.Validators
{
    public class ClockSettingsValidator : AbstractValidator<ClockSettings>
    {
        public ClockSettingsValidator()
        {
            RuleFor(c => c.MinInterval).InclusiveBetween(ms(100), s(20));
            RuleFor(c => c.MaxInterval)
                .InclusiveBetween(ms(100), s(20))
                .GreaterThanOrEqualTo(c => c.MinInterval);

            RuleFor(c => c.MinRealInterval).InclusiveBetween(s(1), s(100));
            RuleFor(c => c.MaxRealInterval)
                .InclusiveBetween(s(1), s(100))
                .GreaterThanOrEqualTo(c => c.MinRealInterval);

            RuleFor(c => c.SensorAmount).InclusiveBetween(2, 20);

            RuleFor(c => c.TriggerSuccessWeight).GreaterThanOrEqualTo(0);
            RuleFor(c => c.DoNotTriggerWeight).GreaterThanOrEqualTo(0);
            RuleFor(c => c.SetWrongSensorIdWeight).GreaterThanOrEqualTo(0);
            RuleFor(c => c.SetWrongDeltaTimeWeight).GreaterThanOrEqualTo(0);
            RuleFor(c => c.TriggerMultipleWeight).GreaterThanOrEqualTo(0);
        }

        private static int ms(int milliseconds)
            => (int)TimeSpan.FromMilliseconds(milliseconds).TotalMilliseconds;

        private static int s(int seconds)
            => (int)TimeSpan.FromSeconds(seconds).TotalMilliseconds;
    }
}
