﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using westgateproject.Helper;
using westgateproject.Models;
using westgateproject.View;
using westgateproject.View.PageForEachFloor.myungpoom;
using westgateproject.View.PageForEachFloor.second;
using Xamarin.Forms;

namespace westgateproject
{
	public partial class InitialPage : ContentPage
	{
        private bool isClicked;
		public InitialPage()
		{
			InitializeComponent();

	        switch (Device.RuntimePlatform)
			{
				case Device.Android:
					DependencyService.Get<ILoginHelper>().StartLogin();
                    login.IsVisible = true;
					break;
				case Device.iOS:
					break;
			}
			isClicked = false;
		}

        public Button getGuest()
        {
            return guest;
        }

		async protected override void OnAppearing()
		{
            // Handle when your app starts
            var shopSync = await SyncData.SyncShopInfo();
            var buildingSync = await SyncData.SyncBuildingInfo();

            if(!shopSync || !buildingSync)
				syncStatus.Text="서버에서 데이터를 가져올 수 없습니다. 앱정보 페이지에서 REFRESH를 눌러 다시 시도할 수 있습니다.";
            else
                syncStatus.Text="지도 정보 동기화가 완료되었습니다.";
		}

		async void startClicked(object sender, EventArgs e)
		{
            if(App.userEmail != null)
            {
                switch(Device.RuntimePlatform)
                {
                    case Device.Android:
                        if (!isClicked)
                            isClicked = true;
                        break;
                    case Device.iOS:
                        break;
                }
                await Navigation.PushAsync(new FirstPage());
                Navigation.RemovePage((Navigation.NavigationStack[0]));
            }
            else
            {
                await DisplayAlert("", "You need to be loged in.", "OK");
            }
		}

        async void disableButton(object sender, EventArgs e)
        {
            if (App.userEmail == null)
            {
                DependencyService.Get<ILoginHelper>().StartLogin();
            }
            else
            {
                await DisplayAlert("", "Login is already completed.", "OK");
            }
        }
	}
}
