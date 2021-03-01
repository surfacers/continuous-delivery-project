using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Enums;
using Hurace.Core.Models;
using Hurace.Data.Ado.Managers;
using SqlKata.Compilers;
using Xunit;
using System.Transactions;

namespace Hurace.Data.Ado.Test.Managers
{
    public class SkierManagerTest
    {

        [Fact]
        public async Task GetByIdAsyncValidTest()
        {
            // Arrange
            ISkierManager manager = GetSkierManager();

            // Act
            Skier skier = await manager.GetByIdAsync(203);

            // Assert
            Assert.NotNull(skier);
            Assert.Equal("Linda", skier.FirstName);
            Assert.Equal("Sherman", skier.LastName);
        }

        [Fact]
        public async Task GetByIdAsyncInvalidTest()
        {
            // Arrange
            ISkierManager manager = GetSkierManager();

            // Act
            Skier skier = await manager.GetByIdAsync(-1);

            // Assert
            Assert.Null(skier);
        }

        [Fact]
        public async Task GetAllAsyncValidTest()
        {
            // Arrange
            ISkierManager manager = GetSkierManager();

            // Act
            IEnumerable<Skier> skier = await manager.GetAllAsync(Gender.Male);
            
            // Assert
            Assert.NotNull(skier);
            Assert.Equal(100, skier.Count());

            skier = await manager.GetAllAsync(Gender.Female, false);
            Assert.NotNull(skier);
            Assert.Empty(skier);

        }

        [Fact]
        public async Task CreateAsyncValidTest()
        {
            // Arrange
            ISkierManager manager = GetSkierManager();
            Skier newSkier = new Skier
            {
                FirstName = "Leo",
                LastName = "Woody",
                CountryCode = "AUT",
                Gender = Gender.Male,
                BirthDate = new DateTime(1995, 7, 10),
                Image = null,
                IsActive = true,
                IsRemoved = false
            };

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                // Act
                await manager.CreateAsync(newSkier);

                // Assert
                Assert.NotEqual(0, newSkier.Id);
            }
        }

        [Fact]
        public async Task CreateAsyncInvalidTest()
        {
            // Arrange
            ISkierManager manager = GetSkierManager();
            Skier newSkier = new Skier
            {
                Image = null,
                IsActive = true,
                IsRemoved = false
            };

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                // Act
                await Assert.ThrowsAnyAsync<Exception>(() => manager.CreateAsync(newSkier));
            }
        }

        [Fact]
        public async Task UpdateAsyncValidTest()
        {
            // Arrange
            ISkierManager manager = GetSkierManager();
            Skier skierToUpdate = new Skier
            {
                Id = 203,
                FirstName = "Linda",
                LastName = "Lovewood",
                CountryCode = "AUT",
                Gender = Gender.Male,
                BirthDate = new DateTime(1992, 9, 9),
                Image = null,
                IsActive = true,
                IsRemoved = false
            };

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                // Act
                bool updatedSkier = await manager.UpdateAsync(skierToUpdate);
                Skier skier = await manager.GetByIdAsync(203);
                // Assert
                Assert.Equal("Lovewood", skier.LastName);
                Assert.True(updatedSkier);
            }
        }

        [Fact]
        public async Task UpdateAsyncInvalidTest()
        {
            // Arrange
            ISkierManager manager = GetSkierManager();
            Skier skierToUpdate = new Skier { Id = -1 };

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                // Act
                bool updatedSkier = await manager.UpdateAsync(skierToUpdate);
                // Assert
                Assert.False(updatedSkier);
            }
        }

        [Fact]
        public async Task RemoveAsyncValidTest()
        {
            // Arrange
            ISkierManager manager = GetSkierManager();

            // Act
            Skier skier = await manager.GetByIdAsync(203);

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                // Act
                await manager.RemoveAsync(skier.Id);
                skier = await manager.GetByIdAsync(203);
                // Assert
                Assert.True(skier.IsRemoved);
            }
        }

        [Fact]
        public async Task RemoveAsyncInvalidTest()
        {
            // Arrange
            ISkierManager manager = GetSkierManager();

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                // Act
                bool removedSkier = await manager.RemoveAsync(-1);
                // Assert
                Assert.False(removedSkier);
            }
        }

        private ISkierManager GetSkierManager()
        {
            return new SkierManager(
                TestUtil.GetMapper(),
                TestUtil.GetAdoManager(),
                TestUtil.GetCompiler());
        }
    }
}
