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
        Result<Root?> data = await viaCepService.GetAddressByCepAsync(cep);

        if (!data.IsSuccess)
            return Results.BadRequest(data.Message);

        if (data.Data is null)
            return Results.NotFound($"Location information for '{cep}' not found.");

        Result<RootResponse>? mappedResponse = data.Data?.ToResponse();

        return Results.Ok(mappedResponse);
    }
}