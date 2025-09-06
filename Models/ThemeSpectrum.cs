namespace QuranApp.Models;

public static class ThemeSpectrum
{
    public static readonly List<ThemePalette> Levels = new()
    {
        new ThemePalette { Background = Colors.White,      Text = Colors.Black,   Accent = Colors.DeepSkyBlue }, // Level 0 (brightest)
        new ThemePalette { Background = Color.FromArgb("#F2F2F2"), Text = Colors.Black,   Accent = Colors.SkyBlue },
        new ThemePalette { Background = Color.FromArgb("#E6E6E6"), Text = Colors.Black,   Accent = Colors.CornflowerBlue },
        new ThemePalette { Background = Color.FromArgb("#D9D9D9"), Text = Colors.Black,   Accent = Colors.SteelBlue },
        new ThemePalette { Background = Color.FromArgb("#CCCCCC"), Text = Colors.Black,   Accent = Colors.SlateBlue },
        new ThemePalette { Background = Color.FromArgb("#BFBFBF"), Text = Colors.Black,   Accent = Colors.DarkSlateBlue },
        new ThemePalette { Background = Color.FromArgb("#A6A6A6"), Text = Colors.White,   Accent = Colors.MediumPurple }, // midpoint
        new ThemePalette { Background = Color.FromArgb("#8C8C8C"), Text = Colors.White,   Accent = Colors.MediumSlateBlue },
        new ThemePalette { Background = Color.FromArgb("#737373"), Text = Colors.White,   Accent = Colors.RoyalBlue },
        new ThemePalette { Background = Color.FromArgb("#595959"), Text = Colors.White,   Accent = Colors.DodgerBlue },
        new ThemePalette { Background = Color.FromArgb("#404040"), Text = Colors.White,   Accent = Colors.LightSteelBlue },
        new ThemePalette { Background = Colors.Black,      Text = Colors.White,   Accent = Colors.LightGray } // Level 11 (darkest)
    };
}