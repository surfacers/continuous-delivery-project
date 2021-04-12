using System;
using FluentValidation;
using Hurace.Core.Models;

namespace Hurace.Core.Validators
{
    public class SkierValidator : AbstractValidator<Skier>
    {
        public SkierValidator()
        {
            this.RuleFor(s => s.Id).PrimaryKey();
            this.RuleFor(s => s.FirstName).NotEmpty().Length(2, 100);
            this.RuleFor(s => s.LastName).NotEmpty().Length(2, 100);
            this.RuleFor(s => s.Gender).IsInEnum();
            this.RuleFor(s => s.CountryCode).NotEmpty().Length(3);
            this.RuleFor(s => s.BirthDate).NullOrInDateRange(DateTime.Now.AddYears(-80), DateTime.Now.AddYears(-15));
        }
    }
}
