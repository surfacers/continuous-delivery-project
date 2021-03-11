using System;
using FluentValidation;
using Hurace.Core.Models;

namespace Hurace.Core.Validators
{
    public class SkierValidator : AbstractValidator<Skier>
    {
        public SkierValidator()
        {
            RuleFor(s => s.Id).PrimaryKey();
            RuleFor(s => s.FirstName).NotEmpty().Length(2, 100);
            RuleFor(s => s.LastName).NotEmpty().Length(2, 100);
            RuleFor(s => s.Gender).IsInEnum();
            RuleFor(s => s.CountryCode).NotEmpty().Length(3);
            RuleFor(s => s.BirthDate).NullOrInDateRange(DateTime.Now.AddYears(-80), DateTime.Now.AddYears(-15));
        }
    }
}
