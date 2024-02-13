using AdbToPurview.Function;
using Castle.Core.Logging;
using Function.Domain.Helpers.Logging;
using Function.Domain.Models.OL;
using Function.Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace unit_tests.Functions
{
    public class PurviewOutTests
    {
        private readonly Mock<ILogger<PurviewOut>> _mockLogger;
        private readonly Mock<IOlToPurviewParsingService> _mockOlToPurviewParsingService;
        private readonly Mock<IPurviewIngestion> _mockPurviewIngestion;
        private readonly Mock<IOlConsolidateEnrichFactory> _mockOlEnrichmentFactory;
        private readonly Mock<IOlClaimCheckService> _mockOlClaimCheckService;
        private readonly Mock<IOlFilter> _mockOlFilter;
        private readonly PurviewOut _function;

        public PurviewOutTests()
        {
            _mockLogger = new Mock<ILogger<PurviewOut>>();
            _mockOlToPurviewParsingService = new Mock<IOlToPurviewParsingService>();
            _mockOlEnrichmentFactory = new Mock<IOlConsolidateEnrichFactory>();
            _mockPurviewIngestion = new Mock<IPurviewIngestion>();
            _mockOlClaimCheckService = new Mock<IOlClaimCheckService>();
            _mockOlFilter = new Mock<IOlFilter>();
            _function = new PurviewOut(_mockLogger.Object, _mockOlToPurviewParsingService.Object, _mockPurviewIngestion.Object, _mockOlEnrichmentFactory.Object, _mockOlClaimCheckService.Object, _mockOlFilter.Object);
        }

        [Fact]
        public async Task Given_InvalidClaimCheckMessage_When_ParsingPurviewOutMessage_Expect_ErrorReceived()
        {
            // Arrange

            // Act
            var actual = await _function.Run("{}");

            // Assert
            Assert.NotNull(actual);
            Assert.Contains("Error in PurviewOut function", actual);

        }
        //"{\"ClaimCheck\":{\"Id\":\"2\",\"Checksum\":\"hash\"}}"
    }
}

