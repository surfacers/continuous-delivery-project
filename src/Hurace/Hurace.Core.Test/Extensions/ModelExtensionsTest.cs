using Hurace.Core.Enums;
using Hurace.Core.Extensions;
using Hurace.Core.Models;
using Xunit;

namespace Hurace.Core.Test.Extensions
{
    public class ModelExtensionsTest
    {
        [Theory]
        [InlineData(RaceType.Downhill, false)]
        [InlineData(RaceType.SuperG, false)]
        [InlineData(RaceType.Slalom, true)]
        [InlineData(RaceType.GiantSlalom, true)]
        public void HasSecondRunTest(RaceType raceType, bool expectedResult)
        {
            var race = new Race
            {
                RaceType = raceType
            };

            Assert.Equal(expectedResult, race.HasSecondRun());
        }

        [Theory]
        [InlineData("Abdul", "Holden", "Abdul Holden")]
        [InlineData(null, "Holden", "Holden")]
        [InlineData("Abdul", null, "Abdul")]
        [InlineData(null, null, "")]
        public void FullNameTest(string firstName, string lastName, string expectedResult)
        {
            var skier = new Skier
            {
                FirstName = firstName,
                LastName = lastName
            };

            Assert.Equal(expectedResult, skier.FullName());
        }
    }
}
