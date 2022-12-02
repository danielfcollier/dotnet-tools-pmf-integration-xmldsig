#nullable disable

using System.Text.Json.Serialization;

namespace Model;

public class Account
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("balance")]
    public decimal Balance { get; set; }
}