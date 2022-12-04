#nullable disable

using System.Text.Json.Serialization;

namespace Models;

public class Customer
{
    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("id")]
    public string Id { get; init; }

    [JsonPropertyName("fullName")]
    public string FullName { get; init; }

    [JsonPropertyName("birthday")]
    public string Birthday { get; init; }

    [JsonPropertyName("documentId")]
    public string DocumentId { get; init; }

    [JsonPropertyName("email")]
    public string Email { get; init; }

    [JsonPropertyName("postalCode")]
    public string PostalCode { get; init; }

    [JsonPropertyName("cellphone")]
    public string Cellphone { get; init; }

    [JsonPropertyName("consultationDate")]
    public string ConsultationDate { get; init; }

    [JsonPropertyName("consultationType")]
    public string ConsultationType { get; init; }

    [JsonPropertyName("consultationModality")]
    public string ConsultationModality { get; init; }

    [JsonPropertyName("Service")]
    public string Services { get; init; }

    [JsonPropertyName("isSent")]
    public string IsSent { get; init; }

    [JsonPropertyName("effectiveDate")]
    public string EffectiveDate { get; init; }
}