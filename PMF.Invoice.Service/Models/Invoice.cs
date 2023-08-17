#nullable disable

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using System.Text.Json.Serialization;

namespace Models;

public class Invoice
{
    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [Required]
    [JsonPropertyName("id")]
    public string Id { get; init; }

    [JsonPropertyName("externalId")]
    public string ExternalId { get; init; }

    [JsonPropertyName("paymentId")]
    public string PaymentId { get; init; }

    [JsonPropertyName("documentId")]
    public string DocumentId { get; init; }

    [JsonPropertyName("fullName")]
    public string FullName { get; init; }

    [JsonPropertyName("birthday")]
    public string Birthday { get; init; }


    [JsonPropertyName("email")]
    public string Email { get; init; }

    [JsonPropertyName("cellphone")]
    public string Cellphone { get; init; }

    [JsonPropertyName("postalCode")]
    public string PostalCode { get; init; }

    [JsonPropertyName("consultationDate")]
    public string ConsultationDate { get; init; }

    [JsonPropertyName("description")]
    public string Description { get; init; }

    [JsonPropertyName("consultationValue")]
    public decimal ConsultationValue { get; init; }

    [JsonPropertyName("service")]
    public string Services { get; init; }

    [JsonPropertyName("cmcCode")]
    public string CmcCode { get; init; }

    [JsonPropertyName("verificationCode")]
    public string VerificationCode { get; init; }

    [JsonPropertyName("observation")]
    public string Observation { get; init; }

    [JsonPropertyName("effectiveDate")]
    public string EffectiveDate { get; init; }
    
    [JsonPropertyName("invoiceDate")]
    public string InvoiceDate { get; init; }

    [DefaultValue(false)]
    [JsonPropertyName("isSent")]
    public bool IsSent { get; init; }

}