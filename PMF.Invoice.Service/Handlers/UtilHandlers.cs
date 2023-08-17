using System.Text.RegularExpressions;

namespace Handlers;

public static class UtilsHandler
{
    public static string MaskDocument(string documentId)
    {
        string cleanDocument = OnlyNumbers(documentId);

        return cleanDocument.PadLeft(11, '0');
    }

    public static string OnlyNumbers(string input)
    {
        Regex onlyNumbers = new(@"\D", RegexOptions.Compiled);

        return onlyNumbers.Replace(input, String.Empty);
    }
}
