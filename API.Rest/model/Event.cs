#nullable disable

using System.Text.Json.Serialization;

namespace Model
{
    public static class EventType
    {
        public const string Deposit = "deposit";
        public const string Withdraw = "withdraw";
        public const string Transfer = "transfer";
    }

    public class Event
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("origin")]
        public string? Origin { get; set; }

        [JsonPropertyName("destination")]
        public string? Destination { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

    }
}