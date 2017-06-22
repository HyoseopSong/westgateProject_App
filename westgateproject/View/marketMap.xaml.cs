﻿﻿using System;
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
        public bool onProcessing;
        public marketMap()
        {
			InitializeComponent();
			switch (Device.RuntimePlatform)
			{
				case Device.Android:
					absL.AnchorX = 0;
					absL.AnchorY = 0;
					absL.Scale = (App.ScreenHeight - 90) / 565;
					break;
				default:
                    absL.AnchorX = 0.53;
                    absL.AnchorY = 0.53;
					absL.Scale = (App.ScreenHeight - 70) / 565;
					break;
			}

			NavigationPage.SetHasBackButton(this, false);


			var boundaryBox = new BoxView { Color = Color.Red };
			AbsoluteLayout.SetLayoutBounds(boundaryBox, new Rectangle(800 * absL.Scale, App.ScreenWidth, 0, 30));
            absL.Children.Add(boundaryBox);
            onProcessing = false;
        }
        async void OnItemClicked(object sender, EventArgs args)
        {
            if (!onProcessing)
            {
                onProcessing = true;
                await Navigation.PushAsync(new AboutPage());
                onProcessing = false;
            }
        }

        async void OnTapped(Label sender, EventArgs args)
		{
			if (!onProcessing)
			{
				onProcessing = true;
	            var color = sender.TextColor;
	            sender.TextColor = new Color(219, 112, 147);
	            await Navigation.PushAsync(new buildingInfo(sender.Text));
				sender.TextColor = color;
				onProcessing = false;
			}
		}

        async void onPreparing(Label sender, EventArgs args)
		{
			if (!onProcessing)
			{
				onProcessing = true;
	            switch (sender.Text)
	            {
	                case "4지구":
	                    await DisplayAlert(sender.Text, "화재 현장 복구 중 입니다.", "확인");
	                    break;
	                default:
	                    await DisplayAlert(sender.Text, "지도 정보가 아직 준비되지 않았습니다.", "확인");
	                    break;
				}
				onProcessing = false;
		    }
		}
    }
}
