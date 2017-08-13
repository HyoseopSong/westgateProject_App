using System;
using System.Collections.Generic;
using System.Diagnostics;
using westgateproject.Helper;
using Xamarin.Forms;

namespace westgateproject.View
{
	public partial class Register : ContentPage
	{
		string _building;
		string _floor;
		string _location;
		public Register()
		{
            InitializeComponent();
		}

		public Register(string building, string floor, string location)
		{
			InitializeComponent();
			_building = building;
			_floor = floor;
			_location = location;
		}

        protected override async void OnAppearing()
		{

			Dictionary<string, string> userInfo = new Dictionary<string, string>();
			Dictionary<string, string> getParamUserInfo = new Dictionary<string, string>
			{
				{ "id", App.userEmail},
				{ "building", _building},
				{ "floor", _floor},
                { "location", _location},
			};
			userInfo = await App.Client.InvokeApiAsync<Dictionary<string, string>>("userInformation", System.Net.Http.HttpMethod.Get, getParamUserInfo);
            if (userInfo != null)
            {
                shopNameEntry.Text = userInfo["shopName"];
                phoneNumberEntry.Text = userInfo["phoneNumber"];
                addInfo.Text = userInfo["addInfo"];
                CancelRequest.IsVisible = true;
            }
        }
		private async void RegisterClicked(object sender, EventArgs e)
		{

            IDictionary<string, string> postDictionary = new Dictionary<string, string>
            {
                { "id", App.userEmail},
                { "name", shopNameEntry.Text },
				{ "building", _building},
				{ "floor", _floor},
				{ "location", _location },
				{ "number", phoneNumberEntry.Text},
                { "addInfo", addInfo.Text}
            };
            await App.Client.InvokeApiAsync("userInformation", System.Net.Http.HttpMethod.Post, postDictionary);

			await DisplayAlert("Register Reqeust Completed!", "After you give me the money, it will be done within this midnight", "Done");

			await Navigation.PopAsync();
		}

		private async void CancelClicked(object sender, EventArgs e)
		{
			var answer = await DisplayAlert("Request will be deleted!", "Do you want to cancel a request?", "Yes", "No");
            if (answer)
            {
                await SyncData.DeleteUserInfo(App.userEmail, _building, _floor, _location);
                await Navigation.PopAsync();
            }
		}
	}
}
