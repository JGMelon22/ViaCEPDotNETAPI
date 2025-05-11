using ViaCepDotNetAPI.Domains.Dtos;
using ViaCepDotNetAPI.Domains.Entities;
using ViaCepDotNetAPI.Domains.Shared;

namespace ViaCepDotNetAPI.Domains.Mappings;

public static class MappingExtensions
{
    public static Result<RootResponse> ToResponse(this Root root)
    {
        RootResponse response = new()
        {
            Cep = root.Cep,
            Logradouro = root.Logradouro,
            Complemento = root.Complemento,
            Unidade = root.Unidade,
            Bairro = root.Bairro,
            Localidade = root.Localidade,
            Uf = root.Uf,
            Estado = root.Estado,
            Regiao = root.Regiao,
            Ibge = root.Ibge,
            Gia = root.Gia,
            Ddd = root.Ddd,
            Siafi = root.Siafi,
            Erro = root.Erro ?? "False"
        };

        return Result<RootResponse>.Success(response);
    }
}