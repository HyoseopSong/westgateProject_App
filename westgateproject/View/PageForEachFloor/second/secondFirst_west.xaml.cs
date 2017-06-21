﻿﻿using System;
using westgateproject.Models;
using Xamarin.Forms;

namespace westgateproject.View.PageForEachFloor.second
{
    public partial class secondFirst_west : ContentPage
	{
        public secondFirst_west()
        {
			InitializeComponent();
			zoomContainer.Content.AnchorX = 0;
			zoomContainer.Content.AnchorY = 0;
			zoomContainer.min_ty = 0;
			switch (Device.RuntimePlatform)
			{
				case Device.Android:
					zoomContainer.Content.Scale = (App.ScreenHeight - 90) / 263.5;
					break;
				default:
					zoomContainer.Content.Scale = (App.ScreenHeight - 70) / 263.5;
					break;
			}
			zoomContainer.maxScale = zoomContainer.Content.Scale;
			zoomContainer.minScale = zoomContainer.Content.Scale;
			zoomContainer.min_tx = -((500 * zoomContainer.Content.Scale) - App.ScreenWidth);
			NavigationPage.SetHasBackButton(this, false);
		}
        async void OnTapped(object sender, EventArgs args)
		{
			ShopInformation infoFromSQLite = new ShopInformation();
			var temp = sender as Label;
			infoFromSQLite = await App.Database.GetShopAsync("2지구", "1층", temp.Text);
			await Navigation.PushAsync(new ShopInfoPage(infoFromSQLite));
        }
		async void OnTappedEast(object sender, EventArgs args)
		{
			await Navigation.PushAsync(new secondFirst_east());
			Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);

		}
		async void goBack(object sender, EventArgs args)
		{
			await Navigation.PopAsync();
		}
    }
}
