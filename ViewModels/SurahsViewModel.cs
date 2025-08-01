using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reactive;

using QuranApp.Models;
using QuranApp.Views;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace QuranApp.ViewModels;


public partial class SurahsViewModel : ReactiveObject
{
    private readonly IQuranService _quranService;

    [Reactive]
    public ObservableCollection<Surah> Surahs { get; set; } = new();

    [Reactive]
    public bool IsLoading { get; set; } = false;

    public ReactiveCommand<Unit, Unit> LoadSurahsCommand { get; }
    public ReactiveCommand<Surah, Unit> SurahSelectedCommand { get; }





    public SurahsViewModel(IQuranService quranService)
    {
        _quranService = quranService;
        LoadSurahsCommand = ReactiveCommand.CreateFromTask(LoadSurahsAsync);
        SurahSelectedCommand = ReactiveCommand.CreateFromTask<Surah>(NavigateToAyahsAsync);

    }




    private async Task LoadSurahsAsync()
    {
        try
        {
            IsLoading = true;
            var data = await _quranService.GetSurahsAsync();
            if (data?.Surahs != null)
            {
                Surahs.Clear();
                foreach (var surah in data.Surahs)
                {
                    Surahs.Add(surah);
                }
            }
            else
            {

                Debug.WriteLine("No surah data was loaded");

                await Shell.Current.DisplayAlert("Error", "Could not load surah data", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading surahs: {ex.Message}");
            await Shell.Current.DisplayAlert("Error", "Failed to load surahs", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task NavigateToAyahsAsync(Surah surah)
    {
        var parameters = new Dictionary<string, object>
        {
            { "Surah", surah }
        };

        await Shell.Current.GoToAsync(nameof(AyahsPage), parameters);
    }
}