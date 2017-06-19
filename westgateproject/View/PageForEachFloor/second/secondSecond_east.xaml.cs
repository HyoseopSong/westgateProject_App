using System;
using System.Collections.Generic;
using westgateproject.Models;
using Xamarin.Forms;

namespace westgateproject.View.PageForEachFloor.second
{
    public partial class secondSecond_east : ContentPage
    {
        public secondSecond_east()
        {
			InitializeComponent();
			zoomContainer.anchorX = 0.4;
			zoomContainer.anchorY = 0.2;
			//NavigationPage.SetHasNavigationBar(this, false);
		}
		async void OnTapped(object sender, EventArgs args)
		{
			ShopInformation infoFromSQLite = new ShopInformation();
			var temp = sender as Label;
			infoFromSQLite = await App.Database.GetShopAsync("2지구", "지하1층", temp.Text);
			await Navigation.PushAsync(new ShopInfoPage(infoFromSQLite));
		}
    }
}
