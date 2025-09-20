namespace QuranApp.Models;

public class ThemeSpectrum
{
    public static readonly int TotalLevels = 12;

    public static readonly List<ThemePalette> Levels =
        Enumerable.Range(0, TotalLevels).Select(i =>
        {
            double t = i / (double)(TotalLevels - 1); // goes from 0 → 1

            // Smooth transition: White → Black
            var background = Lerp(Colors.White, Colors.Black, t);

             // Text: delayed fade black → white (to keep contrast)
        double textT = Math.Pow(t, 2.8); // nonlinear curve: stays black longer, switches later
        var text = Lerp(Colors.Black, Colors.White, textT);

            // Accent example: DeepSkyBlue → LightGray
            var accent = Lerp(Colors.DeepSkyBlue, Colors.LightGray, t);

            return new ThemePalette
            {
                Background = background,
                Text = text,
                Accent = accent
            };
        }).ToList();

    private static Color Lerp(Color from, Color to, double t)
    {
        return Color.FromRgba(
            (byte)(from.Red   * 255 + (to.Red   - from.Red)   * 255 * t),
            (byte)(from.Green * 255 + (to.Green - from.Green) * 255 * t),
            (byte)(from.Blue  * 255 + (to.Blue  - from.Blue)  * 255 * t),
            (byte)(from.Alpha * 255 + (to.Alpha - from.Alpha) * 255 * t)
        );
    }
}