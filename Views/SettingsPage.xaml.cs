
using System.Reactive.Linq;
using QuranApp.ViewModels;
using ReactiveUI.Maui;

namespace QuranApp.Views;



public partial class  SettingsPage : ReactiveContentPage<SettingsViewModel>
{
    public SettingsPage(SettingsViewModel viewModel)
    {
       InitializeComponent();
       BindingContext = ViewModel = viewModel;

       
    }

    
}

    
