using Hurace.Core.Models;
using Hurace.Core.Validators;
using Xunit;

namespace Hurace.Core.Test.Validators
{
    public class LocationValidatorTest
    {
        [Fact]
        public void ValidateDefaultLocationTest()
        {
            // Arrange
            var validator = new LocationValidator();
            var location = new Location();

            // Act
            var result = validator.Validate(location);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(2, result.Errors.Count);

            Assert.Equal(ErrorCode.NotEmpty, result.ErrorCodeFor(nameof(Location.CountryCode)));
            Assert.Equal(ErrorCode.NotEmpty, result.ErrorCodeFor(nameof(Location.City)));
        }
    }
}
