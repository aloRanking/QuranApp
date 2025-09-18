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
        var hour = now.Hour;
        System.Console.WriteLine($"Current hour: {hour}");

        if (hour >= 6 && hour < 17)
        {
            ThemeLevel = 0; // daytime bright
        }
        else
        {
            if (hour >= 17 && hour <= 23)
            {
                int nightHour = hour - 17; // 0–6
                ThemeLevel = 1 + (int)Math.Round((nightHour / 6.0) * 5); // 1–6
            }
            else
            {
                int lateHour = hour; // 0–5
                ThemeLevel = 7 + (int)Math.Round((lateHour / 6.0) * 4); // 7–11
            }
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
