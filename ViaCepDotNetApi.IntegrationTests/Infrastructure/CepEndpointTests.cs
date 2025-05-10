using System.Net;
using System.Net.Http.Json;
using ViaCepDotNetAPI.Domains.Dtos;
using ViaCepDotNetAPI.Domains.Enums;

namespace ViaCepDotNetApi.IntegrationTests.Infrastructure;

public sealed class CepEndpointTests : IntegrationTestBase
{
    public CepEndpointTests(CustomWebAppFactory factory) : base(factory)
    {
    }


    [Fact]
    public async Task Should_Return_Existing_Address_When_Cep_IsFound()
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
        var response = await Client.GetAsync($"/viaCep?cep={validCep}");

        // Assert
        response.EnsureSuccessStatusCode(); // Status code 200-299
        var address = await response.Content.ReadFromJsonAsync<RootResponse>();

        Assert.NotNull(address);
        Assert.Equal("21211-000", address.Cep);
        Assert.Equal("Rua Salviano Valente", address.Logradouro);
    }

    [Fact]
    public async Task GetAddressAsync_ShouldReturnValidResponse_WhenCepIsValid()
    {
        // Arrange
        var validCep = "21211000";
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
        var response = await Client.GetAsync($"/viaCep?cep={validCep}");

        // Assert
        response.EnsureSuccessStatusCode(); // Ensures 2xx response
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Rua Salviano Valente", content);
    }

    [Fact]
    public async Task Should_Return_NotFound_When_Cep_DoesNotExist()
    {
        // Arrange
        string invalidCep = "00000000";
        SetupViaCepApiMockApiNotFound(invalidCep);

        // Act
        var response = await Client.GetAsync($"/viaCep?cep={invalidCep}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Should_Return_BadRequest_When_Cep_IsInvalid()
    {
        // Arrange
        string invalidCep = "invalid";

        // Act
        var response = await Client.GetAsync($"/viaCep?cep={invalidCep}");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Should_Return_BadRequest_When_Cep_IsEmpty()
    {
        // Arrange & Act
        var response = await Client.GetAsync("/viaCep?cep=");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}