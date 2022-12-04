using System.Buffers;
using System.Globalization;
using System.Text;
using System.Web;

namespace Handlers;

public static class StringsHandler
{
    public static string FormatInvariant(string format, params object?[] args) => string.Format(CultureInfo.InvariantCulture, format, args);

    public static string FormatInvariant(int val) => val.ToString(CultureInfo.InvariantCulture);

    public static string FormatInvariant(decimal val, string? format) => val.ToString(format, CultureInfo.InvariantCulture);

    public static bool Equals(string s1, string s2)
    {
        if (s1 == s2)
        {
            return true;
        }

        if (s1 is null)
        {
            return s2?.Length == 0;
        }

        return s1.Equals(s2, System.StringComparison.InvariantCultureIgnoreCase);
    }

    public static string? Coalesce(string? str1, string? str2, string? def = null)
    {
        if (!string.IsNullOrWhiteSpace(str1))
        {
            return str1;
        }

        if (!string.IsNullOrWhiteSpace(str2))
        {
            return str2;
        }

        return def;
    }

    public static string? CoalesceAndTrim(string? str1, string? str2, string? def) => Coalesce(str1, str2, def)?.Trim();

    public static string ToLower(string? str) => str?.ToLowerInvariant() ?? String.Empty;

    public static string ToUpper(string? str) => str?.ToUpperInvariant() ?? String.Empty;

    public static bool ContainsOrdinalCI(string? item2, string? content) => (item2 ?? String.Empty).Contains(content ?? String.Empty, StringComparison.OrdinalIgnoreCase);

    public static (byte[], int) ToUtf8OnRentedBuffer(string str)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            return (Array.Empty<byte>(), 0);
        }

        int len = Encoding.UTF8.GetByteCount(str);
        byte[]? rented = ArrayPool<byte>.Shared.Rent(len);
        int utf8Len = Encoding.UTF8.GetBytes(str, rented);

        return (rented, utf8Len);
    }

    public static void ReturnRentedBuffer(byte[] buffer)
    {
        if (buffer == Array.Empty<byte>())
        {
            return;
        }

        ArrayPool<byte>.Shared.Return(buffer);
    }

    public static string ToUtf8Base64String(string str)
    {
        (byte[] buffer, int len) = ToUtf8OnRentedBuffer(str);

        try
        {
            return Convert.ToBase64String(buffer, 0, len);
        }
        finally
        {
            ReturnRentedBuffer(buffer);
        }
    }

    public static bool IsValidKey(string s) => !string.IsNullOrWhiteSpace(s) && s.All(c => char.IsDigit(c) || char.IsLetter(c) || c == '-');

    public static string RemoveAccents(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return String.Empty;
        }

        string? normalizedString = text.Normalize(NormalizationForm.FormD);
        StringBuilder stringBuilder = new();

        foreach (var c in normalizedString)
        {
            UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    public static string RemoveAccentsAndSpecialChars(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return String.Empty;
        }

        string? normalizedString = text.Normalize(NormalizationForm.FormD);
        StringBuilder stringBuilder = new();

        foreach (var c in normalizedString)
        {
            UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);

            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                if (char.IsLetterOrDigit(c))
                {
                    stringBuilder.Append(c);
                }
                else if (c == '*')
                {
                    stringBuilder.Append(c);
                }
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }
    public static string RemoveSpecialChars(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return String.Empty;
        }

        string? normalizedString = text.Normalize(NormalizationForm.FormD);
        StringBuilder stringBuilder = new();

        foreach (var c in normalizedString)
        {
            UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);

            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                if (char.IsLetterOrDigit(c))
                {
                    stringBuilder.Append(c);
                }
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }
    public static string RemoveSpecialCharacters(string? str, string? specialCharacter)
    {
        if (str is null)
        {
            return string.Empty;
        }

        if (string.IsNullOrWhiteSpace(specialCharacter))
        {
            return str;
        }

        if (str.Contains(specialCharacter, StringComparison.InvariantCulture))
        {
            return str.Replace(specialCharacter, string.Empty, StringComparison.InvariantCultureIgnoreCase);
        }

        return str;
    }

    public static string DecodeBase64(string encoded)
    {
        byte[] data = Convert.FromBase64String(encoded);
        string encodedUrl = Encoding.UTF8.GetString(data);
        string decoded = HttpUtility.UrlDecode(encodedUrl, Encoding.UTF8);

        return decoded;
    }

    public static byte[] ToByteArray(this string hex)
    {
        return Enumerable.Range(0, hex.Length)
            .Where(x => x % 2 == 0)
            .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
            .ToArray();
    }

    public static string ReplaceIC(this string? str, string oldString, string newString) =>
        (str ?? String.Empty).Replace(oldString, newString, StringComparison.InvariantCulture);
}