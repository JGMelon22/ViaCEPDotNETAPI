using ViaCepDotNetAPI.Domains.Entities;
using ViaCepDotNetAPI.Domains.Shared;

namespace ViaCepDotNetAPI.Interfaces;

public interface IViaCepService
{
    Task<Result<Root?>> GetAddressByCepAsync(string cep);
}