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
            isClicked = false;
		}

		async protected override void OnAppearing()
		{
            // Handle when your app starts
            var shopSync = await SyncData.SyncShopInfo();
            var buildingSync = await SyncData.syncBuildingInfo();
            if(!shopSync || !buildingSync)
				syncStatus.Text="서버에서 데이터를 가져올 수 없습니다. 앱정보 페이지에서 REFRESH를 눌러 다시 시도할 수 있습니다.";
            else
                syncStatus.Text="지도 정보 동기화가 완료되었습니다.";
		}

		async void startClicked(object sender, EventArgs e)
		{
            if (!isClicked)
            {
                isClicked = true;
                await Navigation.PushAsync(new FirstPage());
                Navigation.RemovePage(Navigation.NavigationStack[0]);
            }
		}

        void disableButton(object sender, EventArgs e)
        {
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    DependencyService.Get<ILoginHelper>().StartLogin();
                    break;
                case Device.iOS:
                    login.IsVisible = false;
                    break;
            }
        }
	}
}
