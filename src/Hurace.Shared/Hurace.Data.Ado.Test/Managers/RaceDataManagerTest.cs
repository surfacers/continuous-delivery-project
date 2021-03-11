using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Models;
using Hurace.Data.Ado.Managers;
using Xunit;

namespace Hurace.Data.Ado.Test.Managers
{
    public class RaceDataManagerTest
    {
        [Fact]
        public async Task GetByRaceIdAsyncValidTest()
        {
            // Arrange
            IRaceDataManager manager = GetRaceDataManager();

            // Act
            IEnumerable<RaceData> data = await manager.GetByRaceIdAsync(29, 1);

            // Assert
            Assert.NotNull(data);
            Assert.Equal(330, data.Count());

            foreach (var item in data)
            {
                Assert.InRange(item.Id, 1, 330);
                Assert.InRange(item.StartListId, 836, 890);
                Assert.InRange(item.SensorId, 1, 6);
                Assert.NotEqual(DateTime.MinValue, item.TimeStamp);
            }
        }

        [Fact]
        public async Task GetByRaceIdAsyncInvalidTest()
        {
            // Arrange
            IRaceDataManager manager = GetRaceDataManager();

            // Act
            IEnumerable<RaceData> data = await manager.GetByRaceIdAsync(-1, -1);

            // Assert
            Assert.NotNull(data);
            Assert.Empty(data);
        }

        private IRaceDataManager GetRaceDataManager()
        {
            return new RaceDataManager(
                TestUtil.GetMapper(),
                TestUtil.GetAdoManager(),
                TestUtil.GetCompiler());
        }
    }
}
