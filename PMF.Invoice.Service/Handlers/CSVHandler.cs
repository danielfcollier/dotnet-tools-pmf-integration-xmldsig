using System.IO;
using System.Net.Mime;
using System.Text;
using System.Globalization;

using Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace Handlers;

public static class CSVHandler
{
    private readonly static CsvConfiguration config = new(CultureInfo.InvariantCulture)
    {
        HeaderValidated = null,
        HasHeaderRecord = true,
        Delimiter = ",",
        AllowComments = true,
        IgnoreBlankLines = true,
        MissingFieldFound = null,
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

    public static void Write<T>(string filepath, List<T> records)
    {
        using (StreamWriter streamWriter = new(filepath))
        using (CsvWriter csvWriter = new(streamWriter, config))
        {
            csvWriter.WriteHeader<T>();
            csvWriter.NextRecord();
            foreach (var record in records)
            {
                csvWriter.WriteRecord<T>(record);
                csvWriter.NextRecord();
            }
        }
    }
}
