using System.Net;
using System.Xml;
using System.Text.Json;

using Handlers;
using Models;

namespace Services;

public class InvoiceService
{
    private readonly string encryptedPassword;
    private readonly Credentials credentials;
    private OAuth2Token? oAuth2Token;
    private readonly string BASE_URL;

    public InvoiceService(Partner partner)
    {
        bool IS_PROD = false;

        credentials = partner.Secrets;
        encryptedPassword = CryptoHandler.Md5Converter(partner.Secrets.Login.Password)!;
        BASE_URL = IS_PROD ? "https://nfps-e.pmf.sc.gov.br" : "https://nfps-e-hml.pmf.sc.gov.br";
    }

    public async Task<(string?, HttpStatusCode)> Request(XmlDocument signedInvoice)
    {
        oAuth2Token = await RefreshToken();

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

    public async Task<(string?, HttpStatusCode)> Cancel(XmlDocument signedRequest)
    {
        oAuth2Token = await RefreshToken();

        string endpoint = "api/v1/cancelamento/notas/cancela";
        Dictionary<string, string> headers = new()
        {
            { "Content-Type", "application/xml" },
            { "Token", oAuth2Token.Token },
        };
        Uri uri = new($"{BASE_URL}/{endpoint}");

        return await HttpService.PostXml(uri, headers, signedRequest);
    }

    public async Task<(string?, HttpStatusCode)> Validate(XmlDocument signedInvoice)
    {
        oAuth2Token = await RefreshToken();

        string endpoint = "api/v1/processamento/notas/valida-processamento";
        Dictionary<string, string> headers = new()
        {
            { "Content-Type", "application/xml" },
            { "Token", oAuth2Token.Token },
        };
        Uri uri = new($"{BASE_URL}/{endpoint}");

        return await HttpService.PostXml(uri, headers, signedInvoice);
    }

    public async Task<OAuth2Token> RefreshToken()
    {
        // TODO: rule to only request a new token if the current token is invalid

        string endpoint = "api/v1/autenticacao/oauth/token";

        Dictionary<string, string> queryParams = new()
        {
            { "grant_type", "password" },
            { "username", credentials.Login.User },
            { "password", encryptedPassword },
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

        (OAuth2Token? response, HttpStatusCode statusCode) = await HttpService.PostText<OAuth2Token>(uri, headers, queryString);

        if (response is null)
        {
            throw new Exception("OAuth2Token can not be null!");
        }

        return response;
    }

    public static (decimal, decimal, decimal) GetTotals(List<InvoiceRequest.ItemServico> itensServico)
    {
        decimal baseCalculo = itensServico.ToArray().Select(itemServico => itemServico.BaseCalculo).Sum();
        decimal valorIssqn = Math.Round(itensServico.ToArray().Select(itemServico => itemServico.BaseCalculo * itemServico.Aliquota).Sum(), 2, MidpointRounding.AwayFromZero);
        decimal valorTotalServicos = itensServico.ToArray().Select(itemServico => itemServico.ValorTotal).Sum();

        return (baseCalculo, valorIssqn, valorTotalServicos);
    }

    public static List<InvoiceRequest.ItemServico> GetItensServico(Partner partner, Customer customer)
    {
        List<InvoiceRequest.ItemServico> itensServico = new();

        string jsonString = StringsHandler.DecodeBase64WithAccents(customer.Services);
        List<CustomerServices>? services = JsonSerializer.Deserialize<List<CustomerServices>>(jsonString);
        if (services is null)
        {
            throw new Exception("Customer Services array can not be null!");
        }

        foreach (var service in services)
        {
            Partner.Service? serviceInfo = partner.Services.Where(s => s.Id == service.Id).First();
            if (serviceInfo is null)
            {
                throw new Exception("Service Info can not be null!");
            }

            itensServico.Add(
                new()
                {
                    Aliquota = serviceInfo.Aliquota,
                    BaseCalculo = (decimal)(serviceInfo.BaseCalculoFactor * service.Value),
                    Cst = serviceInfo.Cst,
                    DescricaoServico = service.Description,
                    IdCnae = serviceInfo.IdCnae,
                    Quantidade = service.Quantity,
                    ValorTotal = (decimal)(service.Quantity * service.Value),
                    ValorUnitario = service.Value,
                }
            );
        }

        return itensServico;
    }
}

