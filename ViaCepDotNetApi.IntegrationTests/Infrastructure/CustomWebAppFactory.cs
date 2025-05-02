using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using WireMock.Server;

namespace ViaCepDotNetApi.IntegrationTests.Infrastructure;

public class CustomWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public WireMockServer WireMockServer { get; private set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ViaCep:BaseUrl:", $"{WireMockServer.Urls[0]}");

        // builder.ConfigureAppConfiguration((context, config) =>
        // {
        //     Dictionary<string, string> testSettings = new()
        //     {
        //         { "ViaCep:BaseUrl", WireMockServer.Urls[0] + "/" } // EX: http://localhost:port/
        //     };
        //
        //     config.AddInMemoryCollection(testSettings!);
        // });
    }

    public Task InitializeAsync()
    {
        WireMockServer = WireMockServer.Start();
        return Task.CompletedTask;
    }

    public new Task DisposeAsync()
    {
        WireMockServer.Dispose();
        return Task.CompletedTask;
    }
}