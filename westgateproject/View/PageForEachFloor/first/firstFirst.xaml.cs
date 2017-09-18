using System;
using System.Collections.Generic;
using westgateproject.Helper;
using westgateproject.Models;
using Xamarin.Forms;

namespace westgateproject.View.PageForEachFloor.first
{
    public partial class firstFirst : ContentPage
	{
		public bool onProcessing;
		bool backTouched;
        string mapInfoPartKey;

		protected override bool OnBackButtonPressed()
		{
			if (!backTouched)
			{
				backTouched = true;
				Navigation.PopAsync();
			}
			return true;
		}
        public firstFirst(string building, string floor)
        {
			InitializeComponent();
            backTouched = false;
			onProcessing = false;

            Title = floor;
            mapInfoPartKey = building + floor;

			absL.AnchorX = 0;
			absL.AnchorY = 0;
			switch (Device.RuntimePlatform)
			{
				case Device.Android:
					absL.Scale = (App.ScreenHeight - 90) / 280;
					break;
				default:
					absL.Scale = (App.ScreenHeight - 70) / 280;
					break;
			}

			var boundaryBox = new BoxView { Color = Color.Red };
			AbsoluteLayout.SetLayoutBounds(boundaryBox, new Rectangle(510 * absL.Scale, 280 * absL.Scale, 0, 1));
			absL.Children.Add(boundaryBox);
        }
		protected async override void OnAppearing()
		{
			var MapInfo = await SyncData.DownloadShopMapInfo("1지구1층");
			// var MapInfo = await SyncData.DownloadShopMapInfo(mapInfoPartKey);
			if(MapInfo == null)
            {
                await Navigation.PopAsync();
            }
			foreach (var shopInfo in MapInfo)
			{
				Label shopLocation = new Label()
				{
					Text = shopInfo.RowKey,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Center,
					FontSize = shopInfo.FontSize
				};
				switch (shopInfo.BackgroundColor)
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
					case "Transparent":
						shopLocation.TextColor = Color.Transparent;
						break;
					default:
						shopLocation.TextColor = Color.Default;
						break;
				}

				var tapGestureRecognizer = new TapGestureRecognizer();
				tapGestureRecognizer.Tapped += OnTapped;
				shopLocation.GestureRecognizers.Add(tapGestureRecognizer);

				AbsoluteLayout.SetLayoutBounds(shopLocation, new Rectangle(shopInfo.XPosition, shopInfo.YPosition, shopInfo.Width, shopInfo.Height));
				absL.Children.Add(shopLocation);
				loadingLabel.IsVisible = false;
			}


		}
		async void OnTapped(object sender, EventArgs args)
		{
			if (!onProcessing)
			{
				onProcessing = true;
				//ShopInforSQLDb infoFromSQLite = new ShopInforSQLDb();
				var temp = sender as Label;
				//infoFromSQLite = await App.Database.GetShopAsync("2지구", "1층", temp.Text);
				await Navigation.PushAsync(new ShopInfoPage("2지구", "1층", temp.Text));
				onProcessing = false;
			}
		}
    }
}
