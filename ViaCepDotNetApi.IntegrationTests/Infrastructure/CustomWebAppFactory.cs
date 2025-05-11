using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using WireMock.Server;

namespace ViaCepDotNetApi.IntegrationTests.Infrastructure;

public class CustomWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public WireMockServer WireMockServer { get; private set; } = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            Dictionary<string, string> testSettings = new()
            {
                { "ViaCep:BaseUrl", WireMockServer.Urls[0] + "/ws/" }
            };

            config.AddInMemoryCollection(testSettings!);
        });
    }

    public Task InitializeAsync()
    {
        WireMockServer = WireMockServer.Start();
        return Task.CompletedTask;
    }

    public new async Task DisposeAsync()
    {
        WireMockServer.Dispose();
        await base.DisposeAsync();
    }
}