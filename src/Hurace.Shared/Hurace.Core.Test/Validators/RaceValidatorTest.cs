using Hurace.Core.Models;
using Hurace.Core.Validators;
using Xunit;

namespace Hurace.Core.Test.Validators
{
    public class RaceValidatorTest
    {
        [Fact]
        public void ValidateDefaultRaceTest()
        {
            // Arrange
            var validators = new RaceValidator();
            var skier = new Race();

            // Act
            var result = validators.Validate(skier);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(2, result.Errors.Count);

            Assert.Equal(ErrorCode.NotEmpty, result.ErrorCodeFor(nameof(Race.Name)));
            Assert.Equal(ErrorCode.ForeignKey, result.ErrorCodeFor(nameof(Race.LocationId)));
        }
    }
}
