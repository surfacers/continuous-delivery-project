using System;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Logic;
using Hurace.Core.Models;
using Hurace.Data;
using Moq;
using Xunit;

namespace Hurace.Logic.Test.Logics
{
    public class StatisticLogicTest
    {
        [Fact]
        public async Task GetRaceStatisticsTest()
        {
            // Arrange
            var statisticLogic = GetStatisticLogic();

            // Act
            var result = (await statisticLogic.GetRaceStatistics(1, 1, 5)).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result.First().SkierId);
        }

        private IRaceDataManager GetRaceDataManager()
        {
            var raceDataManager = new Mock<IRaceDataManager>();
            raceDataManager.Setup(m => m.GetByRaceIdAsync(1, 1)).ReturnsAsync(new[]
                {
                    new RaceData { Id = 1, SensorId = 1, StartListId = 1, TimeStamp = DateTime.Now },
                    new RaceData { Id = 2, SensorId = 2, StartListId = 1, TimeStamp = DateTime.Now },
                    new RaceData { Id = 3, SensorId = 3, StartListId = 1, TimeStamp = DateTime.Now },
                    new RaceData { Id = 4, SensorId = 4, StartListId = 1, TimeStamp = DateTime.Now },
                    new RaceData { Id = 5, SensorId = 5, StartListId = 1, TimeStamp = DateTime.Now }
                });

            var raceData = new RaceData { Id = 1, SensorId = 2, StartListId = 1, TimeStamp = DateTime.Now };
            raceDataManager.Setup(m => m.CreateAsync(raceData)).Returns(Task.CompletedTask);

            return raceDataManager.Object;
        }

        private IStartListManager GetStartListManager()
        {
            var startListManager = new Mock<IStartListManager>();
            startListManager.Setup(m => m.GetByRaceIdAsync(1, 1)).ReturnsAsync(new[]
                {
                    new StartList { Id = 1, RaceId = 1, SkierId = 1, RunNumber = 1, StartNumber = 1, IsDisqualified = false },
                    new StartList { Id = 2, RaceId = 1, SkierId = 2, RunNumber = 1, StartNumber = 2, IsDisqualified = false },
                    new StartList { Id = 3, RaceId = 1, SkierId = 3, RunNumber = 1, StartNumber = 3, IsDisqualified = false },
                    new StartList { Id = 4, RaceId = 1, SkierId = 4, RunNumber = 1, StartNumber = 4, IsDisqualified = false },
                    new StartList { Id = 5, RaceId = 1, SkierId = 5, RunNumber = 1, StartNumber = 5, IsDisqualified = false }
                });

            return startListManager.Object;
        }

        private IStatisticLogic GetStatisticLogic()
            => new StatisticLogic(GetRaceDataManager(), GetStartListManager());
    }
}

