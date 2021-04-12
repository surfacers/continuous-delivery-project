using FluentValidation;
using Hurace.Core.Models;

namespace Hurace.Core.Validators
{
    public class LocationValidator : AbstractValidator<Location>
    {
        public LocationValidator()
        {
            this.RuleFor(l => l.Id).PrimaryKey();
            this.RuleFor(l => l.CountryCode).NotEmpty().Length(3);
            this.RuleFor(l => l.City).NotEmpty().Length(2, 120);
        }
    }
}
