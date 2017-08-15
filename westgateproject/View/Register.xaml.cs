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
            if(shopNameEntry.Text == null || phoneNumberEntry.Text == null)
            {
                await DisplayAlert("빈 칸 있음", "매장 이름과 전화 번호를 입력해 주세요.", "확인");
            }
            else
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

				await DisplayAlert("신청 완료", "내 매장으로 등록 신청이 완료 되었습니다.", "확인");

				await Navigation.PopAsync();
            }

		}

		private async void CancelClicked(object sender, EventArgs e)
		{
			var answer = await DisplayAlert("취소", "등록 요청을 취소하시겠습니까?", "취소", "무시");
            if (answer)
            {
                await SyncData.DeleteUserInfo(App.userEmail, _building, _floor, _location);
                await Navigation.PopAsync();
            }
		}
	}
}
