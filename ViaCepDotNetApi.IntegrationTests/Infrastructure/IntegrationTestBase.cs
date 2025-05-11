using WireMock.ResponseBuilders;
using WireMock.RequestBuilders;

namespace ViaCepDotNetApi.IntegrationTests.Infrastructure;

public abstract class IntegrationTestBase : IClassFixture<CustomWebAppFactory>, IAsyncLifetime
{
    protected readonly CustomWebAppFactory Factory;
    protected readonly HttpClient Client;

    protected IntegrationTestBase(CustomWebAppFactory factory)
    {
        Factory = factory;
        Client = factory.CreateClient();
    }

    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public virtual Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    protected void SetupViaCepApiMock(string cep, string response)
    {
        Factory.WireMockServer
            .Given(
                Request.Create()
                    .WithPath($"/ws/{cep}/json")
                    .UsingGet())
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(response));
    }
    
    protected void SetupViaCepApiMockApiNotFound(string cep)
    {
        Factory.WireMockServer
            .Given(
                Request.Create()
                    .WithPath($"/ws/{cep}/json")
                    .UsingGet())
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody("{\n  \"erro\": \"true\"\n}"));
    }
}