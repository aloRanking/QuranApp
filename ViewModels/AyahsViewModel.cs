using System.Collections.ObjectModel;
using System.Reactive;
using QuranApp;
using QuranApp.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using System;
using Splat;
using System.Diagnostics;

namespace QuranApp.ViewModels;


[QueryProperty(nameof(Surah), "Surah")]
public class AyahsViewModel : ReactiveObject, IRoutableViewModel
{
    private readonly IQuranService _quranService;
    private bool _hasLoadedAyahs = false;
    private Surah _surah;

    public Surah Surah
    {
        get => _surah;
        set
        {

            if (EqualityComparer<Surah>.Default.Equals(_surah, value))
            {
                return;
            }

            // Now, set the backing field and raise the property changed notification.
            // We don't capture the return value as it's the 'value' itself, not a bool.
            this.RaiseAndSetIfChanged(ref _surah, value);

            // If Surah is now set (not null) and we haven't loaded ayahs for this Surah yet,
            // invoke the command.
            if (_surah != null && !_hasLoadedAyahs)
            {

                LoadAyahsCommand.Execute(Unit.Default).Subscribe();
                _hasLoadedAyahs = true; // Set flag to prevent re-execution for this Surah
            }
        }
    }

    [Reactive]
    public ObservableCollection<Ayah> Ayahs { get; set; } = new();

    [Reactive]
    public bool IsLoading { get; set; }
    [Reactive]
    public bool GridVisibitily { get; set; }


    public ReactiveCommand<Unit, Unit> LoadAyahsCommand { get; }

    public string UrlPathSegment => Surah?.English != null ? $"Surah {Surah.English}" : "Ayahs";


    public IScreen HostScreen { get; }

    public AyahsViewModel(IQuranService quranService, IScreen screen = null)
    {
        _quranService = quranService;

        HostScreen = screen ?? Locator.Current.GetService<IScreen>();


        LoadAyahsCommand = ReactiveCommand.CreateFromTask(LoadAyahsAsync);



        LoadAyahsCommand.ThrownExceptions
            .Subscribe(ex => Console.WriteLine($"Error loading Ayahs: {ex.Message}"));

    }

    private async Task LoadAyahsAsync()
    {
        if (Surah == null)
        {
            Console.WriteLine("Surah object is null. Cannot load Ayahs.");
            return;
        }

        IsLoading = true;
        GridVisibitily = false;
        Ayahs.Clear();
        await Task.Delay(10);
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        Console.WriteLine("LOADING!!!!!!!!!!!!.");

        try
        {



            var fetchTasks = new List<Task<Ayah>>();
            for (int i = 1; i <= Surah.AyahCount; i++)
            {
                fetchTasks.Add(_quranService.GetAyahAsync(Surah.Id, i));
            }


            var fetchedAyahs = await Task.WhenAll(fetchTasks);


            foreach (var ayah in fetchedAyahs.Where(a => a != null))
            {
                Ayahs.Add(ayah);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading ayahs {ex.Message}");

        }
        finally
        {
            IsLoading = false;
            GridVisibitily = true;

            stopwatch.Stop();
            Console.WriteLine($"Ayahs loaded in: {stopwatch.ElapsedMilliseconds} ms");

        }
    }




}
