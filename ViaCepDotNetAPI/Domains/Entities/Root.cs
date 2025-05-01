using System.Text.Json.Serialization;
using ViaCepDotNetAPI.Domains.Enums;

namespace ViaCepDotNetAPI.Domains.Entities;

public class Root
{
    public string Cep { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public string Complemento { get; set; } = string.Empty;
    public string Unidade { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Localidade { get; set; } = string.Empty;
    public UnidadeFederativa Uf { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string Regiao { get; set; } = string.Empty;
    public string Ibge { get; set; } = string.Empty;
    public string Gia { get; set; } = string.Empty;
    public string Ddd { get; set; } = string.Empty;
    public string Siafi { get; set; } = string.Empty;
}
