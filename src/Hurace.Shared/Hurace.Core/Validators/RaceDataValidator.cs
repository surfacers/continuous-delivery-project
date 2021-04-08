using System;
using FluentValidation;
using Hurace.Core.Models;

namespace Hurace.Core.Validators
{
    public class RaceDataValidator : AbstractValidator<RaceData>
    {
        public RaceDataValidator()
        {
            this.RuleFor(r => r.Id).PrimaryKey();
            this.RuleFor(r => r.StartListId).ForeignKey();
            this.RuleFor(r => r.SensorId).GreaterThanOrEqualTo((byte)0);
            this.RuleFor(r => r.TimeStamp).NotDefaultDateTime();
        }
    }
}
