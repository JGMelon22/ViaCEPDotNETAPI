using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using ViaCepDotNetAPI.Domains.Entities;
using ViaCepDotNetAPI.Infrastructure.Configurations;
using ViaCepDotNetAPI.Interfaces;

namespace ViaCepDotNetAPI.Infrastructure.Services;

public class ViaCepService : IViaCepService
{
    private readonly string _baseUrl;
    private readonly IHttpClientFactory _httpClientFactory;
    private ILogger<ViaCepService> _logger;

    public ViaCepService(IOptions<ViaCepOptions> options, IHttpClientFactory httpClientFactory, ILogger<ViaCepService> logger)
    {
        _baseUrl = options.Value.BaseUrl;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<Root> GetAddressByCepAsync(string cep)
    {
        using HttpClient client = _httpClientFactory.CreateClient();

        try
        {
            JsonSerializerOptions options = new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            options.Converters.Add(new JsonStringEnumConverter());

            Root? root = await client.GetFromJsonAsync<Root>(
                $"{_baseUrl}{cep}/json", options);

            return root ?? null!;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error getting address from ViaCep: {Error}", ex);
        }

        return null!;
    }

}
