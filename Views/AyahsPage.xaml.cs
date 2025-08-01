using System.Reactive.Linq;
using QuranApp.ViewModels;
using ReactiveUI.Maui;

namespace QuranApp.Views;



public partial class AyahsPage : ReactiveContentPage<AyahsViewModel>
{
    public AyahsPage(AyahsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = ViewModel = viewModel;
    }

    
}