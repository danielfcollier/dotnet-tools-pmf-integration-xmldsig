#nullable disable

using System.Text.Json.Serialization;

namespace Models;

public class PartnerCredentials
{
    [JsonPropertyName("certificate")]
    public CertificateSignature Certificate { get; init; }
    
    [JsonPropertyName("login")]
    public LoginCredentials Login { get; init; }
    
    [JsonPropertyName("credential")]
    public ClientCredentials Credential { get; init; }

    public class CertificateSignature
    {
        [JsonPropertyName("validity")]
        public string Validity { get; init; }

        [JsonPropertyName("password")]
        public string Password { get; init; }

        [JsonPropertyName("data")]
        public string Data { get; init; }
    }

    public class LoginCredentials
    {
        [JsonPropertyName("user")]
        public string User { get; init; }

        [JsonPropertyName("password")]
        public string Password { get; init; }
    }

    public class ClientCredentials
    {
        [JsonPropertyName("client-id")]
        public string ClientId { get; init; }

        [JsonPropertyName("client-secret")]
        public string ClientSecret { get; init; }
    }
}