public class Ayah
{
    public MetaData Meta { get; set; }
    public List<Word> Words { get; set; }
    public string FullArabic { get; set; }
    public string FullTranslation { get; set; }
    public string FullTransliteration { get; set; }
}

public class MetaData
{
    public int Ayah { get; set; }
    public int Surah { get; set; }
    public int Words { get; set; }
    public int NumberInQuran { get; set; }
    public int Juz { get; set; }
    public int Manzil { get; set; }
    public int Page { get; set; }
    public int Ruku { get; set; }
    public int HizbQuarter { get; set; }
    public bool ShouldSadjah { get; set; }
}

public class Word
{
    public string WordArabic { get; set; }
    public int WordAyah { get; set; }
    public int WordNumberInAyah { get; set; }
    public int WordNumberInQuran { get; set; }
    public int WordNumberInSurah { get; set; }
    public int WordSurah { get; set; }
    public string WordTranslation { get; set; }
    public string WordTransliteration { get; set; }
}