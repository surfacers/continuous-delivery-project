using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Models;
using Hurace.Data.Ado.Managers;
using Xunit;

namespace Hurace.Data.Ado.Test.Managers
{
    public class LocationManagerTest
    {
        [Fact]
        public async Task GetAllByIdsValidAsync()
        {
            // Arrange
            ILocationManager manager = GetLocationManager();

            // Act
            IEnumerable<Location> locations = await manager.GetAllByIdsAsync(new[] { 1, 2, 3, 4 });

            // Assert
            Assert.NotNull(locations);
            Assert.Equal(4, locations.Count());
            Assert.NotNull(locations.First(m => m.CountryCode == "FIN"));
            Assert.NotNull(locations.First(m => m.City == "Levi"));

            Assert.NotNull(locations.First(m => m.CountryCode == "CAN"));
            Assert.NotNull(locations.First(m => m.City == "Lake Louise"));

            Assert.NotNull(locations.First(m => m.CountryCode == "USA"));
            Assert.NotNull(locations.First(m => m.City == "Beaver Creek"));

            Assert.NotNull(locations.First(m => m.CountryCode == "FRA"));
            Assert.NotNull(locations.First(m => m.City == "Val d'Isere"));
        }

        [Fact]
        public async Task GetAllByIdsInvalidAsync()
        {
            // Arrange
            ILocationManager manager = GetLocationManager();

            // Act
            IEnumerable<Location> locations = await manager.GetAllByIdsAsync(new[] { -1, -2 });

            // Assert
            Assert.NotNull(locations);
            Assert.Empty(locations);
        }

        private ILocationManager GetLocationManager()
            => new LocationManager(
                TestUtil.GetMapper(),
                TestUtil.GetAdoManager(),
                TestUtil.GetCompiler());
    }
}
