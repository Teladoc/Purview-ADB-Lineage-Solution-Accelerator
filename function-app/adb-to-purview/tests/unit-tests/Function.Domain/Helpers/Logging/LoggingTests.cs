using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Function.Domain.Helpers.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using UnitTests.Mocks;

namespace unit_tests.Function.Domain.Helpers.Logging
{
    public class LoggingTests
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            var services = new ServiceCollection()
                .AddLogging(options => {
                    options.AddConsole();
                }).BuildServiceProvider();
            var logger = services.GetRequiredService<ILogger<LoggingTests>>();
            var mockLogger = new FakeLogger<LoggingTests>(logger);
            var testEx = new Exception("Test Exception");

            // Act
            var actualException = Record.Exception(() => mockLogger.LogError(testEx, ErrorCodes.SynapseAPI.GetSynapseStorageLocation, "SynapseClient-GetSynapseStorageLocation: Failed to get storage location for {databaseName} and {tableName}. Endpoint: {endpoint}. ErrorMessage {ErrorMessage}", "TestDatabase", "TestTable", "TestUrl", testEx.Message));
            
            // Assert
            Assert.Null(actualException);
            Assert.False(mockLogger.ArgumentExceptionRaised);
        }
    }
}