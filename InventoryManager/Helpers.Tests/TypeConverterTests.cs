using InventoryManager.Data.Entities;
using InventoryManager.DatabaseAccess.Interfaces;
using Moq;
using Xunit;

namespace InventoryManager.Helpers.Tests
{
    public class TypeConverterTests
    {
        public class UintConversionTests
        {
            [Fact]
            public void Convert_PositiveIntegerString_ReturnsSuccess()
            {
                var mockDatabaseController = new Mock<IDatabaseController>();
                var input = "1";
                uint uintExpected = 1;
                object expectedObject = uintExpected;

                var actualResult = TypeConverter.ConvertStringToType(input, typeof(uint), mockDatabaseController.Object, out object actualObject);

                Assert.True(actualResult.IsSuccess);
                Assert.Equal(expectedObject, actualObject);
            }

            [Fact]
            public void Convert_MaxPositiveUintString_ReturnsSuccess()
            {
                var mockDatabaseController = new Mock<IDatabaseController>();
                var input = uint.MaxValue.ToString();
                uint uintExpected = uint.MaxValue;
                object expectedObject = uintExpected;

                var actualResult = TypeConverter.ConvertStringToType(input, typeof(uint), mockDatabaseController.Object, out object actualObject);

                Assert.True(actualResult.IsSuccess);
                Assert.Equal(expectedObject, actualObject);
            }

            [Fact]
            public void Convert_NegativeIntegerString_ReturnsFailure()
            {
                var mockDatabaseController = new Mock<IDatabaseController>();
                var input = "-1";

                var actualResult = TypeConverter.ConvertStringToType(input, typeof(uint), mockDatabaseController.Object, out object actualObject);

                Assert.False(actualResult.IsSuccess);
            }

            [Fact]
            public void Convert_NonIntegerString_ReturnsFailure()
            {
                var mockDatabaseController = new Mock<IDatabaseController>();
                var input = "1 invalid input";

                var actualResult = TypeConverter.ConvertStringToType(input, typeof(uint), mockDatabaseController.Object, out object actualObject);

                Assert.False(actualResult.IsSuccess);
            }
        }

        public class DecimalConversionTests
        {
            [Fact]
            public void Convert_DecimalString_ReturnsSuccess()
            {
                var mockDatabaseController = new Mock<IDatabaseController>();
                var input = "1.01";
                decimal decimalExpected = 1.01m;
                object expectedObject = decimalExpected;

                var actualResult = TypeConverter.ConvertStringToType(input, typeof(decimal), mockDatabaseController.Object, out object actualObject);

                Assert.True(actualResult.IsSuccess);
                Assert.Equal(expectedObject, actualObject);
            }

            [Fact]
            public void Convert_NonDecimalString_ReturnsFailure()
            {
                var mockDatabaseController = new Mock<IDatabaseController>();
                var input = "1 invalid input";

                var actualResult = TypeConverter.ConvertStringToType(input, typeof(decimal), mockDatabaseController.Object, out object actualObject);

                Assert.False(actualResult.IsSuccess);
            }
        }

        public class ProductConversionTests
        {
            [Fact]
            public void Convert_ExistentProductCodeString_ReturnsSuccess()
            {
                var input = "PRODUCT1";
                var mockDatabaseController = new Mock<IDatabaseController>();
                Product product = new Product();
                mockDatabaseController.Setup(x => x.TryReadEntityByCode(input, out product)).Returns(new Result() { IsSuccess = true });
                var expectedType = typeof(Product);

                var actualResult = TypeConverter.ConvertStringToType(input, typeof(Product), mockDatabaseController.Object, out object actualObject);

                Assert.True(actualResult.IsSuccess);
                Assert.IsType(expectedType, actualObject);
            }

            [Fact]
            public void Convert_NonexistentProductCodeString_ReturnsFailure()
            {
                var input = "PRODUCT1";
                var mockDatabaseController = new Mock<IDatabaseController>();
                Product product = new Product();
                mockDatabaseController.Setup(x => x.TryReadEntityByCode(input, out product)).Returns(new Result() { IsSuccess = false });
                var expectedType = typeof(Product);

                var actualResult = TypeConverter.ConvertStringToType(input, typeof(Product), mockDatabaseController.Object, out object actualObject);

                Assert.False(actualResult.IsSuccess);
            }
        }
    }
}
