﻿using System;
using System.Collections.Generic;
using westgateproject.Models;
using Xamarin.Forms;

namespace westgateproject.View.PageForEachFloor.fifth
{
    public partial class fifthSecond : ContentPage
	{
		public bool onProcessing;
        public fifthSecond()
        {
			InitializeComponent();
			absL.AnchorX = 0;
			absL.AnchorY = 0;
			switch (Device.RuntimePlatform)
			{
				case Device.Android:
					absL.Scale = (App.ScreenHeight - 90) / 324;
					break;
				default:
					absL.Scale = (App.ScreenHeight - 70) / 324;
					break;
			}
			NavigationPage.SetHasBackButton(this, false);
			onProcessing = false;

			var boundaryBox = new BoxView { Color = Color.Red };
			AbsoluteLayout.SetLayoutBounds(boundaryBox, new Rectangle(485 * absL.Scale, App.ScreenWidth, 0, 30));
			absL.Children.Add(boundaryBox);
        }

        async void OnTapped(object sender, EventArgs args)
		{
			if (!onProcessing)
			{
				onProcessing = true;
	            ShopInformation infoFromSQLite = new ShopInformation();
	            var temp = sender as Label;
	            infoFromSQLite = await App.Database.GetShopAsync("5지구", "2층", temp.Text);
				await Navigation.PushAsync(new ShopInfoPage(infoFromSQLite));
				onProcessing = false;
			}
		}
		async void goBack(object sender, EventArgs args)
		{
			await Navigation.PopAsync();
		}
    }
}
