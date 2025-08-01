using Microsoft.Extensions.Logging;
using QuranApp.Views;
using QuranApp.ViewModels;
using CommunityToolkit.Maui;

namespace QuranApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<IQuranService, QuranService>();
		builder.Services.AddTransient<SurahsViewModel>();
		builder.Services.AddTransient<SurahsPage>();
		builder.Services.AddTransient<AyahsViewModel>();
		builder.Services.AddTransient<AyahsPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
