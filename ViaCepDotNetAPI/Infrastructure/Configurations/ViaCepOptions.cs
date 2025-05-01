namespace ViaCepDotNetAPI.Infrastructure.Configurations;

public sealed class ViaCepOptions
{
    public const string SectionName = "ViaCep";
    
    public string BaseUrl { get; set; } = string.Empty;
}