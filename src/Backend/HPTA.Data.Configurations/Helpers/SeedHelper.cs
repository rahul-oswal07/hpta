using Newtonsoft.Json;

namespace HPTA.Data.Configurations.Helpers;

public static class SeedHelper
{
    public static List<TEntity> SeedData<TEntity>(string fileName)
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string path = "../../../Seed";
        string fullPath = Path.Combine(currentDirectory, path, fileName);

        List<TEntity> result;
        using (StreamReader reader = new(fullPath))
        {
            string json = reader.ReadToEnd();
            result = JsonConvert.DeserializeObject<List<TEntity>>(json) ?? [];
        }

        return result;
    }
}
