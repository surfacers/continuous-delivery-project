using System;
using Hurace.Core.Models;
using Hurace.Core.Validators;
using Xunit;

namespace Hurace.Core.Test.Validators
{
    public class SkierValidatorTest
    {
        [Fact]
        public void ValidateDefaultSkierTest()
        {
            // Arrange
            var validators = new SkierValidator();
            var skier = new Skier();

            // Act
            var result = validators.Validate(skier);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(3, result.Errors.Count);

            Assert.Equal(ErrorCode.NotEmpty, result.ErrorCodeFor(nameof(Skier.FirstName)));
            Assert.Equal(ErrorCode.NotEmpty, result.ErrorCodeFor(nameof(Skier.LastName)));
            Assert.Equal(ErrorCode.NotEmpty, result.ErrorCodeFor(nameof(Skier.CountryCode)));
        }

        [Fact]
        public void ValidateBirthdateInvalidSkierTest()
        {
            // Arrange
            var validators = new SkierValidator();
            var skier = GetValidSkier();

            // Act
            skier.BirthDate = DateTime.Now.AddYears(-81);
            var resultBirthDateTooOld = validators.Validate(skier);

            skier.BirthDate = DateTime.Now.AddYears(-1);
            var resultBirthDateTooYoung = validators.Validate(skier);

            // Assert
            Assert.Equal(ErrorCode.DateTimeNotInRange, resultBirthDateTooOld.ErrorCodeFor(nameof(Skier.BirthDate)));
            Assert.Equal(ErrorCode.DateTimeNotInRange, resultBirthDateTooYoung.ErrorCodeFor(nameof(Skier.BirthDate)));
        }

        [Theory]
        [InlineData(null, ErrorCode.NotEmpty)]
        [InlineData("AT", ErrorCode.ExactLength)]
        public void ValidateCountryCodeInvalidSkierTest(string countryCode, string expectedError)
        {
            // Arrange
            var validators = new SkierValidator();
            var skier = GetValidSkier();

            // Act
            skier.CountryCode = countryCode;
            var resultCountryCodeNull = validators.Validate(skier);

            // Assert
            Assert.Equal(expectedError, resultCountryCodeNull.ErrorCodeFor(nameof(Skier.CountryCode)));
        }

        private static Skier GetValidSkier()
            => new Skier
            {
                FirstName = "Manuel",
                LastName = "Neuer",
                CountryCode = "GER"
            };
    }
}
