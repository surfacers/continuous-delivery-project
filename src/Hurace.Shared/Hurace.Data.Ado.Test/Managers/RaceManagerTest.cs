using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Hurace.Core.Enums;
using Hurace.Core.Models;
using Hurace.Data.Ado.Managers;
using Microsoft.Data.SqlClient;
using SqlKata.Compilers;
using Xunit;

namespace Hurace.Data.Ado.Test.Managers
{
    public class RaceManagerTest
    {
        [Fact]
        public async Task GetByIdAsyncValidTest()
        {
            // Arrange
            IRaceManager manager = GetRaceManager();

            // Act
            Race race = await manager.GetByIdAsync(2);

            // Assert
            Assert.NotNull(race);
            Assert.Equal(2, race.Id);
            Assert.Equal("Levi", race.Name);
            Assert.Equal("Night Slalom in Levi", race.Description);
            Assert.Equal(new DateTime(2019, 11, 24, 14, 0, 0), race.RaceDate);
            Assert.Equal(RaceType.Slalom, race.RaceType);
            Assert.Equal(5, race.SensorAmount);
            Assert.Equal(Gender.Male, race.Gender);
            Assert.Equal(1, race.LocationId);
            Assert.Equal(RaceState.NotStarted, race.RaceState);
        }

        [Fact]
        public async Task GetByIdAsyncInvalidTest()
        {
            // Arrange
            IRaceManager manager = GetRaceManager();

            // Act
            Race race = await manager.GetByIdAsync(-1);

            // Assert
            Assert.Null(race);
        }

        [Fact]
        public async Task GetAllAsyncValidTest()
        {
            // Arrange
            IRaceManager manager = GetRaceManager();

            // Act
            IEnumerable<Race> races = await manager.GetAllAsync();

            // Assert
            Assert.NotNull(races);
            Assert.Equal(22, races.Count());
        }

        [Fact]
        public async Task CreateAsyncValidTest()
        {
            // Arrange
            IRaceManager manager = GetRaceManager();
            Race newRace = GetValidRace();

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                // Act
                await manager.CreateAsync(newRace);

                // Assert
                Assert.NotEqual(0, newRace.Id);
            }
        }

        [Fact]
        public async Task CreateAsyncInvalidTest()
        {
            // Arrange
            IRaceManager manager = GetRaceManager();
            Race newRace = new Race
            {
                LocationId = -1, // Invalid Id
            };

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                // Act & Assert
                await Assert.ThrowsAnyAsync<Exception>(() => manager.CreateAsync(newRace));
            }
        }

        [Fact]
        public async Task UpdateAsyncValidTest()
        {
            // Arrange
            IRaceManager manager = GetRaceManager();
            Race race = GetValidRace();

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                // Act
                await manager.CreateAsync(race);
                race.Name = "Updated Name";
                race.RaceState = RaceState.Canceled;

                await manager.UpdateAsync(race);
                race = await manager.GetByIdAsync(race.Id);

                // Assert
                Assert.Equal("Updated Name", race.Name);
                Assert.Equal(RaceState.Canceled, race.RaceState);
            }
        }

        [Fact]
        public async Task UpdateAsyncInvalidTest()
        {
            // Arrange
            IRaceManager manager = GetRaceManager();
            Race race = GetValidRace();

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                // Act
                await manager.CreateAsync(race);
                race.Id = 0;

                bool hasUpdated = await manager.UpdateAsync(race);

                // Assert
                Assert.False(hasUpdated);
            }
        }

        [Fact]
        public async Task RemoveAsyncValidTest()
        {
            // Arrange
            IRaceManager manager = GetRaceManager();
            Race race = GetValidRace();

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                // Act
                await manager.CreateAsync(race);
                bool hasRemoved = await manager.RemoveAsync(race.Id);

                // Assert
                Assert.True(hasRemoved);
            }
        }

        [Fact]
        public async Task RemoveAsyncInvalidTest()
        {
            // Arrange
            IRaceManager manager = GetRaceManager();

            // Act
            bool hasRemoved = await manager.RemoveAsync(-1);

            // Assert
            Assert.False(hasRemoved);
        }

        private Race GetValidRace()
            => new Race
            {
                Name = "TestRace",
                Description = null,
                LocationId = 1,
                RaceDate = DateTime.Now
            };

        private IRaceManager GetRaceManager()
            => new RaceManager(
                TestUtil.GetMapper(),
                TestUtil.GetAdoManager(),
                TestUtil.GetCompiler());
    }
}
