using System.Text.Json;
using QuranApp.Models;



public class QuranService : IQuranService
{


JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true
        };

    public async Task<QuranData> GetSurahsAsync()
    {

        try
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("surahs.json");
            if (stream == null) return new QuranData(); // Return empty instead of null

            using var reader = new StreamReader(stream);
            var contents = await reader.ReadToEndAsync();
            return JsonSerializer.Deserialize<QuranData>(contents, options) ?? new QuranData();
        }
        catch
        {
            return new QuranData(); 
        }
    }

    public async Task<Ayah> GetAyahAsync(int surahNumber, int ayahNumber)
    {
        var fileName = $"{surahNumber}-{ayahNumber}.json";

        try
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
            using var reader = new StreamReader(stream);
            var contents = await reader.ReadToEndAsync();
            return JsonSerializer.Deserialize<Ayah>(contents, options);
        }
        catch (FileNotFoundException)
        {
            return new Ayah(); 
        }
    }
}