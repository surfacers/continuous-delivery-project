using FluentValidation;
using Hurace.Core.Models;

namespace Hurace.Core.Validators
{
    public class StartListValidator : AbstractValidator<StartList>
    {
        public StartListValidator()
        {
            RuleFor(r => r.Id).PrimaryKey();
            RuleFor(r => r.SkierId).ForeignKey();
            RuleFor(r => r.RaceId).ForeignKey();
            RuleFor(s => s.StartNumber).GreaterThanOrEqualTo(1);
            RuleFor(s => s.RunNumber).GreaterThanOrEqualTo((byte)1);
        }
    }
}
