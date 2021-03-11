using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Enums;
using Hurace.Core.Logic;
using Hurace.Core.Models;
using Hurace.Data;
using Moq;
using Xunit;

namespace Hurace.Logic.Test.Logics
{
    public class SkierLogicTest
    {
        [Theory]
        [InlineData(Gender.Male, true, 5)]
        [InlineData(Gender.Male, false, 2)]
        [InlineData(Gender.Female, true, 7)]
        [InlineData(Gender.Female, false, 0)]
        public async Task GetAllAsync_ValidTest1(Gender gender, bool isActive, int amount)
        {
            // Arrange
            var skierLogic = GetSkierLogic();

            // Act
            var skiers = await skierLogic.GetAllAsync(gender, isActive);

            // Assert
            Assert.Equal(amount, skiers.Count());
        }

        [Theory]
        [InlineData(true, 12)]
        [InlineData(false, 2)]
        public async Task GetAllAsync_ValidTest2(bool isActive, int amount)
        {
            // Arrange
            var skierLogic = GetSkierLogic();

            // Act
            var skiers = await skierLogic.GetAllAsync(isActive);

            // Assert
            Assert.Equal(amount, skiers.Count());
        }

        [Theory]
        [InlineData(9)]
        public async Task GetById_ValidTest(int id)
        {
            // Arrange
            var skierLogic = GetSkierLogic();

            // Act
            var skier = await skierLogic.GetByIdAsync(id);

            // Assert
            Assert.NotNull(skier);
            Assert.Equal(id, skier.Id);
        }


        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task GetById_InvalidTest(int id)
        {
            // Arrange
            var skierLogic = GetSkierLogic();

            // Act
            var skier = await skierLogic.GetByIdAsync(id);

            // Assert
            Assert.Null(skier);
        }

        [Theory]
        [InlineData(0)]
        public async Task Remove_ValidTest(int id)
        {
            // Arrange
            var skierLogic = GetSkierLogic();

            // Act
            var result = await skierLogic.RemoveAsync(id);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(12)]
        [InlineData(15)]
        [InlineData(-23)]
        [InlineData(-345)]
        public async Task Remove_InvalidTest(int id)
        {
            // Arrange
            var skierLogic = GetSkierLogic();

            // Act
            var result = await skierLogic.RemoveAsync(id);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Save_ValidTest()
        {
            // Arrange
            var skierLogic = GetSkierLogic();
            var skier = new Skier
            {
                FirstName = "Hansi",
                LastName = "Hinterseer",
                Gender = Gender.Male,
                CountryCode = "AUT",
                IsActive = false,
                IsRemoved = false,
                BirthDate = new System.DateTime(1970, 5, 14)
            };

            // Act
            var result = await skierLogic.SaveAsync(skier);

            // Assert
            Assert.True(result.IsSuccess);
        }


        [Fact]
        public async Task Save_InvalidTest()
        {
            // Arrange
            var skierLogic = GetSkierLogic();
            var skier = new Skier();

            // Act
            var result = await skierLogic.SaveAsync(skier);

            // Assert
            Assert.True(result.IsError);
        }


        private ISkierManager GetSkierManager(Data data)
        {
            var skierManager = new Mock<ISkierManager>();

            skierManager
                .Setup(m => m.GetAllAsync(It.IsAny<Gender>(), It.IsAny<bool>()))
                .ReturnsAsync((Gender gender, bool isActive) =>
                {
                    return data.Skiers
                        .Where(m => m.Gender == gender && m.IsActive == isActive);
                });

            skierManager
                .Setup(m => m.GetAllAsync(It.IsAny<bool>()))
                .ReturnsAsync((bool isActive) =>
                {
                    return data.Skiers
                        .Where(m => m.IsActive == isActive);
                });

            skierManager
                .Setup(m => m.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => data.Skiers.FirstOrDefault(s => s.Id == id));

            return skierManager.Object;
        }

        private ISkierLogic GetSkierLogic()
        {
            var data = new Data();
            return new SkierLogic(GetSkierManager(data), new Core.Validators.SkierValidator());
        }
    }
}
