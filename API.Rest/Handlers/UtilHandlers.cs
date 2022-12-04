using System.Text.RegularExpressions;

namespace Handlers;

public static class UtilsHandler
{
    public static string MaskPersonDocument(string documentId)
    {
        string cleanDocument = OnlyNumbers(documentId);

        return cleanDocument.PadLeft(11, '0');
    }

    public static string MaskDate(string date)
    {
        var temp = date.Split('/');
        var year = temp[2];
        var month = temp[1];
        var day = temp[0];

        return $"{year}-{month}-{day}";
    }

    public static string OnlyNumbers(string input)
    {
        Regex onlyNumbers = new(@"\D", RegexOptions.Compiled);

        return onlyNumbers.Replace(input, String.Empty);
    }
}
