using System.Reactive.Linq;
using QuranApp.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace QuranApp.ViewModels;

public partial class ThemeViewModel : ReactiveObject
{
    private int _themeLevel;
    public int ThemeLevel
    {
        get => _themeLevel;
        set => this.RaiseAndSetIfChanged(ref _themeLevel, value);
    }

    private ThemePalette _currentTheme;
    public ThemePalette CurrentTheme
    {
        get => _currentTheme;
        private set => this.RaiseAndSetIfChanged(ref _currentTheme, value);
    }

    
   public ThemeViewModel()
    {
        // Start at level 0 (brightest)
        _themeLevel = 0;
        _currentTheme = ThemeSpectrum.Levels[0];

        // When ThemeLevel changes, update CurrentTheme + AppBar resources
        this.WhenAnyValue(vm => vm.ThemeLevel)
            .Select(level => ThemeSpectrum.Levels[level])
            .Subscribe(theme =>
            {
                CurrentTheme = theme;

                
                Application.Current.Resources["NavBarBackgroundColor"] = theme.Background;
                Application.Current.Resources["NavBarTextColor"] = theme.Text;
                Application.Current.Resources["PageBackgroundColor"] = theme.Background;
                Application.Current.Resources["PageTextColor"] = theme.Text;
                Application.Current.Resources["AccentColor"] = theme.Accent;
            });
    }
}