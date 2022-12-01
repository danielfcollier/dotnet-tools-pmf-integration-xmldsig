#nullable disable

using System.Text.Json.Serialization;

namespace Model
{
    public class Transaction
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("origin")]
        public Account? Origin { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("destination")]
        public Account? Destination { get; set; }
    }
}