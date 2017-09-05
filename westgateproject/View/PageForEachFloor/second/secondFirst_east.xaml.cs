﻿using System;
using westgateproject.Helper;
using westgateproject.Models;
using Xamarin.Forms;

namespace westgateproject.View.PageForEachFloor.second
{
    public partial class secondFirst_east : ContentPage
	{
		public bool onProcessing;
        public secondFirst_east()
        {
			InitializeComponent();
			absL.AnchorX = 0;
			absL.AnchorY = 0;
			switch (Device.RuntimePlatform)
			{
				case Device.Android:
					absL.Scale = (App.ScreenHeight - 90) / 265;
					break;
				default:
					absL.Scale = (App.ScreenHeight - 70) / 265;
					break;
			}
			NavigationPage.SetHasBackButton(this, false);
			onProcessing = false;

			var boundaryBox = new BoxView { Color = Color.Red };
			AbsoluteLayout.SetLayoutBounds(boundaryBox, new Rectangle(465 * absL.Scale, App.ScreenWidth, 0, 30));
			absL.Children.Add(boundaryBox);
		}

        protected async override void OnAppearing()
        {
            var MapInfo = await SyncData.DownloadShopMapInfo();


            foreach (var shopInfo in MapInfo)
            {
                Label shopLocation = new Label()
                {
                    Text = shopInfo.RowKey,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = shopInfo.FontSize
                };
                switch(shopInfo.BackgroundColor)
                {
                    case "Aqua":
                        shopLocation.BackgroundColor = Color.Aqua;
                        break;
                    default:
                        shopLocation.BackgroundColor = Color.Default;
                        break;
				}
				switch (shopInfo.TextColor)
				{
					case "Blue":
						shopLocation.TextColor = Color.Blue;
						break;
					default:
						shopLocation.TextColor = Color.Default;
						break;
				}

                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += OnTappedWest;
                shopLocation.GestureRecognizers.Add(tapGestureRecognizer);

                AbsoluteLayout.SetLayoutBounds(shopLocation, new Rectangle(shopInfo.XPosition, shopInfo.YPosition, shopInfo.Width, shopInfo.Height));
                absL.Children.Add(shopLocation);
            }
        }
		async void OnTapped(object sender, EventArgs args)
		{
			if (!onProcessing)
			{
				onProcessing = true;
	            ShopInformation infoFromSQLite = new ShopInformation();
	            var temp = sender as Label;
				//infoFromSQLite = await App.Database.GetShopAsync("2지구", "1층", temp.Text);
				await Navigation.PushAsync(new ShopInfoPage("2지구", "1층", temp.Text));
				onProcessing = false;
			}
        }

		async void OnTappedWest(object sender, EventArgs args)
		{
			//if (!onProcessing)
			//{
			//	onProcessing = true;
	  //          await Navigation.PushAsync(new secondFirst_west());
			//	Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
			//	onProcessing = false;
			//}
		}
		async void goBack(object sender, EventArgs args)
		{
			await Navigation.PopAsync();
		}
    }
}
