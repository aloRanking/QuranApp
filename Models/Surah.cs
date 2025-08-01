
namespace QuranApp.Models;

public class Surah
{
    public int Id { get; set; }
    public int AyahCount { get; set; }
    public string Anglicised { get; set; }
    public string Arabic { get; set; }
    public string English { get; set; }
}