using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SMSMicroService.Tests.Utilities
{
    public class HostedServiceRunner
    {
        public HostedServiceRunner() { }

        public static async Task RunHostedServiceAsync<THostedService>(
            TimeSpan testDuration, Action<IServiceCollection> configureServices = null)
            where THostedService : IHostedService
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var hostBuilder = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddHostedService<THostedService>();

                        configureServices?.Invoke(services);

                        services.AddSingleton<IHostedService, TestHostedService>(provider =>
                            new TestHostedService(provider.GetRequiredService<THostedService>(), cancellationToken));
                    });
                });

            using var host = hostBuilder.Start();

            await Task.Delay(testDuration, cancellationToken);

            cancellationTokenSource.Cancel();
        }
    }
}
