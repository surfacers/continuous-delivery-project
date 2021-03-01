using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;

namespace Hurace.Core.Validators
{
    public static class FluentValidationExtensions
    {
        public static string ErrorCodeFor(this ValidationResult validation, string prop)
            => validation.Errors.FirstOrDefault(e => e.PropertyName == prop)?.ErrorCode;

        public static IRuleBuilderOptions<T, DateTime?> NullOrInDateRange<T>(this IRuleBuilder<T, DateTime?> ruleBuilder, DateTime min, DateTime max)
            => ruleBuilder
                .Must(d => d == null || (d >= min && d <= max))
                .WithErrorCode(ErrorCode.DateTimeNotInRange);

        public static IRuleBuilderOptions<T, DateTime> NotDefaultDateTime<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
            => ruleBuilder
                .NotEqual(DateTime.MinValue)
                .WithErrorCode(ErrorCode.NotDefaultDateTime);

        public static IRuleBuilderOptions<T, int> PrimaryKey<T>(this IRuleBuilder<T, int> ruleBuilder)
            => ruleBuilder
                .GreaterThanOrEqualTo(0)
                .WithErrorCode(ErrorCode.PrimaryKey);

        public static IRuleBuilderOptions<T, int> ForeignKey<T>(this IRuleBuilder<T, int> ruleBuilder)
            => ruleBuilder
                .GreaterThanOrEqualTo(1)
                .WithErrorCode(ErrorCode.ForeignKey);
    }
}
