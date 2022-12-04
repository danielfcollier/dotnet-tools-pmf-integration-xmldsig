#nullable disable

using System.Text.Json.Serialization;

namespace Models;

public class CustomerServices
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; init; }

    [JsonPropertyName("value")]
    public decimal Value { get; init; }

    [JsonPropertyName("description")]
    public string Description { get; init; }
}