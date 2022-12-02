using System.Net;
using System.Xml;
using System.Text.Json;

using Models;

namespace Services;

public static class InvoiceService
{
    public static readonly bool IS_PROD = false;
    public static readonly string BASE_URL = IS_PROD ?
        "https://nfps-e.pmf.sc.gov.br"        // PROD
        : "https://nfps-e-hml.pmf.sc.gov.br"; // HOMOLOG

    public static async Task<(string, HttpStatusCode)> Create(XmlDocument signedInvoice, OAuth2TokenService oAuth2Token)
    {
        string endpoint = "api/v1/processamento/notas/processa";
        Dictionary<string, string> headers = new()
        {
            { "Content-Type", "application/xml" },
            { "Token", oAuth2Token.Token },
        };
        Uri uri = new($"{BASE_URL}/{endpoint}");
        var (response, statusCode) = await HttpService.PostXml(uri, headers, signedInvoice);

        return (response, statusCode);
    }

    public static async Task<(string?, HttpStatusCode)> Cancel(XmlDocument signedRequest, OAuth2TokenService oAuth2Token)
    {
        string endpoint = "api/v1/cancelamento/notas/cancela";
        Dictionary<string, string> headers = new()
        {
            { "Content-Type", "application/xml" },
            { "Token", oAuth2Token.Token },
        };
        Uri uri = new($"{BASE_URL}/{endpoint}");
        
        return await HttpService.PostXml(uri, headers, signedRequest);
    }

    public static async Task<(string?, HttpStatusCode)> Validate(XmlDocument signedInvoice, OAuth2TokenService oAuth2Token)
    {
        string endpoint = "api/v1/processamento/notas/valida-processamento";
        Dictionary<string, string> headers = new()
        {
            { "Content-Type", "application/xml" },
            { "Token", oAuth2Token.Token },
        };
        Uri uri = new($"{BASE_URL}/{endpoint}");

        return await HttpService.PostXml(uri, headers, signedInvoice);
    }

    public static async Task<OAuth2TokenService?> RefreshToken(Credentials credentials)
    {
        string endpoint = "api/v1/autenticacao/oauth/token";

        Dictionary<string, string> queryParams = new()
            {
                { "grant_type", "password" },
                { "username", credentials.Login.User },
                { "password", credentials.Login.EncryptedPassword },
                { "client_id", credentials.Credential.ClientId },
                { "client_secret", credentials.Credential.ClientSecret }
            };
        string queryString = HttpService.GetQueryString(queryParams);

        Dictionary<string, string> headers = new()
        {
            { "Content-Type", "application/x-www-form-urlencoded" },
            { "User", credentials.Credential.ClientId },
            { "Password", credentials.Credential.ClientSecret }
        };

        Uri uri = new($"{BASE_URL}/{endpoint}");

        var (response, statusCode) = await HttpService.PostText(uri, headers, queryString);

        var jsonVar = JsonSerializer.Deserialize<OAuth2TokenResponse>(response)!;
        OAuth2TokenService result = new()
        {
            Token = jsonVar.access_token,
            RefreshToken = jsonVar.refresh_token,
            Expiration = jsonVar.expires_in,
        };

        return result;
    }


}

