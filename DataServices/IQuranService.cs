using QuranApp.Models;

public interface IQuranService
{
    Task<QuranData> GetSurahsAsync();
    Task<Ayah> GetAyahAsync(int surahNumber, int ayahNumber);
}