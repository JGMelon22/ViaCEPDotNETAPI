using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using ViaCepDotNetAPI.Domains.Entities;
using ViaCepDotNetAPI.Domains.Shared;
using ViaCepDotNetAPI.Infrastructure.Configurations;
using ViaCepDotNetAPI.Interfaces;

namespace ViaCepDotNetAPI.Infrastructure.Services;

public class ViaCepService : IViaCepService
{
    private readonly string _baseUrl;
    private readonly HttpClient _httpClient;
    private ILogger<ViaCepService> _logger;

    public ViaCepService
    (
        IOptions<ViaCepOptions> options,
        HttpClient httpClient,
        ILogger<ViaCepService> logger
    )
    {
        _baseUrl = options.Value.BaseUrl;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<Result<Root?>> GetAddressByCepAsync(string cep)
    {
        try
        {
            JsonSerializerOptions options = new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new JsonStringEnumConverter() }
            };

            options.Converters.Add(new JsonStringEnumConverter());

            string normalizedCep = NormalizeCepInputFormat(cep);

            Root? address = await _httpClient.GetFromJsonAsync<Root>(
                $"{_baseUrl}{normalizedCep}/json", options);

            return address != null
                ? Result<Root?>.Success(address)
                : Result<Root?>.Failure("Failed to deserialize response.");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error getting address from ViaCep: {Error}", ex);

            return Result<Root?>.Failure("An error occurred while fetching CEP information.");
        }
    }

    private static string NormalizeCepInputFormat(string cep)
    {
        if (!string.IsNullOrWhiteSpace(cep) && cep.All(char.IsDigit) && cep.Length == 8)
            return cep;

        else
            return Regex.Replace(cep, @"[\s\-\.]", "");
    }
}
