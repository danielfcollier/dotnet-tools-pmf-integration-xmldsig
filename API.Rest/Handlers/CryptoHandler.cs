using System.Text;
using System.Security.Cryptography;

namespace Handlers;

public static class CryptoHandler
{

    public static string Md5Converter(string input)
    {
        var md5 = MD5.Create();

        var inputBytes = Encoding.ASCII.GetBytes(input);
        var hashBytes = md5.ComputeHash(inputBytes);

        return Convert.ToHexString(hashBytes);
    }
}