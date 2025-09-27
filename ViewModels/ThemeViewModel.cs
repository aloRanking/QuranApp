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

                ApplyTheme(theme);
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

    ThemePalette target;

    if (hour >= 6 && hour < 17)
    {
        target = ThemeSpectrum.Levels[0]; // day bright
    }
    else if (hour >= 17 && hour <= 23)
    {
        int nightHour = hour - 17; // 0–6
        int index = 1 + (int)Math.Round((nightHour / 6.0) * 5); // 1–6
        target = ThemeSpectrum.Levels[index];
    }
    else
    {
        int lateHour = hour; // 0–5
        int index = 7 + (int)Math.Round((lateHour / 6.0) * 4); // 7–11
        target = ThemeSpectrum.Levels[index];
    }

    CurrentTheme = target;
    ApplyTheme(target); //
    }

    




    private void ApplyTheme(ThemePalette target)
    {
        var duration = 500u; // fade duration (ms)

        // Background
        if (Application.Current.Resources.TryGetValue("PageBackgroundColor", out var bgObj) && bgObj is Color oldBg)
        {
            AnimateColor(oldBg, target.Background, duration, c => Application.Current.Resources["NavBarBackgroundColor"] = c, "NavBarBgAnim");
       
            AnimateColor(oldBg, target.Background, duration, c => Application.Current.Resources["PageBackgroundColor"] = c, "PageBgAnim");
             }
        else
        {
            Application.Current.Resources["NavBarBackgroundColor"] = target.Background;
            Application.Current.Resources["PageBackgroundColor"] = target.Background;
            
        }

        // Text
        if (Application.Current.Resources.TryGetValue("PageTextColor", out var txtObj) && txtObj is Color oldTxt)
        {
            AnimateColor(oldTxt, target.Text, duration, c => Application.Current.Resources["NavBarTextColor"] = c, "NavBarTextAnim");
       
            AnimateColor(oldTxt, target.Text, duration, c => Application.Current.Resources["PageTextColor"] = c, "PageTextAnim");
             }
        else
        {
            Application.Current.Resources["NavBarTextColor"] = target.Text;
            Application.Current.Resources["PageTextColor"] = target.Text;
            
        }
        
   

    // Accent (optional instant or animate similarly)
        Application.Current.Resources["AccentColor"] = target.Accent;

 
    
    }

private void AnimateColor(Color from, Color to, uint duration, Action<Color> setter, string animationName)
{
   var owner = Application.Current?.MainPage as IAnimatable;
    if (owner == null)
    {
        MainThread.BeginInvokeOnMainThread(() => setter(to));
        return;
    }

    // Abort previous animation with the same name (prevents overlapping)
    owner.AbortAnimation(animationName);

    var animation = new Animation(v =>
    {
        var c = Color.FromRgba(
            from.Red   + (to.Red   - from.Red)   * v,
            from.Green + (to.Green - from.Green) * v,
            from.Blue  + (to.Blue  - from.Blue)  * v,
            from.Alpha + (to.Alpha - from.Alpha) * v
        );
        setter(c);
    }, 0, 1);

    // Commit on UI thread and use the page as animation owner
    MainThread.BeginInvokeOnMainThread(() =>
    {
        animation.Commit(owner, animationName, length: duration, easing: Easing.Linear);
    });}

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
