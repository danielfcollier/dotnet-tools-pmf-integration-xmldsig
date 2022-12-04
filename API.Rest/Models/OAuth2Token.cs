#nullable disable

using System.Text.Json.Serialization;

namespace Models;

public class OAuth2Token
{
    [JsonPropertyName("access_token")]
    public string Token { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonPropertyName("scope")]
    public string Scope { get; set; }
}