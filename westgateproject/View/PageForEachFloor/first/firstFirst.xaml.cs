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
		protected override bool OnBackButtonPressed()
		{
			if (!backTouched)
			{
				backTouched = true;
				Navigation.PopAsync();
			}
			return true;
		}
        public firstFirst()
        {
			InitializeComponent();
            backTouched = false;
			onProcessing = false;

			//switch (Device.RuntimePlatform)
			//{
			//	case Device.Android:
			//		absL.Scale = (App.ScreenHeight - 90) / 265;
			//		break;
			//	default:
			//		absL.Scale = (App.ScreenHeight - 70) / 265;
			//		break;
			//}
        }
		protected async override void OnAppearing()
		{
			var MapInfo = await SyncData.DownloadShopMapInfo();

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
					default:
						shopLocation.TextColor = Color.Default;
						break;
				}

				var tapGestureRecognizer = new TapGestureRecognizer();
				tapGestureRecognizer.Tapped += OnTapped;
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
				ShopInforSQLDb infoFromSQLite = new ShopInforSQLDb();
				var temp = sender as Label;
				//infoFromSQLite = await App.Database.GetShopAsync("2지구", "1층", temp.Text);
				await Navigation.PushAsync(new ShopInfoPage("2지구", "1층", temp.Text));
				onProcessing = false;
			}
		}
    }
}
