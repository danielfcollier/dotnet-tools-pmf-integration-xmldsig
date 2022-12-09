using System.Text.Json;

using Models;

namespace Handlers;

public static class JsonHandler
{
    public static async Task<T?> ReadAll<T>(string filepath)
    {
        var content = await File.ReadAllTextAsync(filepath);

        return JsonSerializer.Deserialize<T>(content);
    }

    // public static async Task Reset()
    // {
    //     await File.WriteAllTextAsync(filepath, String.Empty);
    // }

    // public static async Task Create<T>(T element)
    // {
    //     T? data = await ReadAll();

    //     if (data is null)
    //     {
    //         data = new() { element };
    //     }
    //     else
    //     {
    //         data.Add(element);
    //     }

    //     await File.WriteAllTextAsync(filepath, JsonSerializer.Serialize(data));
    // }

    // public static async Task<T?> Read<T>(string id)
    // {
    //     var data = await ReadAll();

    //     if (data is null)
    //     {
    //         return null;
    //     }

    //     foreach (var element in data)
    //     {
    //         if (element.Id == id)
    //         {
    //             return element;
    //         }
    //     }

    //     return null;
    // }

    // private static async Task<T> Update<T>(T element, decimal amount)
    // {
    //     List<T>? data = await ReadAll();

    //     if (data is null)
    //     {
    //         throw new Exception();
    //     }

    //     T updatedElement = new();
    //     foreach (var content in data)
    //     {
    //         if (content.Id == element.Id)
    //         {
    //             content.Balance = element.Balance + amount;
    //             updatedElement.Id = content.Id;
    //             updatedElement.Balance = content.Balance;
    //         }
    //     }

    //     await File.WriteAllTextAsync(filepath, JsonSerializer.Serialize(data));

    //     return updatedElement;
    // }
}