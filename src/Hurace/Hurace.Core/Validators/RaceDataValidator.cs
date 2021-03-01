using System;
using FluentValidation;
using Hurace.Core.Models;

namespace Hurace.Core.Validators
{
    public class RaceDataValidator : AbstractValidator<RaceData>
    {
        public RaceDataValidator()
        {
            RuleFor(r => r.Id).PrimaryKey();
            RuleFor(r => r.StartListId).ForeignKey();
            RuleFor(r => r.SensorId).GreaterThanOrEqualTo((byte)0);
            RuleFor(r => r.TimeStamp).NotDefaultDateTime();
        }
    }
}
