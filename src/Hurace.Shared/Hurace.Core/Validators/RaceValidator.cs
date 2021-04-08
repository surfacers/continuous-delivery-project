using FluentValidation;
using Hurace.Core.Models;

namespace Hurace.Core.Validators
{
    public class RaceValidator : AbstractValidator<Race>
    {
        public RaceValidator()
        {
            this.RuleFor(r => r.Id).PrimaryKey();
            this.RuleFor(r => r.Name).NotEmpty().Length(2, 50);
            this.RuleFor(r => r.Description).MaximumLength(500);
            this.RuleFor(r => r.SensorAmount).InclusiveBetween(2, 20);
            this.RuleFor(r => r.RaceDate).NotDefaultDateTime();
            this.RuleFor(r => r.RaceType).IsInEnum();
            this.RuleFor(r => r.LocationId).ForeignKey();
            this.RuleFor(r => r.Gender).IsInEnum();
            this.RuleFor(r => r.RaceState).IsInEnum();
        }
    }
}
