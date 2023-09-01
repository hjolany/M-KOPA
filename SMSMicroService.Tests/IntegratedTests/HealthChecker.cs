using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;

namespace SMSMicroService.Tests.Integrated_Test
{
    public class HealthChecker: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;

        public HealthChecker(WebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task HealthCheckEndpoint_ReturnsOk()
        {
            // Arrange

            // Act
            var response = await _httpClient.GetAsync("/health");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
