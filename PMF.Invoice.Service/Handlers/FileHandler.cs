namespace Handlers;

public static class FileHandler
{
    public static async Task<string?> ReadAsync(string file, string relativePath)
    {
        string filepath = Path.Join(relativePath, file);
        string? data = await File.ReadAllTextAsync(filepath).ConfigureAwait(false);

        return data;
    }
}