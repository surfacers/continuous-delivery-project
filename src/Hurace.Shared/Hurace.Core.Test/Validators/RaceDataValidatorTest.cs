using Hurace.Core.Models;
using Hurace.Core.Validators;
using Xunit;

namespace Hurace.Core.Test.Validators
{
    public class RaceDataValidatorTest
    {
        [Fact]
        public void ValidateDefaultRaceDataTest()
        {
            // Arrange
            var validator = new RaceDataValidator();
            var raceData = new RaceData();

            // Act
            var result = validator.Validate(raceData);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(2, result.Errors.Count);

            Assert.Equal(ErrorCode.ForeignKey, result.ErrorCodeFor(nameof(RaceData.StartListId)));
            Assert.Equal(ErrorCode.NotDefaultDateTime, result.ErrorCodeFor(nameof(RaceData.TimeStamp)));
        }
    }
}
