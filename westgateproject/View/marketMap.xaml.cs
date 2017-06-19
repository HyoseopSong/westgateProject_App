using System;
using System.Collections.Generic;
using System.Diagnostics;
using westgateproject.Models;
using westgateproject.View.PageForEachFloor;
using westgateproject.View.PageForEachFloor.second;
using Xamarin.Forms;

namespace westgateproject
{
    public partial class marketMap : ContentPage
	{
        public marketMap()
        {
			InitializeComponent();
			NavigationPage.SetHasBackButton(this, false);
            zoomContainer.Content.AnchorX = 0;
            zoomContainer.Content.AnchorY = 0;
			zoomContainer.min_ty = 0;
			switch (Device.RuntimePlatform)
			{
				case Device.Android:
					Debug.WriteLine("Android width : " + App.ScreenWidth);
					zoomContainer.Content.Scale = (App.ScreenHeight - 90) / 565;
					break;
				default:
                    Debug.WriteLine("Default width : " + App.ScreenWidth);
					zoomContainer.Content.Scale = (App.ScreenHeight - 70) / 565;
					break;
			}
            zoomContainer.min_tx = -((800 * zoomContainer.Content.Scale) - App.ScreenWidth);
			zoomContainer.maxScale = zoomContainer.Content.Scale;
			zoomContainer.minScale = zoomContainer.Content.Scale;
        }

        public List<string> marketList()
        {
            List<string> markets = new List<string>();
            foreach(var abs in absLayout.Children)
            {
                var absLabel = abs as Label;
                if(absLabel != null)
                {
                    markets.Add(absLabel.Text);
                }
            }
            return markets;
        }
        async void OnItemClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new AboutPage());
        }

        async void OnTapped(Label sender, EventArgs args)
        {
            var color = sender.TextColor;
            sender.TextColor = new Color(219, 112, 147);
            await Navigation.PushAsync(new buildingInfo(sender.Text));
            sender.TextColor = color;
		}

		async void OnTappedSQL(Label sender, EventArgs args)
		{
			await Navigation.PushAsync(new SQLiteViewer());

		}

    }
}
