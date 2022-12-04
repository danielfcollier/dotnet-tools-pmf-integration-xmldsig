using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Xml;

using Handlers;

namespace Services;

public static class HttpService
{
    private static readonly HttpClient client = new();
    private static readonly HttpClient _client = GetService();

    public static HttpClient GetService()
    {
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.ConnectionClose = false;
        client.DefaultRequestHeaders.Add("Connection", "keep-alive");
        client.DefaultRequestHeaders.Add("Keep-Alive", "3600");
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

        return client;
    }

    public static async Task<(T?, HttpStatusCode)> GetRequest<T>(
        Uri uri,
        Dictionary<string, string>? headers,
        object? data
    ) => await JsonRequest<T>(uri, HttpMethod.Get, headers, data);

    public static async Task<(T?, HttpStatusCode)> PostRequest<T>(
        Uri uri,
        Dictionary<string, string>? headers,
        object? data
    ) => await JsonRequest<T>(uri, HttpMethod.Post, headers, data);

    public static async Task<(T?, HttpStatusCode)> PostText<T>(
        Uri uri,
        Dictionary<string, string>? headers,
        string? data
    ) => await TextRequest<T>(uri, HttpMethod.Post, headers, data);

    public static async Task<(string?, HttpStatusCode)> PostXml(
        Uri uri,
        Dictionary<string, string>? headers,
        XmlDocument? data
    ) => await XmlRequest(uri, HttpMethod.Post, headers, data);

    public static string GetQueryString(Dictionary<string, string> queryParams)
    {
        List<string> list = new();
        foreach (var item in queryParams)
        {
            list.Add($"{item.Key}={item.Value}");
        }

        string queryString = string.Join("&", list);

        return queryString;
    }

    private static async Task<(T?, HttpStatusCode)> JsonRequest<T>(
        Uri uri,
        HttpMethod method,
        Dictionary<string, string>? headers,
        object? data
    )
    {
        HttpRequestMessage request = BuildJsonRequest(uri, method, headers, data);
        HttpResponseMessage? response = await _client.SendAsync(request);

        string? result = await response.Content.ReadAsStringAsync();
        T? output = JsonSerializer.Deserialize<T>(result);

        return (output, response?.StatusCode ?? HttpStatusCode.BadRequest);
    }


    private static HttpRequestMessage BuildJsonRequest(
        Uri uri,
        HttpMethod method,
        Dictionary<string, string>? headers,
        object? data
    )
    {
        string payload = JsonSerializer.Serialize(data);
        string? contentType = GetContentType(headers);
        StringContent content = new(payload, Encoding.UTF8, contentType);

        HttpRequestMessage request = new(method, uri);
        SetAuthorizationHeader(headers, request);
        SetHeaders(headers, request);
        SetContent(content, request);

        return request;
    }

    private static async Task<(T?, HttpStatusCode)> TextRequest<T>(
        Uri uri,
        HttpMethod method,
        Dictionary<string, string>? headers,
        string? data
    )
    {
        HttpRequestMessage request = BuildTextRequest(uri, method, headers, data);
        Console.WriteLine(await request.Content!.ReadAsStringAsync());
        HttpResponseMessage? response = await _client.SendAsync(request);

        string? result = await response.Content.ReadAsStringAsync();
        T? output = JsonSerializer.Deserialize<T>(result);

        return (output, response?.StatusCode ?? HttpStatusCode.BadRequest);
    }

    private static HttpRequestMessage BuildTextRequest(
        Uri uri,
        HttpMethod method,
        Dictionary<string, string>? headers,
        string? data
    )
    {
        string payload = data is not null ? data : String.Empty;
        string? contentType = GetContentType(headers);
        StringContent content = new(payload, Encoding.UTF8, contentType);

        HttpRequestMessage request = new(method, uri);
        SetAuthorizationHeader(headers, request);
        SetHeaders(headers, request);
        SetContent(content, request);

        return request;
    }

    private static async Task<(string?, HttpStatusCode)> XmlRequest(
        Uri uri,
        HttpMethod method,
        Dictionary<string, string>? headers,
        XmlDocument? data
    )
    {
        HttpRequestMessage request = BuildXmlRequest(uri, method, headers, data);
        HttpResponseMessage? response = await _client.SendAsync(request);

        string? result = await response.Content.ReadAsStringAsync();

        return (result, response?.StatusCode ?? HttpStatusCode.BadRequest);
    }

    private static HttpRequestMessage BuildXmlRequest(
        Uri uri,
        HttpMethod method,
        Dictionary<string, string>? headers,
        XmlDocument? data
    )
    {
        string payload = data is not null ? data.OuterXml : String.Empty;
        string? contentType = GetContentType(headers);
        StringContent content = new(payload, Encoding.UTF8, contentType);

        HttpRequestMessage request = new(method, uri);
        SetAuthorizationHeader(headers, request);
        SetHeaders(headers, request);
        SetContent(content, request);

        return request;
    }

    private static void SetHeaders(Dictionary<string, string>? headers, HttpRequestMessage req)
    {
        if (headers is null)
        {
            return;
        }

        foreach (var header in headers)
        {
            req.Headers.Add(header.Key, header.Value);
        }
    }

    private static string? GetContentType(Dictionary<string, string>? headers)
    {
        if (headers!.TryGetValue("Content-Type", out string? contentType))
        {
            headers.Remove("Content-Type");
            return contentType;
        }

        return "text/plain";
    }

    private static void SetContent(StringContent content, HttpRequestMessage req)
    {
        req.Content = content;
    }

    private static void SetAuthorizationHeader(Dictionary<string, string>? headers, HttpRequestMessage req)
    {
        if (headers is null)
        {
            return;
        }

        if (headers.TryGetValue("User", out string? user))
        {
            headers.Remove("User");
        }

        if (headers.TryGetValue("Password", out string? password))
        {
            headers.Remove("Password");
        }

        if (headers.TryGetValue("Token", out string? token))
        {
            headers.Remove("Token");
        }

        if (user is not null && password is not null)
        {
            req.Headers.Authorization = new AuthenticationHeaderValue("Basic", StringsHandler.ToUtf8Base64String($"{user}:{password}"));
            return;
        }
        if (token is not null)
        {
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return;
        }
    }
}
