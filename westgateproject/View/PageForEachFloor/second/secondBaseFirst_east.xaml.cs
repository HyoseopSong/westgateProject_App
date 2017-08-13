﻿using System;
using System.Collections.Generic;
using westgateproject.Models;
using westgateproject.View.PageForEachFloor.second;
using Xamarin.Forms;

namespace westgateproject.View.PageForEachFloor
{
    public partial class secondBaseFirst_east : ContentPage
	{
		public bool onProcessing;
        public secondBaseFirst_east()
        {
			InitializeComponent();
			absL.AnchorX = 0;
			absL.AnchorY = 0;
			switch (Device.RuntimePlatform)
			{
				case Device.Android:
					absL.Scale = (App.ScreenHeight - 90) / 254;
					break;
				default:
					absL.Scale = (App.ScreenHeight - 70) / 254;
					break;
			}
			NavigationPage.SetHasBackButton(this, false);
			onProcessing = false;

			var boundaryBox = new BoxView { Color = Color.Red };
			AbsoluteLayout.SetLayoutBounds(boundaryBox, new Rectangle(440 * absL.Scale, App.ScreenWidth, 0, 30));
			absL.Children.Add(boundaryBox);
		}
		async void OnTapped(object sender, EventArgs args)
		{
			if (!onProcessing)
			{
				onProcessing = true;
				ShopInformation infoFromSQLite = new ShopInformation();
				var temp = sender as Label;
				//infoFromSQLite = await App.Database.GetShopAsync("2지구", "지하1층", temp.Text);
				await Navigation.PushAsync(new ShopInfoPage("2지구", "지하1층", temp.Text));
				onProcessing = false;
			}
		}

		async void OnTappedWest(object sender, EventArgs args)
		{
			if (!onProcessing)
			{
				onProcessing = true;
			    await Navigation.PushAsync(new secondBaseFirst_west());
				Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
				onProcessing = false;
			}
		}

        async void goBack(object sender, EventArgs args)
        {
            await Navigation.PopAsync();
        }
    
    }
}
