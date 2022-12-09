#nullable disable

using System.Text.Json.Serialization;

namespace Models;

public class Account
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("balance")]
    public decimal Balance { get; set; }
}