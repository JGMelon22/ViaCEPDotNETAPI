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
            .WithName("GetAddressInformationViaCep")
            .WithOpenApi();
    }

    private static async Task<IResult> GetAddressAsync(string cep, IViaCepService viaCepService)
    {
        Result<Root?> address = await viaCepService.GetAddressByCepAsync(cep);

        if (!address.IsSuccess)
            return Results.BadRequest(address.Message);

        if (address.Data!.Erro == "true")
            return Results.NotFound($"Location information for '{cep}' not found.");

        if (address.Data is null)
            return Results.BadRequest(address.Message);

        Result<RootResponse>? mappedResponse = address.Data?.ToResponse();

        return Results.Ok(mappedResponse);
    }
}