using System.Diagnostics;
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


    // [Fact]
    // public async Task Should_Return_Existing_Address_When_Cep_IsFound()
    // {
    //     // Arrange
    //     string validCep = "21211000";
    //     var expectedResponse = new RootResponse
    //     {
    //         Cep = "21211-000",
    //         Logradouro = "Rua Exemplo",
    //         Bairro = "Bairro Exemplo",
    //         Localidade = "Rio de Janeiro",
    //         Uf = UnidadeFederativa.RJ,
    //         Estado = "Rio de Janeiro",
    //         Regiao = "Sudeste"
    //     };
    //
    //     SetupViaCepApiMock(validCep, expectedResponse);
    //
    //     // Act
    //     var response = await Client.GetAsync($"/viaCep?cep={validCep}");
    //
    //     // Assert
    //     response.EnsureSuccessStatusCode(); // Status code 200-299
    //     var address = await response.Content.ReadFromJsonAsync<RootResponse>();
    //
    //     Assert.NotNull(address);
    //     Assert.Equal(expectedResponse.Cep, address.Cep);
    //     Assert.Equal(expectedResponse.Logradouro, address.Logradouro);
    // }

    [Fact]
    public async Task GetAddressAsync_ShouldReturnValidResponse_WhenCepIsValid()
    {
        // Arrange
        var validCep = "21211000";
        var fullUrl = $"{Client.BaseAddress}/viaCep?cep={validCep}";
    
        // Set a breakpoint here to inspect fullUrl
    
        // Act
        var response = await Client.GetAsync($"/viaCep?cep={validCep}");

        // Assert
        response.EnsureSuccessStatusCode();  // Ensures 2xx response
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Rua Exemplo", content);
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