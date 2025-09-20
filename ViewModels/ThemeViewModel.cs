using System.Reactive.Linq;
using QuranApp.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace QuranApp.ViewModels;

using System;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Timers;

public partial class ThemeViewModel : ReactiveObject
{
    private readonly Timer _themeTimer;


    
    private int _themeLevel;
[Reactive]
public int ThemeLevel
{
    get => _themeLevel;
    set
    {
        var clamped = Math.Clamp(value, 0, ThemeSpectrum.Levels.Count - 1);
        this.RaiseAndSetIfChanged(ref _themeLevel, clamped);
    }
}
    [Reactive] public ThemePalette CurrentTheme { get; private set; }

   
    private IDisposable? _autoThemeTimer;

 [Reactive] public bool IsAutoTheme { get; set; } = true;
   


    public ThemeViewModel()
    {
        // Init
        ThemeLevel = 0;
        CurrentTheme = ThemeSpectrum.Levels[0];

        // Apply theme on ThemeLevel change
        this.WhenAnyValue(vm => vm.ThemeLevel)
            .Select(level => ThemeSpectrum.Levels[level])
            .Subscribe(theme =>
            {
                CurrentTheme = theme;

                Application.Current.Resources["NavBarBackgroundColor"] = theme.Background;
                Application.Current.Resources["NavBarTextColor"] = theme.Text;
                Application.Current.Resources["PageBackgroundColor"] = theme.Background;
                Application.Current.Resources["PageTextColor"] = theme.Text;
            },
            ex =>
        {
            System.Diagnostics.Debug.WriteLine($"Theme pipeline error: {ex}");
        }

            );
            
            //StartAutoThemeSimulation();

        //Timer setup (check every 30 min)
        _themeTimer = new Timer(TimeSpan.FromMinutes(30).TotalMilliseconds);
        _themeTimer.Elapsed += (s, e) => UpdateThemeFromTime();

        // React to IsAutoTheme changes
        this.WhenAnyValue(vm => vm.IsAutoTheme)
            .Subscribe(isAuto =>
            {
                if (isAuto)
                {
                    UpdateThemeFromTime(); 
                    _themeTimer.Start();
                }
                else
                {
                    _themeTimer.Stop(); 
                }
            },
            ex =>
            {
                System.Diagnostics.Debug.WriteLine($"IsAutoTheme pipeline error: {ex}");
            });
    }

    private void UpdateThemeFromTime()
{
    var now = DateTime.Now;
    double hour = now.Hour + (now.Minute / 60.0); 
    Console.WriteLine($"Current hour: {hour}");

    if (hour >= 6 && hour < 17)
    {
        // Daytime (bright)
        ThemeLevel = 0;
    }
    else if (hour >= 17 && hour < 24)
    {
        // Evening → Night (fade 0 → 11)
        double fraction = (hour - 17) / (24 - 17); // 0 at 17h, 1 at 24h
        int level = (int)Math.Round(fraction * (ThemeSpectrum.Levels.Count - 1));
        ThemeLevel = Math.Clamp(level, 3, ThemeSpectrum.Levels.Count - 1);
    }
    else
    {
        // Midnight – 6 AM → darkest
        ThemeLevel = ThemeSpectrum.Levels.Count - 1;
    }
}
        private void StartAutoThemeSimulation()
    {
        
        _autoThemeTimer?.Dispose();

       
       _autoThemeTimer = Observable
        .Interval(TimeSpan.FromSeconds(50))
        .StartWith(0L) //start immediately
        .ObserveOn(RxApp.MainThreadScheduler) 
        .Subscribe(_ =>
        {
                var now = DateTime.Now;

                // Simulate: 0–59 minutes = full day cycle
                int minute = now.Minute;

                System.Console.WriteLine($"Simulated minute: {minute}");

                // Divide 60 minutes into 12 theme levels
                int level = minute / 5; // 5 minutes = 1 theme step
                level = Math.Clamp(level, 0, ThemeSpectrum.Levels.Count - 1);

                ThemeLevel = level;
            });
    }
    }
