using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace westgateproject.View.PageForEachFloor.office
{
    public partial class unionParking : ContentPage
	{
        public unionParking()
        {
			InitializeComponent();
			absL.AnchorX = 0;
			absL.AnchorY = 0;
			switch (Device.RuntimePlatform)
			{
				case Device.Android:
					absL.Scale = (App.ScreenHeight - 90) / 354;
					break;
				default:
					absL.Scale = (App.ScreenHeight - 70) / 354;
					break;
			}
			NavigationPage.SetHasBackButton(this, false);
			parkingImage.TranslationX = 90;

			var boundaryBox = new BoxView { Color = Color.Red };
			AbsoluteLayout.SetLayoutBounds(boundaryBox, new Rectangle(689 * absL.Scale, App.ScreenWidth, 0, 30));
			absL.Children.Add(boundaryBox);
		}


		async void goBack(object sender, EventArgs args)
		{
			await Navigation.PopAsync();
		}
    }
}
