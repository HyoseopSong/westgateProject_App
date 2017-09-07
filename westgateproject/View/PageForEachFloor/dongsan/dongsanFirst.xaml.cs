﻿using System;
using System.Collections.Generic;
using westgateproject.Models;
using Xamarin.Forms;

namespace westgateproject.View.PageForEachFloor.dongsan
{
    public partial class dongsanFirst : ContentPage
	{
		public bool onProcessing;
		bool backTouched;
		protected override bool OnBackButtonPressed()
		{
			if (!backTouched)
			{
				backTouched = true;
				Navigation.PopAsync();
			}
			return true;
		}
        public dongsanFirst()
		{
			InitializeComponent();
            backTouched = false;
			absL.AnchorX = 0;
			absL.AnchorY = 0;
			switch (Device.RuntimePlatform)
			{
				case Device.Android:
					absL.Scale = (App.ScreenHeight - 90) / 279;
					break;
				default:
					absL.Scale = (App.ScreenHeight - 70) / 279;
					break;
			}
			//zoomContainer.maxScale = zoomContainer.Content.Scale;
			//zoomContainer.minScale = zoomContainer.Content.Scale;
			//zoomContainer.min_tx = -((619 * zoomContainer.Content.Scale) - App.ScreenWidth);
			NavigationPage.SetHasBackButton(this, false);

			var boundaryBox = new BoxView { Color = Color.Red };
			AbsoluteLayout.SetLayoutBounds(boundaryBox, new Rectangle(619 * absL.Scale, App.ScreenWidth, 0, 30));
			absL.Children.Add(boundaryBox);
			onProcessing = false;
		}

		async void OnTapped(object sender, EventArgs args)
		{
			if (!onProcessing)
			{
				onProcessing = true;
	            ShopInformation infoFromSQLite = new ShopInformation();
	            var temp = sender as Label;
				//infoFromSQLite = await App.Database.GetShopAsync("동산상가", "1층", temp.Text);
				await Navigation.PushAsync(new ShopInfoPage("동산상가", "1층", temp.Text));
				//await Navigation.PushAsync(new ShopInfoPage(infoFromSQLite));
				onProcessing = false;
			}
		}
		async void goBack(object sender, EventArgs args)
		{
			await Navigation.PopAsync();
		}
    }
}
