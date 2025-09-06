﻿using QuranApp.Views;

namespace QuranApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(SurahsPage), typeof(SurahsPage));
        Routing.RegisterRoute(nameof(AyahsPage), typeof(AyahsPage));
		Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
	}
}
