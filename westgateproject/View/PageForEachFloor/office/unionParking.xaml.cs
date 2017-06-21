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
			zoomContainer.Content.AnchorX = 0;
			zoomContainer.Content.AnchorY = 0;
			zoomContainer.min_ty = 0;
			switch (Device.RuntimePlatform)
			{
				case Device.Android:
					zoomContainer.Content.Scale = (App.ScreenHeight - 90) / 565;
					break;
				default:
					zoomContainer.Content.Scale = (App.ScreenHeight - 70) / 565;
					break;
			}
			zoomContainer.min_tx = -((890 * zoomContainer.Content.Scale) - App.ScreenWidth);
			zoomContainer.maxScale = zoomContainer.Content.Scale;
			zoomContainer.minScale = zoomContainer.Content.Scale;
			NavigationPage.SetHasBackButton(this, false);
            parkingImage.TranslationX = 90;
		}
		async void goBack(object sender, EventArgs args)
		{
			await Navigation.PopAsync();
		}
    }
}
