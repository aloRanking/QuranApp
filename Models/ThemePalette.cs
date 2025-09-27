
namespace QuranApp.Models;

public class ThemePalette
{
    public Color Background { get; set; }
    public Color Text { get; set; }
    public Color Accent { get; set; }
    
    
     // Blend two palettes together
    public static ThemePalette Lerp(ThemePalette a, ThemePalette b, double t)
    {
        return new ThemePalette
        {
            Background = LerpColor(a.Background, b.Background, t),
            Text = LerpColor(a.Text, b.Text, t),
            Accent = LerpColor(a.Accent, b.Accent, t)
        };
    }

    private static Color LerpColor(Color a, Color b, double t)
    {
        return Color.FromRgba(
            (byte)(a.Red   + (b.Red   - a.Red)   * t),
            (byte)(a.Green + (b.Green - a.Green) * t),
            (byte)(a.Blue  + (b.Blue  - a.Blue)  * t),
            (byte)(a.Alpha + (b.Alpha - a.Alpha) * t)
        );
    }
}