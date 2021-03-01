using Hurace.Core.Enums;
using Hurace.Core.Logic;
using Hurace.Core.Models;
using Hurace.Data;
using Hurace.Data.Ado;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Hurace.Logic.Test.Logics
{
    public class RaceLogicTest
    {

        [Theory]
        [InlineData(12)]
        public async Task GetAllValidTest(int amount)
        {
            var raceLogic = GetRaceLogic();

            var result = (await raceLogic.GetAllAsync()).ToList();

            Assert.Equal(amount, result.Count);
        }

        [Theory]
        [InlineData(RaceState.NotStarted, 3)]
        [InlineData(RaceState.Running, 4)]
        [InlineData(RaceState.Done, 5)]
        public async Task GetByRaceStateValidTest(RaceState state, int amount)
        {
            var raceLogic = GetRaceLogic();

            var result = (await raceLogic.GetByRaceStateAsync(state)).ToList();

            Assert.Equal(amount, result.Count);
        }

        [Theory]
        [InlineData(0, RaceState.NotStarted)]
        [InlineData(0, RaceState.Running)]
        [InlineData(18, RaceState.NotStarted)]
        [InlineData(24, RaceState.NotStarted)]
        public void CanRemoveValidTest(int raceId, RaceState state)
        {
            var raceLogic = GetRaceLogic();
            var race = new Race { Id = raceId, RaceState = state };

            var result = raceLogic.CanRemove(race);

            Assert.True(result);
        }

        [Theory]
        [InlineData(20, RaceState.Running)]
        [InlineData(25, RaceState.Running)]
        [InlineData(28, RaceState.Done)]
        [InlineData(30, RaceState.Canceled)]
        public void CanRemoveInvalidTest(int raceId, RaceState state)
        {
            var raceLogic = GetRaceLogic();
            var race = new Race { Id = raceId, RaceState = state };

            var result = raceLogic.CanRemove(race);

            Assert.False(result);
        }

        [Theory]
        [InlineData(0, RaceState.NotStarted)]
        [InlineData(0, RaceState.Running)]
        [InlineData(18, RaceState.NotStarted)]
        [InlineData(24, RaceState.NotStarted)]
        public async Task RemoveValidTest(int raceId, RaceState state)
        {
            var raceLogic = GetRaceLogic();
            var race = new Race { Id = raceId, RaceState = state };

            var result = await raceLogic.RemoveAsync(race);

            Assert.True(result);
        }

        [Theory]
        [InlineData(20, RaceState.Running)]
        [InlineData(25, RaceState.Running)]
        [InlineData(28, RaceState.Done)]
        [InlineData(30, RaceState.Canceled)]
        public async Task RemoveInvalidTest(int raceId, RaceState state)
        {
            var raceLogic = GetRaceLogic();
            var race = new Race { Id = raceId, RaceState = state };

            var result = await raceLogic.RemoveAsync(race);

            Assert.False(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(18)]
        [InlineData(19)]
        [InlineData(24)]
        public async Task SaveValidTest(int raceId)
        {
            var raceLogic = GetRaceLogic();
            var race = new Race {
                Id = raceId,
                Gender = Gender.Male,
                LocationId = 1,
                Name = "Race",
                SensorAmount = 5,
                RaceDate = DateTime.Now,
                RaceType = RaceType.Downhill,
                RaceState = RaceState.NotStarted };

            var result = await raceLogic.SaveAsync(race);
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(0,1)]
        [InlineData(18,1)]
        [InlineData(19,2)]
        [InlineData(24,1)]
        public async Task SaveWithStartlistValidTest(int raceId, int runNumber)
        {
            var raceLogic = GetRaceLogic();
            var race = new Race
            {
                Id = raceId,
                Gender = Gender.Male,
                LocationId = 1,
                Name = "Race",
                SensorAmount = 5,
                RaceDate = DateTime.Now,
                RaceType = RaceType.Downhill,
                RaceState = RaceState.NotStarted
            };
            
            var result = await raceLogic.SaveAsync(race, runNumber, new List<StartList>());
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(18)]
        [InlineData(19)]
        [InlineData(20)]
        [InlineData(24)]
        [InlineData(25)]
        public async Task SaveInvalidTest(int raceId)
        {
            var raceLogic = GetRaceLogic();
            var race = new Race { Id = raceId, Name = "Invalid Race" };

            var result = await raceLogic.SaveAsync(race);
            Assert.True(result.IsError);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        [InlineData(-1, -1)]
        public async Task SaveWithStartlistInvalidTest(int raceId, int runNumber)
        {
            var raceLogic = GetRaceLogic();
            var race = new Race { Id = raceId, Name = "Invalid Race" };

            var result = await raceLogic.SaveAsync(race, runNumber, null);
            Assert.True(result.IsError);
        }


        [Theory]
        [InlineData(18)]
        [InlineData(19)]
        [InlineData(24)]
        public async Task StartAsyncValidTest(int raceId)
        {
            var raceLogic = GetRaceLogic();

            var result = await raceLogic.StartAsync(raceId);

            Assert.True(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(22)]
        [InlineData(26)]
        public async Task StartAsyncInvalidTest(int raceId)
        {
            var raceLogic = GetRaceLogic();

            var result = await raceLogic.StartAsync(raceId);

            Assert.False(result);
        }

        private IRaceManager GetRaceManager(Data data)
        {
            var raceManager = new Mock<IRaceManager>();
            raceManager
                .Setup(m => m.GetAllAsync())
                .ReturnsAsync(() => data.Races);

            raceManager
                .Setup(m => m.GetByRaceStateAsync(It.IsAny<RaceState>()))
                .ReturnsAsync((RaceState state) => {
                    return data.Races
                        .Where(m => m.RaceState == state);
                });

            raceManager
                .Setup(m => m.UpdateAsync(It.IsAny<Race>()))
                .ReturnsAsync((Race race) => {
                    if (race.Id == 18 || race.Id == 19 || race.Id == 24)
                        return true;

                    return false;
                });

            raceManager
                .Setup(m => m.UpdateRaceState(It.IsAny<int>(), RaceState.Running))
                .ReturnsAsync((int raceId, RaceState state) => {
                    if(raceId == 18 || raceId == 19 || raceId == 24)
                        return true;

                    return false;
                });


            return raceManager.Object;
        }

        private IStartListManager GetStartListManager(Data data)
        {
            var startListManager = new Mock<IStartListManager>();

            startListManager
                .Setup(m => m.SaveAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<StartList>>()))
                .ReturnsAsync((int raceId, int runNumber, IEnumerable<StartList> startList) => {
                    if (raceId == 0 || raceId == 18 || raceId == 19 || raceId == 24)
                        return true;

                    return false;
                });

            return startListManager.Object;
        }

        private IRaceLogic GetRaceLogic()
        {
            var data = new Data();
            return new RaceLogic(
                GetRaceManager(data), 
                GetStartListManager(data), 
                new Core.Validators.RaceValidator()
            );
        }
    }
}
