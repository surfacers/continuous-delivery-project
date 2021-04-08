using FluentValidation;
using Hurace.Core.Models;

namespace Hurace.Core.Validators
{
    public class StartListValidator : AbstractValidator<StartList>
    {
        public StartListValidator()
        {
            this.RuleFor(r => r.Id).PrimaryKey();
            this.RuleFor(r => r.SkierId).ForeignKey();
            this.RuleFor(r => r.RaceId).ForeignKey();
            this.RuleFor(s => s.StartNumber).GreaterThanOrEqualTo(1);
            this.RuleFor(s => s.RunNumber).GreaterThanOrEqualTo((byte)1);
        }
    }
}
