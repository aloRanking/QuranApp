using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
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
    public ObservableCollection<Surah> FilteredSurahs { get; set; } = new();

    [Reactive]
    public bool IsLoading { get; set; } = false;

    [Reactive]
    public string SearchText { get; set; } = string.Empty;

    public ReactiveCommand<Unit, Unit> LoadSurahsCommand { get; }
    public ReactiveCommand<Surah, Unit> SurahSelectedCommand { get; }
    public ReactiveCommand<Surah, Unit> CancelSearchCommand { get; }

    public SurahsViewModel(IQuranService quranService)
    {
        _quranService = quranService;

        LoadSurahsCommand = ReactiveCommand.CreateFromTask(LoadSurahsAsync);
        SurahSelectedCommand = ReactiveCommand.CreateFromTask<Surah>(NavigateToAyahsAsync);
        CancelSearchCommand = ReactiveCommand.Create<Surah>(CancelSearch);

      
    


        // React to SearchText changes
        this.WhenAnyValue(x => x.SearchText)
            .Throttle(TimeSpan.FromMilliseconds(300)) // debounce typing
            .DistinctUntilChanged()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(FilterSurahs);
    }

    private void CancelSearch(Surah surah)
    {
        SearchText = string.Empty;
        FilterSurahs(string.Empty);
        //SearchEntry.Unfocus();
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
                FilteredSurahs.Clear();

                foreach (var surah in data.Surahs)
                {
                    Surahs.Add(surah);
                    FilteredSurahs.Add(surah); // initialize filtered list
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

    private void FilterSurahs(string search)
    {
        if (Surahs == null) return;

        FilteredSurahs.Clear();

        var query = string.IsNullOrWhiteSpace(search)
            ? Surahs
            : Surahs.Where(s =>
                   s.English.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                   s.Arabic.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                   s.Anglicised.Contains(search, StringComparison.OrdinalIgnoreCase));

        foreach (var surah in query)
            FilteredSurahs.Add(surah);
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