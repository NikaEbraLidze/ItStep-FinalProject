using System.Text.Json;

namespace Atm.DAL
{
    public class JsonFileManager
    {
        public static async Task<List<T>> LoadAsync<T>(string filePath) where T : class
        {
            if (!File.Exists(filePath))
                return new List<T>();

            try
            {
                using (FileStream fs = File.OpenRead(filePath))
                {
                    var data = await JsonSerializer.DeserializeAsync<List<T>>(fs, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return data ?? new List<T>();
                }
            }
            catch (JsonException)
            {
                return new List<T>();
            }
        }

        public static async Task SaveAsync<T>(string filePath, List<T> data) where T : class
        {
            using (FileStream fs = File.Create(filePath))
            {
                var option = new JsonSerializerOptions { WriteIndented = true };

                await JsonSerializer.SerializeAsync(fs, data, option);
            }
        }
    }
}