using System;
using System.Collections.Generic;
using westgateproject.Models;
using Xamarin.Forms;

namespace westgateproject.View.PageForEachFloor.office
{
    public partial class shopUnion : ContentPage
    {
        public shopUnion()
        {
			InitializeComponent();
			zoomContainer.Content.AnchorX = 0;
			zoomContainer.Content.AnchorY = 0;

			zoomContainer.min_ty = 0;
			switch (Device.RuntimePlatform)
			{
				case Device.Android:
					zoomContainer.Content.Scale = (App.ScreenHeight - 90) / 190;
					break;
				default:
					zoomContainer.Content.Scale = (App.ScreenHeight - 70) / 190;
					break;
			}
			zoomContainer.maxScale = zoomContainer.Content.Scale;
			zoomContainer.minScale = zoomContainer.Content.Scale;
			zoomContainer.min_tx = -((540 * zoomContainer.Content.Scale) - App.ScreenWidth);
			NavigationPage.SetHasBackButton(this, false);
		}

		async void OnTapped(object sender, EventArgs args)
		{
			await Navigation.PushAsync(new unionParking());
		}
		async void goBack(object sender, EventArgs args)
		{
			await Navigation.PopAsync();
		}
    }
}
