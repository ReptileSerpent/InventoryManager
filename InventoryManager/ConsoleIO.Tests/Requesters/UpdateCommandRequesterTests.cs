using InventoryManager.ConsoleIO.Interfaces;
using InventoryManager.ConsoleIO.Requesters;
using InventoryManager.Data.Entities;
using InventoryManager.DatabaseAccess.Interfaces;
using InventoryManager.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace InventoryManager.ConsoleIO.Tests.Requesters
{
    public class UpdateCommandRequesterTests
    {
        public class PropertyValuesRequestTests
        {
            [Theory]
            [InlineData("Product 1.1", "800", "CATEGORY2", "Description 2")]
            public void RequestPropertyValues_ValidProduct_ReturnsSuccess(string newName, string newPrice, string newCategoryCode, string newDescription)
            {
                var mockLogger = new Mock<ILogger>();
                var mockConsole = new Mock<IConsole>();
                mockConsole.SetupSequence(x => x.ReadLine())
                    .Returns(newName)
                    .Returns(newPrice)
                    .Returns(newCategoryCode)
                    .Returns(newDescription);
                var mockDatabaseController = new Mock<IDatabaseController>();
                Category originalCategory = new Category() { Id = 1, Code = "CATEGORY1", Name = "Category 1" };
                Category newCategory = new Category() { Id = 2, Code = "CATEGORY2", Name = "Category 2" };
                mockDatabaseController.Setup(x => x.TryReadEntityByCode(originalCategory.Code, out originalCategory)).Returns(new Result() { IsSuccess = true });
                mockDatabaseController.Setup(x => x.TryReadEntityByCode(newCategory.Code, out newCategory)).Returns(new Result() { IsSuccess = true });
                var requester = new UpdateCommandRequester(mockLogger.Object, mockConsole.Object, mockDatabaseController.Object);
                Product product = new Product() { Id = 1, Code = "PRODUCT1", Name = "Product 1", Price = 1000u, Category = originalCategory, Description = "Description 1" };

                var actualResult = requester.RequestPropertyValues(product);

                Assert.True(actualResult.IsSuccess);
                Assert.Equal(newName, product.Name);
                Assert.Equal(newPrice, product.Price.ToString());
                Assert.Equal(newCategoryCode, product.Category.Code);
                Assert.Equal(newDescription, product.Description);
            }

            [Theory]
            [InlineData("Category 1.1")]
            public void RequestPropertyValues_ValidCategory_ReturnsSuccess(string newName)
            {
                var mockLogger = new Mock<ILogger>();
                var mockConsole = new Mock<IConsole>();
                mockConsole.SetupSequence(x => x.ReadLine())
                    .Returns(newName);
                var mockDatabaseController = new Mock<IDatabaseController>();
                var requester = new UpdateCommandRequester(mockLogger.Object, mockConsole.Object, mockDatabaseController.Object);
                Category category = new Category() { Id = 1, Code = "CATEGORY1", Name = "Category 1" };

                var actualResult = requester.RequestPropertyValues(category);

                Assert.True(actualResult.IsSuccess);
                Assert.Equal(newName, category.Name);
            }

            [Theory]
            [InlineData("LOCATION2")]
            public void RequestPropertyValues_ValidWarehouse_ReturnsSuccess(string newLocationCode)
            {
                var mockLogger = new Mock<ILogger>();
                var mockConsole = new Mock<IConsole>();
                mockConsole.SetupSequence(x => x.ReadLine())
                    .Returns(newLocationCode);
                var mockDatabaseController = new Mock<IDatabaseController>();
                Location originalLocation = new Location() { Id = 1, Code = "LOCATION1", Country = "Country", City = "City", Street = "Street" };
                Location newLocation = new Location() { Id = 1, Code = newLocationCode, Country = "Another country", City = "Another city", Street = "Another street" };
                mockDatabaseController.Setup(x => x.TryReadEntityByCode(originalLocation.Code, out originalLocation)).Returns(new Result() { IsSuccess = true });
                mockDatabaseController.Setup(x => x.TryReadEntityByCode(newLocation.Code, out newLocation)).Returns(new Result() { IsSuccess = true });
                var requester = new UpdateCommandRequester(mockLogger.Object, mockConsole.Object, mockDatabaseController.Object);
                Warehouse warehouse = new Warehouse() { Id = 1, Code = "WAREHOUSE1", Location = originalLocation };

                var actualResult = requester.RequestPropertyValues(warehouse);

                Assert.True(actualResult.IsSuccess);
                Assert.Equal(newLocationCode, warehouse.Location.Code);
            }

            [Theory]
            [InlineData("Another country", "Another city", "Another street")]
            public void RequestPropertyValues_ValidLocation_ReturnsSuccess(string newCountry, string newCity, string newStreet)
            {
                var mockLogger = new Mock<ILogger>();
                var mockConsole = new Mock<IConsole>();
                mockConsole.SetupSequence(x => x.ReadLine())
                    .Returns(newCountry)
                    .Returns(newCity)
                    .Returns(newStreet);
                var mockDatabaseController = new Mock<IDatabaseController>();
                var requester = new UpdateCommandRequester(mockLogger.Object, mockConsole.Object, mockDatabaseController.Object);
                var location = new Location() { Id = 1, Code = "LOCATION1", Country = "Country", City = "City", Street = "Street" };

                var actualResult = requester.RequestPropertyValues(location);

                Assert.True(actualResult.IsSuccess);
                Assert.Equal(newCountry, location.Country);
                Assert.Equal(newCity, location.City);
                Assert.Equal(newStreet, location.Street);
            }

            [Theory]
            [InlineData("PRODUCT2", "WAREHOUSE2", "20")]
            public void RequestPropertyValues_ValidInventoryEntry_ReturnsSuccess(string newProductCode, string newWarehouseCode, string newCount)
            {
                var mockLogger = new Mock<ILogger>();
                var mockConsole = new Mock<IConsole>();
                mockConsole.SetupSequence(x => x.ReadLine())
                    .Returns(newProductCode)
                    .Returns(newWarehouseCode)
                    .Returns(newCount);
                var mockDatabaseController = new Mock<IDatabaseController>();
                Category originalCategory = new Category() { Id = 1, Code = "CATEGORY1", Name = "Category 1" };
                Category newCategory = new Category() { Id = 2, Code = "CATEGORY2", Name = "Category 2" };
                Product originalProduct = new Product() { Id = 1, Code = "PRODUCT1", Name = "Product 1", Price = 1000u, Category = originalCategory, Description = "Description 1" };
                Product newProduct = new Product() { Id = 2, Code = newProductCode, Name = "Product 2", Price = 800u, Category = newCategory, Description = "Description 2" };
                Location originalLocation = new Location() { Id = 1, Code = "LOCATION1", Country = "Country", City = "City", Street = "Street" };
                Location newLocation = new Location() { Id = 2, Code = "LOCATION2", Country = "Another country", City = "Another city", Street = "Another street" };
                Warehouse originalWarehouse = new Warehouse() { Id = 1, Code = "WAREHOUSE1", Location = originalLocation };
                Warehouse newWarehouse = new Warehouse() { Id = 2, Code = newWarehouseCode, Location = newLocation };
                mockDatabaseController.Setup(x => x.TryReadEntityByCode(originalProduct.Code, out originalProduct)).Returns(new Result() { IsSuccess = true });
                mockDatabaseController.Setup(x => x.TryReadEntityByCode(newProduct.Code, out newProduct)).Returns(new Result() { IsSuccess = true });
                mockDatabaseController.Setup(x => x.TryReadEntityByCode(originalWarehouse.Code, out originalWarehouse)).Returns(new Result() { IsSuccess = true });
                mockDatabaseController.Setup(x => x.TryReadEntityByCode(newWarehouse.Code, out newWarehouse)).Returns(new Result() { IsSuccess = true });
                var requester = new UpdateCommandRequester(mockLogger.Object, mockConsole.Object, mockDatabaseController.Object);
                var uintExpectedCount = uint.Parse(newCount);
                InventoryEntry inventoryEntry = new InventoryEntry() { Id = 1, Product = originalProduct, Warehouse = originalWarehouse, Count = 50 };

                var actualResult = requester.RequestPropertyValues(inventoryEntry);

                Assert.True(actualResult.IsSuccess);
                Assert.Equal(newProductCode, inventoryEntry.Product.Code);
                Assert.Equal(newWarehouseCode, inventoryEntry.Warehouse.Code);
                Assert.Equal(uintExpectedCount, inventoryEntry.Count);
            }
        }
    }
}