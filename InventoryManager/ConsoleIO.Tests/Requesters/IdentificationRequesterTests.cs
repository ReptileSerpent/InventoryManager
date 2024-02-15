using InventoryManager.ConsoleIO.Interfaces;
using InventoryManager.ConsoleIO.Requesters;
using InventoryManager.Data.Interfaces;
using InventoryManager.DatabaseAccess.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace InventoryManager.ConsoleIO.Tests.Requesters
{
    public class IdentificationRequesterTests
    {
        public class CodeRequestTests {
            [Fact]
            public void RequestCode_ValidInput_ReturnsSuccess()
            {
                var input = "code 1";
                var mockLogger = new Mock<ILogger>();
                var mockConsole = new Mock<IConsole>();
                mockConsole.SetupSequence(x => x.ReadLine()).Returns(input);
                var mockDatabaseController = new Mock<IDatabaseController>();
                var requester = new IdentificationRequester(mockLogger.Object, mockConsole.Object, mockDatabaseController.Object);

                var actualResult = requester.RequestCode<IEntityWithCode>(out string actualCode);

                Assert.True(actualResult.IsSuccess);
                Assert.Equal(input, actualCode);
            }

            [Fact]
            public void RequestCode_NullInput_ReturnsFailure()
            {
                string? input = null;
                var mockLogger = new Mock<ILogger>();
                var mockConsole = new Mock<IConsole>();
                mockConsole.SetupSequence(x => x.ReadLine()).Returns(input);
                var mockDatabaseController = new Mock<IDatabaseController>();
                var requester = new IdentificationRequester(mockLogger.Object, mockConsole.Object, mockDatabaseController.Object);

                var actualResult = requester.RequestCode<IEntityWithCode>(out string actualCode);

                Assert.False(actualResult.IsSuccess);
            }
        }

        public class IdRequestTests
        {
            [Theory]
            [MemberData(nameof(ValidUintTestData))]
            public void RequestId_ValidUintId_ReturnsSuccess(string input, uint expectedId)
            {
                var mockLogger = new Mock<ILogger>();
                var mockConsole = new Mock<IConsole>();
                mockConsole.SetupSequence(x => x.ReadLine()).Returns(input);
                var mockDatabaseController = new Mock<IDatabaseController>();
                var requester = new IdentificationRequester(mockLogger.Object, mockConsole.Object, mockDatabaseController.Object);

                var actualResult = requester.RequestId<IEntity>(out uint actualId);

                Assert.True(actualResult.IsSuccess);
                Assert.Equal(expectedId, actualId);
            }

            public static IEnumerable<object[]> ValidUintTestData()
            {
                return new List<object[]>
                {
                    new object[] { "1", 1u },
                    new object[] { uint.MaxValue.ToString(), uint.MaxValue }
                };
            }
        }
    }
}
