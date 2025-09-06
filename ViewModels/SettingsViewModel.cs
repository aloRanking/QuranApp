using QuranApp.ViewModels;
using ReactiveUI;
namespace QuranApp.ViewModels;
public class SettingsViewModel : ReactiveObject
{
    public ThemeViewModel Theme { get; }

    public SettingsViewModel(ThemeViewModel theme)
    {
        Theme = theme;
    }
}