using System.Text;
using System.Security.Cryptography;

namespace Handlers;

public static class CryptoHandler
{
    public static string Md5Converter(string input)
    {
        MD5 md5 = MD5.Create();

        byte[] inputBytes = Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        return Convert.ToHexString(hashBytes);
    }
}