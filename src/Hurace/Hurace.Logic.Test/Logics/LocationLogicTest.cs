using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Logic;
using Hurace.Core.Models;
using Hurace.Core.Validators;
using Hurace.Data;
using Moq;
using Xunit;

namespace Hurace.Logic.Test.Logics
{
    public class LocationLogicTest
    {
        [Fact]
        public async Task GetAllAsync_ValidTest()
        {
            // Arrange
            var locationLogic = GetLocationLogic();

            // Act
            var result = (await locationLogic.GetAllAsync()).ToList();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal("AUT", result.First().CountryCode);
            Assert.Equal("Hagenberg", result.First().City);
        }

        private ILocationManager GetLocationManager()
        {
            var data = new Data();

            var locationManager = new Mock<ILocationManager>();
            locationManager.Setup(m => m.GetAllAsync()).ReturnsAsync(data.Locations);

            return locationManager.Object;
        }

        private ILocationLogic GetLocationLogic()
            => new LocationLogic(GetLocationManager());
    }
}
