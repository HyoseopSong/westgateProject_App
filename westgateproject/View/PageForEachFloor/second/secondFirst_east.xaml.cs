﻿using System;
using westgateproject.Models;
using Xamarin.Forms;

namespace westgateproject.View.PageForEachFloor.second
{
    public partial class secondFirst_east : ContentPage
    {
        public secondFirst_east()
        {
			InitializeComponent();
			zoomContainer.Content.AnchorX = 0;
			zoomContainer.Content.AnchorY = 0;

			zoomContainer.min_ty = 0;
			switch (Device.RuntimePlatform)
			{
				case Device.Android:
					zoomContainer.Content.Scale = (App.ScreenHeight - 90) / 265;
					break;
				default:
					zoomContainer.Content.Scale = (App.ScreenHeight - 70) / 265;
					break;
			}
			zoomContainer.maxScale = zoomContainer.Content.Scale;
			zoomContainer.minScale = zoomContainer.Content.Scale;
			zoomContainer.min_tx = -((475 * zoomContainer.Content.Scale) - App.ScreenWidth);
			NavigationPage.SetHasBackButton(this, false);
		}
		async void OnTapped(object sender, EventArgs args)
		{
            ShopInformation infoFromSQLite = new ShopInformation();
            var temp = sender as Label;
			infoFromSQLite = await App.Database.GetShopAsync("2지구", "1층", temp.Text);
            await Navigation.PushAsync(new ShopInfoPage(infoFromSQLite));
        }

		async void OnTappedWest(object sender, EventArgs args)
		{
            await Navigation.PushAsync(new secondFirst_west()); 
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count-2]);
		}
		async void goBack(object sender, EventArgs args)
		{
			await Navigation.PopAsync();
		}
    }
}
