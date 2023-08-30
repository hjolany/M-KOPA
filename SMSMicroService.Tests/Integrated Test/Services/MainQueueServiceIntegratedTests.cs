using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SMSApi.Boundary.Requests;
using SMSMicroService.Services;

namespace SMSMicroService.Tests.Integrated_Test.Services
{
    public class MainQueueServiceIntegratedTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public MainQueueServiceIntegratedTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ServiceBeingExecuted()
        {
            // Arrange & Action
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var builder = _factory.WithWebHostBuilder(hostBuilder =>
            {
                hostBuilder.ConfigureServices(collection =>
                {
                    collection.AddSingleton<Program>();
                    collection.AddHostedService<MainQueueService>();
                });
            });

            var app = builder.CreateClient(new WebApplicationFactoryClientOptions());

            var message = new MessageDomain()
            {
                PhoneNumber = "+40",
                Content = "Sample"
            };
            

            _ =await app.SendAsync(new HttpRequestMessage(HttpMethod.Post, "/api/v1/queue/send")
            {
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(message), 
                    Encoding.UTF8, "application/json")
            });
            

            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);

            // Assert

            var response = await app.GetAsync("/api/v1/queue/consumer/count");

            response.ShouldNotBeNull();
            response.ShouldBeOfType(typeof(HttpResponseMessage));

            var count = await response?.Content.ReadAsStringAsync();

            count.ShouldNotBeNullOrEmpty();
            int.Parse(count).ShouldBeGreaterThan(0); 
        }
    }
}
