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

                var actualResult = TypeConverter.TryConvertStringToType(input, typeof(uint), mockDatabaseController.Object, out object actualObject);

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

                var actualResult = TypeConverter.TryConvertStringToType(input, typeof(uint), mockDatabaseController.Object, out object actualObject);

                Assert.True(actualResult.IsSuccess);
                Assert.Equal(expectedObject, actualObject);
            }

            [Fact]
            public void Convert_NegativeIntegerString_ReturnsFailure()
            {
                var mockDatabaseController = new Mock<IDatabaseController>();
                var input = "-1";

                var actualResult = TypeConverter.TryConvertStringToType(input, typeof(uint), mockDatabaseController.Object, out object actualObject);

                Assert.False(actualResult.IsSuccess);
            }

            [Fact]
            public void Convert_NonIntegerString_ReturnsFailure()
            {
                var mockDatabaseController = new Mock<IDatabaseController>();
                var input = "1 invalid input";

                var actualResult = TypeConverter.TryConvertStringToType(input, typeof(uint), mockDatabaseController.Object, out object actualObject);

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

                var actualResult = TypeConverter.TryConvertStringToType(input, typeof(decimal), mockDatabaseController.Object, out object actualObject);

                Assert.True(actualResult.IsSuccess);
                Assert.Equal(expectedObject, actualObject);
            }

            [Fact]
            public void Convert_NonDecimalString_ReturnsFailure()
            {
                var mockDatabaseController = new Mock<IDatabaseController>();
                var input = "1 invalid input";

                var actualResult = TypeConverter.TryConvertStringToType(input, typeof(decimal), mockDatabaseController.Object, out object actualObject);

                Assert.False(actualResult.IsSuccess);
            }
        }

        public class EntityWithCodeConversionTests
        {
            [Theory]
            [MemberData(nameof(TestData))]
            public void Convert_CodeStringOfExistentEntityWithCode_ReturnsSuccess<T>(T value) where T : class, Data.Interfaces.IEntityWithCode, new()
            {
                var mockDatabaseController = new Mock<IDatabaseController>();
                var input = "ENTITY_1";
                var entity = value;
                mockDatabaseController.Setup(x => x.TryReadEntityByCode<T>(input, out entity)).Returns(new Result() { IsSuccess = true });
                var expectedType = typeof(T);

                var actualResult = TypeConverter.TryConvertStringToType(input, typeof(T), mockDatabaseController.Object, out object actualObject);

                Assert.True(actualResult.IsSuccess);
                Assert.IsType(expectedType, actualObject);
            }

            [Theory]
            [MemberData(nameof(TestData))]
            public void Convert_CodeStringOfNonexistentEntityWithCode_ReturnsFailure<T>(T value) where T : class, Data.Interfaces.IEntityWithCode, new()
            {
                var mockDatabaseController = new Mock<IDatabaseController>();
                var input = "ENTITY_1";
                var entity = value;
                mockDatabaseController.Setup(x => x.TryReadEntityByCode<T>(input, out entity)).Returns(new Result() { IsSuccess = false });

                var actualResult = TypeConverter.TryConvertStringToType(input, typeof(T), mockDatabaseController.Object, out object actualObject);

                Assert.False(actualResult.IsSuccess);
            }

            public static IEnumerable<object[]> TestData()
            {
                return new List<object[]>
                {
                    new object[] { new Product() },
                    new object[] { new Category() },
                    new object[] { new Warehouse() },
                    new object[] { new Location() }
                };
            }
        }

        public class InventoryEntryConversionTests
        {
            [Theory]
            [MemberData(nameof(ValidUintTestData))]
            public void Convert_IdOfExistentInventoryEntry_ReturnsSuccess(string stringInput, uint uintInput)
            {
                var mockDatabaseController = new Mock<IDatabaseController>();
                var entity = new InventoryEntry();
                mockDatabaseController.Setup(x => x.TryReadEntityById<InventoryEntry>(uintInput, out entity)).Returns(new Result() { IsSuccess = true });
                var expectedType = typeof(InventoryEntry);

                var actualResult = TypeConverter.TryConvertStringToType(stringInput, typeof(InventoryEntry), mockDatabaseController.Object, out object actualObject);

                Assert.True(actualResult.IsSuccess);
                Assert.IsType(expectedType, actualObject);
            }

            [Theory]
            [MemberData(nameof(ValidUintTestData))]
            public void Convert_IdOfNonexistentInventoryEntry_ReturnsFailure(string stringInput, uint uintInput)
            {
                var mockDatabaseController = new Mock<IDatabaseController>();
                var entity = new InventoryEntry();
                mockDatabaseController.Setup(x => x.TryReadEntityById<InventoryEntry>(uintInput, out entity)).Returns(new Result() { IsSuccess = false });
                
                var actualResult = TypeConverter.TryConvertStringToType(stringInput, typeof(InventoryEntry), mockDatabaseController.Object, out object actualObject);

                Assert.False(actualResult.IsSuccess);
            }

            [Theory]
            [MemberData(nameof(InvalidUintTestData))]
            public void Convert_InvalidUintId_ReturnsFailure(string stringInput)
            {
                var mockDatabaseController = new Mock<IDatabaseController>();
                var entity = new InventoryEntry();
                // Value of uintInput is irrelevant as execution should never reach TryReadEntityByCode in this case
                var uintInput = 1u;
                mockDatabaseController.Setup(x => x.TryReadEntityById<InventoryEntry>(uintInput, out entity)).Returns(new Result() { IsSuccess = false });
                
                var actualResult = TypeConverter.TryConvertStringToType(stringInput, typeof(InventoryEntry), mockDatabaseController.Object, out object actualObject);

                Assert.False(actualResult.IsSuccess);
            }

            public static IEnumerable<object[]> ValidUintTestData()
            {
                return new List<object[]>
                {
                    new object[] { "1", 1u },
                    new object[] { uint.MaxValue.ToString(), uint.MaxValue }
                };
            }

            public static IEnumerable<object[]> InvalidUintTestData()
            {
                return new List<object[]>
                {
                    new object[] { "-1" },
                    new object[] { "1 invalid input" }
                };
            }
        }
    }
}
