using Microsoft.Extensions.Options;
using ViaCepDotNetAPI.Infrastructure.Configurations;
using ViaCepDotNetAPI.Infrastructure.Services;
using ViaCepDotNetAPI.Interfaces;

namespace ViaCepDotNetAPI.Extensions;

public static class IocExtensions
{
    public static IServiceCollection AddViaCepClient(this IServiceCollection services)
    {
        services.AddHttpClient<IViaCepService, ViaCepService>((serviceProvider, client) =>
        {
            ViaCepOptions viaCepConfig = serviceProvider
               .GetRequiredService<IOptions<ViaCepOptions>>()
               .Value;

            client.BaseAddress = new Uri(viaCepConfig.BaseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        return services;
    }
}
