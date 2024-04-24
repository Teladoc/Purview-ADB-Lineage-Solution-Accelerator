using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Function.Domain.Helpers;
using Microsoft.Azure.Functions.Worker;
using System.IO;
using System.Net;
using System.Security.Claims;
using UnitTests.Mocks;

namespace UnitTests.Function.Domain.Helpers
{
    public class HttpHelperTests
    {
        private HttpHelper _util = new();
        
        [Fact]
        public void ValidateRequestHeaders_Should_Return_True_When_SourceHeader_Matches()
        {
            // Arrange
            Mock<FakeHttpRequestData> mockRequest = new();
            mockRequest.Setup(x => x.Headers).Returns([]); // Set up the headers
            mockRequest.Object.Headers.Add("x-teladoc-udf-ol-source", "ExpectedValue"); // Set the expected header value

            // Act
            var result = _util.ValidateRequestHeaders(mockRequest.Object, "ExpectedValue");

            // Assert
            Assert.True(result, "Expected header value should match.");
        }

        [Fact]
        public void ValidateRequestHeaders_Should_Return_False_When_SourceHeader_Does_Not_Match()
        {
            // Arrange
            Mock<FakeHttpRequestData> mockRequest = new();
            mockRequest.Setup(x => x.Headers).Returns([]); // Set up the headers
            mockRequest.Object.Headers.Add("x-teladoc-udf-ol-source", "DifferentValue"); // Set a different header value

            // Act
            var result = _util.ValidateRequestHeaders(mockRequest.Object, "ExpectedValue");

            // Assert
            Assert.False(result, "Expected header value should not match.");
        }

        [Fact]
        public void ValidateRequestHeaders_Should_Return_False_When_SourceHeader_Is_Not_Present()
        {
            // Arrange
            Mock<FakeHttpRequestData> mockRequest = new();
            mockRequest.Setup(x => x.Headers).Returns([]); // Set up the headers

            // Act
            var result = _util.ValidateRequestHeaders(mockRequest.Object, "ExpectedValue");

            // Assert
            Assert.False(result, "Header should not be present.");
        }
    }
}
