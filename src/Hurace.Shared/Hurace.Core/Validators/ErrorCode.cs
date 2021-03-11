namespace Hurace.Core.Validators
{
    public static class ErrorCode
    {
        public const string NotEmpty = "NotEmptyValidator";
        public const string Length = "LengthValidator";
        public const string ExactLength = "ExactLengthValidator";
        public const string DateTimeNotInRange = "DateTimeNotInRangeValidator";
        public const string GreaterThanOrEqual = "GreaterThanOrEqualValidator";
        public const string NotDefaultDateTime = "NotDefaultDateTimeValidator";
        public const string PrimaryKey = "PrimaryKeyValidator";
        public const string ForeignKey = "ForeignKeyValidator";
    }
}
