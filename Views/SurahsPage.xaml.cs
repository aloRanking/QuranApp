

using System.Reactive.Linq;
using QuranApp.ViewModels;
using ReactiveUI.Maui;

namespace QuranApp.Views;



public partial class SurahsPage : ReactiveContentPage<SurahsViewModel>
{
    public SurahsPage(SurahsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = ViewModel = viewModel;
        BindingContext = viewModel;

       
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((SurahsViewModel)BindingContext).LoadSurahsCommand.Execute();
    }
}

    

