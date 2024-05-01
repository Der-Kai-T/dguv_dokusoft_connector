using System.Text.Json;
using Dokusoft.Data.Models;

namespace Dokusoft.IO.File;

public static class JsonFileWriter
{
    private static JsonSerializerOptions Options = new ()
    {
        WriteIndented = true,
    };
    
    public static bool WriteToFile(DeviceModel model, string path)
    {
        if (!Directory.Exists(path))
            return false;

        var fileName = $"{Guid.NewGuid().ToString().Replace("-", "")}.txt";

        var totalPath = Path.Join(path, fileName);

        try
        {
            var file = System.IO.File.CreateText(totalPath);
            file.Write(CreateJson(model));
            file.Close();
            return true;
        }
        catch
        {
            return false;
        }

        return false;
    }

    private static string CreateJson(DeviceModel model) => JsonSerializer.Serialize(model, Options);
}