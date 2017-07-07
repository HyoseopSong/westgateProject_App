﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using westgateproject.Helper;
using westgateproject.Models;
using westgateproject.View;
using westgateproject.View.PageForEachFloor.myungpoom;
using westgateproject.View.PageForEachFloor.second;
using Xamarin.Auth;
using Xamarin.Forms;

namespace westgateproject
{
    public partial class InitialPage : ContentPage
    {
        Account account;
        AccountStore store;
        private bool isClicked;
        public InitialPage()
        {
            InitializeComponent();
            isClicked = false;
            store = AccountStore.Create();
            account = store.FindAccountsForService(Constants.AppName).FirstOrDefault();
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
                await Navigation.PushAsync(new marketMap());
                Navigation.RemovePage(Navigation.NavigationStack[0]);
            }
        }

        void loginButton(object sender, EventArgs e)
        {
            string clientId = null;
            string redirectUri = null;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    clientId = Constants.iOSClientId;
                    redirectUri = Constants.iOSRedirectUrl;
                    break;

                case Device.Android:
                    clientId = Constants.AndroidClientId;
                    redirectUri = Constants.AndroidRedirectUrl;
                    break;
            }

            var authenticator = new OAuth2Authenticator(
                clientId,
                null,
                Constants.Scope,
                new Uri(Constants.AuthorizeUrl),
                new Uri(redirectUri),
                new Uri(Constants.AccessTokenUrl),
                null,
                true);

            authenticator.Completed += OnAuthCompleted;
            authenticator.Error += OnAuthError;

            AuthenticationState.Authenticator = authenticator;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);
        }
        async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            User user = null;
            if (e.IsAuthenticated)
            {
                
                // If the user is authenticated, request their basic user data from Google
                // UserInfoUrl = https://www.googleapis.com/oauth2/v2/userinfo
                var request = new OAuth2Request("GET", new Uri(Constants.UserInfoUrl), null, e.Account);
                var response = await request.GetResponseAsync();
                if (response != null)
                {
                    // Deserialize the data and store it in the account store
                    // The users email address will be used to identify data in SimpleDB
                    string userJson = await response.GetResponseTextAsync();
                    user = JsonConvert.DeserializeObject<User>(userJson);
                }

                if (account != null)
                {
                    store.Delete(account, Constants.AppName);
                }

                await store.SaveAsync(account = e.Account, Constants.AppName);
                await DisplayAlert("Email address", user.Email, "OK");


                foreach (var temp in e.Account.Properties)
                    Debug.WriteLine(temp.Key + " : " + temp.Value);

                App.Client.CurrentUser = new MobileServiceUser(e.Account.Properties["id_token"]);
                App.Client.CurrentUser.MobileServiceAuthenticationToken = e.Account.Properties["access_token"];
                Debug.WriteLine("Token  : " + App.Client.CurrentUser.MobileServiceAuthenticationToken);
                Debug.WriteLine("UserId : " + App.Client.CurrentUser.UserId);
                IDictionary<string, string> result = new Dictionary<string, string>();
                result = await App.Client.InvokeApiAsync<IDictionary<string, string>>("notice", System.Net.Http.HttpMethod.Get, null);
            }
        }

        void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            Debug.WriteLine("Authentication error: " + e.Message);
        }
    }
}
