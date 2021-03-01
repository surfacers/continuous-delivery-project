using FluentValidation;
using Hurace.Core.Models;

namespace Hurace.Core.Validators
{
    public class LocationValidator : AbstractValidator<Location>
    {
        public LocationValidator()
        {
            RuleFor(l => l.Id).PrimaryKey();
            RuleFor(l => l.CountryCode).NotEmpty().Length(3);
            RuleFor(l => l.City).NotEmpty().Length(2, 120);
        }
    }
}
