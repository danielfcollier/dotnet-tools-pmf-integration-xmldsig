#nullable disable

using System.Text.Json.Serialization;

namespace Models;

public class Address
{
    [JsonPropertyName("cep")]
    public string PostalCode { get; set; }

    [JsonPropertyName("logradouro")]
    public string Street { get; set; }

    [JsonPropertyName("bairro")]
    public string Neighborhood { get; set; }

    [JsonPropertyName("uf")]
    public string State { get; set; }

    [JsonPropertyName("ibge")]
    public string MunicipalCode { get; set; }

    [JsonPropertyName("countryCode")]
    public int CountryCode { get; set; } = 1058; // BRAZIL COUNTRY CODE
}