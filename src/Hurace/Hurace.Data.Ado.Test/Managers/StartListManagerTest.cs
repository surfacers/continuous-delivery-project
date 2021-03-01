using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Hurace.Core.Models;
using Hurace.Data.Ado.Managers;
using SqlKata.Compilers;
using Xunit;

namespace Hurace.Data.Ado.Test.Managers
{
    public class StartListManagerTest
    {
        [Fact]
        public async Task GetByIdAsyncValidTest()
        {
            // Arrange
            IStartListManager manager = GetStartListManager();

            // Act
            StartList startListEntry = await manager.GetByIdAsync(976);

            // Assert
            Assert.Equal(30, startListEntry.RaceId);
            Assert.False(startListEntry.IsDisqualified);
        }
        [Fact]
        public async Task GetByIdAsyncInvalidTest()
        {
            // Arrange
            IStartListManager manager = GetStartListManager();

            // Act
            StartList startListEntry = await manager.GetByIdAsync(-1);

            // Assert
            Assert.Null(startListEntry);
        }

        [Fact]
        public async Task GetByRaceIdAsyncValidTest()
        {
            // Arrange
            IStartListManager manager = GetStartListManager();

            // Act
            IEnumerable<StartList> startListList = await manager.GetByRaceIdAsync(30, 1);

            // Assert
            Assert.NotNull(startListList);
            Assert.Equal(55, startListList.Count());

            // Act
            startListList = await manager.GetByRaceIdAsync(30, 2);

            // Assert
            Assert.NotNull(startListList);
            Assert.Equal(30, startListList.Count());
        }

        [Fact]
        public async Task GetByRaceIdAsyncInvalidTest()
        {
            // Arrange
            IStartListManager manager = GetStartListManager();

            // Act
            IEnumerable<StartList> startList = await manager.GetByRaceIdAsync(-1, 1);

            // Assert
            Assert.NotNull(startList);
            Assert.Empty(startList);
        }

        [Fact]
        public async Task SaveAsyncValidTest()
        {
            // Arrange
            IStartListManager manager = GetStartListManager();
            int raceId = 18;
            byte runNumber = 1;
            int skierStartIndex = 210;
            int skierAmount = 45;
            int lastSkierId = skierAmount + skierStartIndex;

            IEnumerable<StartList> startList = await manager.GetByRaceIdAsync(raceId, runNumber);
            List<StartList> startListToAdd = new List<StartList>();

            // Assert
            Assert.NotNull(startList);
            Assert.Empty(startList);

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                // Act
                int startNumber = 1;
                for (int i = skierStartIndex; i < lastSkierId; i++)
                {
                    startListToAdd.Add(new StartList
                    {
                        RaceId = raceId,
                        RunNumber = runNumber,
                        SkierId = i,
                        StartNumber = startNumber,
                        IsDisqualified = false
                    });
                    startNumber++;
                }

                bool savedStartList = await manager.SaveAsync(raceId, runNumber, startListToAdd);
                Assert.True(savedStartList);

                IEnumerable<StartList> addedStartList = await manager.GetByRaceIdAsync(raceId, runNumber);
                Assert.NotNull(addedStartList);
                Assert.NotEmpty(addedStartList);
                Assert.Equal(skierAmount, addedStartList.Count());
            }
        }

        [Fact]
        public async Task SaveAsyncInvalidTest()
        {
            // Arrange
            IStartListManager manager = GetStartListManager();
            IEnumerable<StartList> startList = await manager.GetByRaceIdAsync(30, 1);

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                // Act and Assert 
                await Assert.ThrowsAnyAsync<Exception>(() => manager.SaveAsync(-1, -1, null));
                await Assert.ThrowsAnyAsync<Exception>(() => manager.SaveAsync(-1, -1, new[] { new StartList() }));
                await Assert.ThrowsAnyAsync<Exception>(() => manager.SaveAsync(-1, -1, startList));
                await Assert.ThrowsAnyAsync<Exception>(() => manager.SaveAsync(30, -1, startList));
            }
        }

        [Fact]
        public async Task UpdateDisqualifiedValidTest()
        {
            // Arrange
            IStartListManager manager = GetStartListManager();

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                // Act
                bool updatedStartList = await manager.UpdateDisqualified(976, true);
                StartList startListEntry = await manager.GetByIdAsync(976);

                // Assert
                Assert.True(updatedStartList);
                Assert.Equal(30, startListEntry.RaceId);
                Assert.True(startListEntry.IsDisqualified);
            }
        }

        [Fact]
        public async Task UpdateDisqualifiedInvalidTest()
        {
            // Arrange
            IStartListManager manager = GetStartListManager();

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                // Act
                bool updatedStartList = await manager.UpdateDisqualified(-1, true);
                // Assert
                Assert.False(updatedStartList);
            }
        }

        private IStartListManager GetStartListManager()
        {
            return new StartListManager(
                TestUtil.GetMapper(),
                TestUtil.GetAdoManager(),
                new SqlServerCompiler());
        }
    }
}
