using FluentValidation;
using Hurace.Core.Models;

namespace Hurace.Core.Validators
{
    public class RaceValidator : AbstractValidator<Race>
    {
        public RaceValidator()
        {
            RuleFor(r => r.Id).PrimaryKey();
            RuleFor(r => r.Name).NotEmpty().Length(2, 50);
            RuleFor(r => r.Description).MaximumLength(500);
            RuleFor(r => r.SensorAmount).InclusiveBetween(2, 20);
            RuleFor(r => r.RaceDate).NotDefaultDateTime();
            RuleFor(r => r.RaceType).IsInEnum();
            RuleFor(r => r.LocationId).ForeignKey();
            RuleFor(r => r.Gender).IsInEnum();
            RuleFor(r => r.RaceState).IsInEnum();
        }
    }
}
