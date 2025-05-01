using ViaCepDotNetAPI.Domains.Dtos;
using ViaCepDotNetAPI.Domains.Entities;
using ViaCepDotNetAPI.Domains.Mappings;
using ViaCepDotNetAPI.Domains.Shared;
using ViaCepDotNetAPI.Interfaces;

namespace ViaCepDotNetAPI.Endpoints;

public static class CepEndpoint
{
    public static void MapCepRoutes(this IEndpointRouteBuilder app)
    {
        app.MapGet("/viaCep", GetAddressAsync)
            .WithName("GetAdddressInformationViaCep")
            .WithOpenApi();
    }

    private static async Task<IResult> GetAddressAsync(string cep, IViaCepService viaCepService)
    {
        Result<Root?> data = await viaCepService.GetAddressByCepAsync(cep);

        if (data is null)
            return Results.NotFound($"Location information for '{cep}' not found.");

        Result<RootResponse>? mappedResponse = data.Data?.ToResponse();

        return data.IsSuccess
            ? Results.Ok(mappedResponse)
            : Results.BadRequest(mappedResponse);
    }
}