using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Extensions;
using Hurace.Core.Logic;
using Hurace.Core.Models;
using Hurace.Data;
using Moq;
using Xunit;

namespace Hurace.Logic.Test.Logics
{
    public class StartListLogicTest
    {
        [Fact]
        public async Task GenerateStartListForRunAsync_ValidTest()
        {
            // Arrange
            var startListLogic = GetStartListLogic();

            // Act
            await startListLogic.UpdateDisqualified(84, isDisqualified: true);
            await startListLogic.UpdateDisqualified(91, isDisqualified: true);
            await startListLogic.UpdateDisqualified(98, isDisqualified: true);
            var result = (await startListLogic.GenerateStartListForRunAsync(22, runNumber: 2)).ToList();

            // Assert
            Assert.Equal(3, result.Count);
        }

        [Theory]
        [InlineData(20, 5)]
        [InlineData(21, 4)]
        [InlineData(22, 6)]
        [InlineData(23, 6)]
        [InlineData(25, 5)]
        [InlineData(-1, 0)]
        public async Task GetByRaceIdAsyncAsync_ValidTest(int raceId, int amount)
        {
            // Arrange
            var startListLogic = GetStartListLogic();

            // Act
            var result = await startListLogic.GetByRaceIdAsync(raceId, runNumber: 1);

            // Assert
            Assert.Equal(amount, result.Count());
        }

        [Theory]
        [InlineData(1, -1)]
        [InlineData(-1, -1)]
        [InlineData(-1, 1)]
        [InlineData(3, 1)]
        [InlineData(3, 2)]
        public async Task GetByRaceIdAsyncAsync_NotFoundTest(int raceId, int runNumber)
        {
            // Arrange
            var startListLogic = GetStartListLogic();

            // Act
            var result = await startListLogic.GetByRaceIdAsync(raceId, runNumber);

            // Assert
            Assert.Empty(result);
        }

        [Theory]
        [InlineData(30, true)]
        [InlineData(36, true)]
        [InlineData(42, true)]
        public async Task UpdateDisqualified_ValidTest(int id, bool isDisqualified)
        {
            // Arrange
            var startListLogic = GetStartListLogic();

            // Act
            var result = await startListLogic.UpdateDisqualified(id, isDisqualified);

            // Assert
            Assert.True(result == isDisqualified);
        }


        private IRaceDataManager GetRaceDataManager(Data data)
        {
            var raceDataManager = new Mock<IRaceDataManager>();
            raceDataManager
                .Setup(m => m.GetByRaceIdAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((int raceId, int runNumber) => {
                    var startListIds = data.StartLists.Where(r => r.RaceId == raceId && r.RunNumber == runNumber).Select(s => s.Id);
                    return data.RaceDatas.Where(r => startListIds.Contains(r.StartListId)).ToList();
                });

            var raceData = new RaceData { Id = 1, SensorId = 2, StartListId = 1, TimeStamp = DateTime.Now };
            raceDataManager.Setup(m => m.CreateAsync(raceData)).Returns(Task.CompletedTask);

            return raceDataManager.Object;
        }

        private IStartListManager GetStartListManager(Data data)
        {
            var startListManager = new Mock<IStartListManager>();

            startListManager
                .Setup(m => m.GetByRaceIdAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((int raceId, int runNumber) => data.StartLists.Where(s => s.RaceId == raceId && s.RunNumber == runNumber));

            startListManager
                .Setup(m => m.SaveAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<StartList>>()))
                .Callback((int raceId, int runNumber, IEnumerable<StartList> saveStartLists) =>
                {
                    saveStartLists.ForEach(s =>
                    {
                        s.Id = data.Id++;
                        s.RunNumber = (byte)runNumber;
                        s.RaceId = raceId;

                        data.StartLists.Add(s);
                    });
                })
                .ReturnsAsync(true);

            startListManager
                .Setup(m => m.UpdateDisqualified(It.IsAny<int>(), It.IsAny<bool>()))
                .Callback((int id, bool isDisqulified) =>
                {
                    var startList = data.StartLists.Where(s => s.Id == id).FirstOrDefault();
                    if (startList == null || startList.Id <= 0) throw new Exception("Startlist not found");

                    startList.IsDisqualified = isDisqulified;
                })
                .ReturnsAsync(true);

            return startListManager.Object;
        }

        private IStartListLogic GetStartListLogic()
        {
            var data = new Data();
            return new StartListLogic(GetStartListManager(data), GetRaceDataManager(data));
        }
    }
}