namespace Utils;

public static class FileHandler
{
    public static async Task<string?> ReadAsync(string file, string relativePath)
    {
        var filepath = Path.Join(relativePath, file);
        var data = await File.ReadAllTextAsync(filepath).ConfigureAwait(false);

        return data;
    }
}