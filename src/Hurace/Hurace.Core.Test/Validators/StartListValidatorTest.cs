using Hurace.Core.Models;
using Hurace.Core.Validators;
using Xunit;

namespace Hurace.Core.Test.Validators
{
    public class StartListValidatorTest
    {
        [Fact]
        public void ValidateDefaultStartListTest()
        {
            // Arrange
            var validators = new StartListValidator();
            var skier = new StartList();

            // Act
            var result = validators.Validate(skier);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(4, result.Errors.Count);

            Assert.Equal(ErrorCode.GreaterThanOrEqual, result.ErrorCodeFor(nameof(StartList.StartNumber)));
            Assert.Equal(ErrorCode.GreaterThanOrEqual, result.ErrorCodeFor(nameof(StartList.RunNumber)));
            Assert.Equal(ErrorCode.ForeignKey, result.ErrorCodeFor(nameof(StartList.SkierId)));
            Assert.Equal(ErrorCode.ForeignKey, result.ErrorCodeFor(nameof(StartList.RaceId)));
        }
    }
}
