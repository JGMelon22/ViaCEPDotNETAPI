using ViaCepDotNetAPI.Domains.Entities;

namespace ViaCepDotNetAPI.Interfaces;

public interface IViaCepService
{
    Task<Root> GetAddressByCepAsync(string cep);
}