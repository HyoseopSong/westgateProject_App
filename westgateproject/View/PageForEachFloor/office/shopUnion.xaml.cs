using System;
using System.Collections.Generic;
using westgateproject.Models;
using Xamarin.Forms;

namespace westgateproject.View.PageForEachFloor.office
{
    public partial class shopUnion : ContentPage
	{
		public bool onProcessing;
        public shopUnion()
        {
			InitializeComponent();
			absL.AnchorX = 0;
			absL.AnchorY = 0;

			switch (Device.RuntimePlatform)
			{
				case Device.Android:
					absL.Scale = (App.ScreenHeight - 90) / 189;
					break;
				default:
					absL.Scale = (App.ScreenHeight - 70) / 189;
					break;
			}
			NavigationPage.SetHasBackButton(this, false);
			onProcessing = false;

			var boundaryBox = new BoxView { Color = Color.Red };
			AbsoluteLayout.SetLayoutBounds(boundaryBox, new Rectangle(490 * absL.Scale, App.ScreenWidth, 0, 30));
			absL.Children.Add(boundaryBox);
		}

		async void OnTapped(object sender, EventArgs args)
		{
			if (!onProcessing)
			{
				onProcessing = true;
				await Navigation.PushAsync(new unionParking());
				onProcessing = false;
			}
		}
		async void goBack(object sender, EventArgs args)
		{
		    await Navigation.PopAsync();
		}
    }
}
