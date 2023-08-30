using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using SMSMicroService.Entities.Domains;
using SMSMicroService.Helpers;
using System.Net;

namespace SMSMicroService.Tests.UnitTests.Helpers
{
    public class CallApiTests
    {
        private readonly Mock<ILogger<CallApi<MessageDomain>>> _logger;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClientMock;

        public CallApiTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClientMock = new HttpClient(_httpMessageHandlerMock.Object);
            _logger = new Mock<ILogger<CallApi<MessageDomain>>>();
        }

        [Fact]
        public async Task GetThenReturns200()
        {
            // Arrange
            var url = "https://example.com/api"; 

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var _sut = new CallApi<MessageDomain>(_logger.Object, _httpClientMock);

            // Act
            var result = await _sut.Get(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.True(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task GetThenLogExceptionWhenExceptionFired()
        {
            // Arrange
            var url = "https://example.com/api"; 

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Sample Exception"));

            var _sut = new CallApi<MessageDomain>(_logger.Object, _httpClientMock);

            // Act
            var result = await _sut.Get(url);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.False(result.IsSuccessStatusCode);

            _logger.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
        }

        [Fact]
        public async Task GetThenReturnsBadRequestWhenExceptionFired()
        {
            // Arrange
            var url = "https://example.com/api"; 
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Sample Exception"));

            var _sut = new CallApi<MessageDomain>(_logger.Object, _httpClientMock);

            // Act
            var result = await _sut.Get(url);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task PostThenReturns200()
        {
            // Arrange
            var url = "https://example.com/api";
            var data = new MessageDomain()
            {
                PhoneNumber = "123",
                Content = "Sample"
            };

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var _sut = new CallApi<MessageDomain>(_logger.Object, _httpClientMock);

            // Act
            var result = await _sut.Post(url, data);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.True(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task PostThenLogExceptionWhenExceptionFired()
        {
            // Arrange
            var url = "https://example.com/api";
            var data = new MessageDomain()
            {
                PhoneNumber = "123",
                Content = "Sample"
            };
             
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Sample Exception"));

            var _sut = new CallApi<MessageDomain>(_logger.Object, _httpClientMock);

            // Act
            var result = await _sut.Post(url, data);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.False(result.IsSuccessStatusCode);

            _logger.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
        }
    }
}
