using InventoryManager.ConsoleIO.Interfaces;
using InventoryManager.ConsoleIO.Requesters;
using InventoryManager.Data.Entities;
using InventoryManager.DatabaseAccess.Interfaces;
using InventoryManager.Helpers;
using Moq;
using Xunit;

namespace InventoryManager.ConsoleIO.Tests.Requesters
{
    public class CreateCommandRequesterTests
    {
        [Theory]
        [InlineData("PRODUCT1", "Product 1", "1000", "CATEGORY1", "Description 1")]
        public void RequestPropertyValues_ValidProduct_ReturnsSuccess(string code, string name, string price, string categoryCode, string description)
        {
            var mockConsole = new Mock<IConsole>();
            mockConsole.SetupSequence(x => x.ReadLine())
                .Returns(code)
                .Returns(name)
                .Returns(price)
                .Returns(categoryCode)
                .Returns(description);
            var mockDatabaseController = new Mock<IDatabaseController>();
            Category category = new Category() { Id = 1, Code = "CATEGORY1", Name = "Category 1" };
            mockDatabaseController.Setup(x => x.TryReadEntityByCode(category.Code, out category)).Returns(new Result() { IsSuccess = true });
            var requester = new CreateCommandRequester(mockConsole.Object, mockDatabaseController.Object);

            var actualResult = requester.RequestPropertyValues(out Product actualProduct);

            Assert.True(actualResult.IsSuccess);
            Assert.Equal(code, actualProduct.Code);
            Assert.Equal(name, actualProduct.Name);
            Assert.Equal(price, actualProduct.Price.ToString());
            Assert.Equal(categoryCode, actualProduct.Category.Code);
            Assert.Equal(description, actualProduct.Description);
        }

        [Theory]
        [InlineData("CATEGORY1", "Category 1")]
        public void RequestPropertyValues_ValidCategory_ReturnsSuccess(string code, string name)
        {
            var mockConsole = new Mock<IConsole>();
            mockConsole.SetupSequence(x => x.ReadLine())
                .Returns(code)
                .Returns(name);
            var mockDatabaseController = new Mock<IDatabaseController>();
            var requester = new CreateCommandRequester(mockConsole.Object, mockDatabaseController.Object);

            var actualResult = requester.RequestPropertyValues(out Category actualCategory);

            Assert.True(actualResult.IsSuccess);
            Assert.Equal(code, actualCategory.Code);
            Assert.Equal(name, actualCategory.Name);
        }

        [Theory]
        [InlineData("WAREHOUSE1", "LOCATION1")]
        public void RequestPropertyValues_ValidWarehouse_ReturnsSuccess(string code, string locationCode)
        {
            var mockConsole = new Mock<IConsole>();
            mockConsole.SetupSequence(x => x.ReadLine())
                .Returns(code)
                .Returns(locationCode);
            var mockDatabaseController = new Mock<IDatabaseController>();
            Location location = new Location() { Id = 1, Code = "LOCATION1", Country = "Country", City = "City", Street = "Street" };
            mockDatabaseController.Setup(x => x.TryReadEntityByCode(location.Code, out location)).Returns(new Result() { IsSuccess = true });
            var requester = new CreateCommandRequester(mockConsole.Object, mockDatabaseController.Object);

            var actualResult = requester.RequestPropertyValues(out Warehouse actualWarehouse);

            Assert.True(actualResult.IsSuccess);
            Assert.Equal(code, actualWarehouse.Code);
            Assert.Equal(locationCode, actualWarehouse.Location.Code);
        }

        [Theory]
        [InlineData("LOCATION1", "Country", "City", "Street")]
        public void RequestPropertyValues_ValidLocation_ReturnsSuccess(string code, string country, string city, string street)
        {
            var mockConsole = new Mock<IConsole>();
            mockConsole.SetupSequence(x => x.ReadLine())
                .Returns(code)
                .Returns(country)
                .Returns(city)
                .Returns(street);
            var mockDatabaseController = new Mock<IDatabaseController>();
            var requester = new CreateCommandRequester(mockConsole.Object, mockDatabaseController.Object);

            var actualResult = requester.RequestPropertyValues(out Location actualLocation);

            Assert.True(actualResult.IsSuccess);
            Assert.Equal(code, actualLocation.Code);
            Assert.Equal(country, actualLocation.Country);
            Assert.Equal(city, actualLocation.City);
            Assert.Equal(street, actualLocation.Street);
        }

        [Theory]
        [InlineData("PRODUCT1", "WAREHOUSE1", "50")]
        public void RequestPropertyValues_ValidInventoryEntry_ReturnsSuccess(string productCode, string warehouseCode, string count)
        {
            var mockConsole = new Mock<IConsole>();
            mockConsole.SetupSequence(x => x.ReadLine())
                .Returns(productCode)
                .Returns(warehouseCode)
                .Returns(count);
            var mockDatabaseController = new Mock<IDatabaseController>();
            Category category = new Category() { Id = 1, Code = "CATEGORY1", Name = "Category 1" };
            Product product = new Product() { Id = 1, Code = "PRODUCT1", Name = "Product 1", Price = 1000u, Category = category, Description = "Description 1" };
            Location location = new Location() { Id = 1, Code = "LOCATION1", Country = "Country", City = "City", Street = "Street" };
            Warehouse warehouse = new Warehouse() { Id = 1, Code = "WAREHOUSE1", Location = location };
            mockDatabaseController.Setup(x => x.TryReadEntityByCode(product.Code, out product)).Returns(new Result() { IsSuccess = true });
            mockDatabaseController.Setup(x => x.TryReadEntityByCode(warehouse.Code, out warehouse)).Returns(new Result() { IsSuccess = true });
            var requester = new CreateCommandRequester(mockConsole.Object, mockDatabaseController.Object);
            var uintExpectedCount = uint.Parse(count);

            var actualResult = requester.RequestPropertyValues(out InventoryEntry actualInventoryEntry);

            Assert.True(actualResult.IsSuccess);
            Assert.Equal(productCode, actualInventoryEntry.Product.Code);
            Assert.Equal(warehouseCode, actualInventoryEntry.Warehouse.Code);
            Assert.Equal(uintExpectedCount, actualInventoryEntry.Count);
        }
    }
}