using System.Net;
using ViaCepDotNetApi.IntegrationTests.Infrastructure;

namespace ViaCepDotNetApi.IntegrationTests;

public sealed class CepEndpointTests : IntegrationTestBase
{
    public CepEndpointTests(CustomWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetAddressAsync_ShouldReturnValidResponse_WhenCepIsValid()
    {
        // Arrange
        string validCep = "21211000";
        string expectedResponse = """
                                  {
                                    "cep": "21211-000",
                                    "logradouro": "Rua Salviano Valente",
                                    "complemento": "",
                                    "unidade": "",
                                    "bairro": "Penha Circular",
                                    "localidade": "Rio de Janeiro",
                                    "uf": "RJ",
                                    "estado": "Rio de Janeiro",
                                    "regiao": "Sudeste",
                                    "ibge": "3304557",
                                    "gia": "",
                                    "ddd": "21",
                                    "siafi": "6001"
                                  }
                                  """;

        SetupViaCepApiMock(validCep, expectedResponse);

        // Act
        HttpResponseMessage response = await Client.GetAsync($"/viaCep?cep={validCep}");

        // Assert
        response.EnsureSuccessStatusCode();
        string content = await response.Content.ReadAsStringAsync();

        Assert.Contains("Rua Salviano Valente", content);
        Assert.Contains("Penha Circular", content);
        Assert.Contains("Rio de Janeiro", content);
        Assert.Contains("RJ", content);
        Assert.Contains("21211-000", content);
        Assert.Contains("Rio de Janeiro", content);
        Assert.Contains("Sudeste", content);
        Assert.Contains("3304557", content);
        Assert.Contains("21", content);
        Assert.Contains("6001", content);
    }

    [Fact]
    public async Task Should_Return_BadRequest_When_Cep_IsInvalid()
    {
        // Arrange
        string invalidCep = "99999999";

        SetupViaCepApiMockApiNotFound(invalidCep);
        
        // Act
        HttpResponseMessage response = await Client.GetAsync($"/viaCep?cep={invalidCep}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Contains("Location information for '99999999' not found.", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Should_Return_BadRequest_When_Cep_IsEmpty()
    {
        // Arrange & Act
        HttpResponseMessage response = await Client.GetAsync("/viaCep?cep=");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}