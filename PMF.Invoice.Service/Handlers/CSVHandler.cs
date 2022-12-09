using System.Globalization;

using Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace Handlers;

public static class CSVHandler
{
    private readonly static CsvConfiguration config = new(CultureInfo.InvariantCulture)
    {
        IgnoreBlankLines = true,
    };

    public static List<T> Read<T>(string filepath)
    {
        List<T> data = new();

        using (StreamReader streamReader = new(filepath))
        using (CsvReader csvReader = new(streamReader, config))
        {
            data = csvReader.GetRecords<T>().ToList();
        }

        return data;
    }
}
