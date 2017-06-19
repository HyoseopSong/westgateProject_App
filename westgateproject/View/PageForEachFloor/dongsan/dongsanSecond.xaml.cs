﻿﻿using System;
using System.Collections.Generic;
using westgateproject.Models;
using Xamarin.Forms;

namespace westgateproject.View.PageForEachFloor.dongsan
{
    public partial class dongsanSecond : ContentPage
    {
        public dongsanSecond()
        {
			InitializeComponent();
			zoomContainer.Content.AnchorX = 0;
			zoomContainer.Content.AnchorY = 0;

			zoomContainer.min_ty = 0;
			switch (Device.RuntimePlatform)
			{
				case Device.Android:
					zoomContainer.Content.Scale = (App.ScreenHeight - 90) / 290;
					break;
				default:
					zoomContainer.Content.Scale = (App.ScreenHeight - 70) / 290;
					break;
			}
			zoomContainer.maxScale = zoomContainer.Content.Scale;
			zoomContainer.minScale = zoomContainer.Content.Scale;
			zoomContainer.min_tx = -((620 * zoomContainer.Content.Scale) - App.ScreenWidth);
			NavigationPage.SetHasBackButton(this, false);
		}

		async void OnTapped(object sender, EventArgs args)
		{
			ShopInformation infoFromSQLite = new ShopInformation();
			var temp = sender as Label;
			infoFromSQLite = await App.Database.GetShopAsync("동산상가", "2층", temp.Text);
			await Navigation.PushAsync(new ShopInfoPage(infoFromSQLite));
		}
		async void goBack(object sender, EventArgs args)
		{
			await Navigation.PopAsync();
		}
    }
}
