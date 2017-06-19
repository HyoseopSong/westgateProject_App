using System;
using System.Collections.Generic;
using System.Diagnostics;
using westgateproject.Models;
using Xamarin.Forms;

namespace westgateproject.View.PageForEachFloor
{
    public partial class SQLiteViewer : ContentPage
    {
        public SQLiteViewer()
        {
            InitializeComponent();
        }

        async public void OnBuildingClicked(object sender, EventArgs args)
        {

            List<BuildingInformation> infoFromSQLite = new List<BuildingInformation>();
            infoFromSQLite = await App.Database.GetBuildingAsync();
            foreach (var info in infoFromSQLite)
			{
				Debug.WriteLine("info.ID : " + info.ID);
				Debug.WriteLine("info.Building : " + info.Building);
				Debug.WriteLine("info.BaseFirst : " + info.BaseFirst);
				Debug.WriteLine("info.First : " + info.First);
				Debug.WriteLine("info.Second : " + info.Second);
				Debug.WriteLine("info.Third : " + info.Third);
				Debug.WriteLine("info.Forth : " + info.Forth);
                Debug.WriteLine("");
            }
        }

        async void OnShopClicked(object sender, EventArgs args)
		{
			List<ShopInformation> infoFromSQLite = new List<ShopInformation>();
			infoFromSQLite = await App.Database.GetShopAsync();
			foreach (var info in infoFromSQLite)
			{
				Debug.WriteLine("info.ID : " + info.ID);
				Debug.WriteLine("info.Building : " + info.Building);
				Debug.WriteLine("info.Floor : " + info.Floor);
				Debug.WriteLine("info.Location : " + info.Location);
				Debug.WriteLine("info.ShopName : " + info.ShopName);
				Debug.WriteLine("info.PhoneNumber : " + info.PhoneNumber);
				Debug.WriteLine("");
			}
        }
    }
}
