#nullable disable

using System.Text.Json.Serialization;

namespace Models;

public class Partner
{
    [JsonPropertyName("id")]
    public string Id { get; init; }

    [JsonPropertyName("municipalData")]
    public MunicipalData Municipal { get; init; }

    [JsonPropertyName("services")]
    public List<Service> Services { get; init; }

    [JsonPropertyName("secrets")]
    public Credentials Secrets { get; init; }

    public class MunicipalData
    {
        [JsonPropertyName("numeroAedf")]
        public string NumeroAedf { get; init; }
    }

    public class Service
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("idCNAE")]
        public int IdCnae { get; init; }

        [JsonPropertyName("cst")]
        public int Cst { get; init; }

        [JsonPropertyName("baseCalculoFactor")]
        public decimal BaseCalculoFactor { get; init; }

        [JsonPropertyName("aliquota")]
        public decimal Aliquota { get; init; }

        [JsonPropertyName("description")]
        public string DescricaoServico { get; init; }

        [JsonPropertyName("taxes")]
        public TaxesModel Taxes { get; init; }

        public class TaxesModel
        {
            [JsonPropertyName("state")]
            public decimal State { get; init; }

            [JsonPropertyName("federal")]
            public decimal Federal { get; init; }
        }
    }
}