using Hurace.Core.Logic;
using Hurace.Core.Models;
using Hurace.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using System.Threading.Tasks;
using Hurace.Core.Validators;

namespace Hurace.Logic.Test.Logics
{
    public class RaceDataLogicTest
    {
        [Theory]
        [InlineData(18, 1, 0)]
        [InlineData(19, 1, 0)]
        [InlineData(20, 1, 25)]
        public async Task GetByRaceIdAsync_ValidTest(int raceId, int runNumber, int amount)
        {
            // Arrange
            var raceLogic = GetRaceLogic();

            // Act
            var result = (await raceLogic.GetByRaceIdAsync(raceId, runNumber)).ToList();

            // Assert
            Assert.Equal(amount, result.Count);
        }

        [Fact]
        public async Task CreateAsyncTest()
        {
            // Arrange
            var raceLogic = GetRaceLogic();
            var raceData = new RaceData { Id = 1, SensorId = 2, StartListId = 1, TimeStamp = DateTime.Now };

            // Act
            var result = await raceLogic.CreateAsync(new[] { raceData });

            // Assert
            Assert.True(result);
        }

        private IRaceDataManager GetRaceDataManager()
        {
            var data = new Data();

            var raceDataManager = new Mock<IRaceDataManager>();
            raceDataManager
                .Setup(m => m.GetByRaceIdAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((int raceId, int runNumber) => {
                    return data.StartLists
                        .Where(m => m.RaceId == raceId && m.RunNumber == runNumber)
                        .SelectMany(m => data.RaceDatas.Where(r => r.StartListId == m.Id));
                });

            return raceDataManager.Object;
        }

        private IRaceDataLogic GetRaceLogic()
            => new RaceDataLogic(GetRaceDataManager(), new RaceDataValidator());

    }
}

