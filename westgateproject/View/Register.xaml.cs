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

        string _payment;
		string _phoneNumber;
		string _homepage;

		public Register()
		{
            InitializeComponent();
		}

		public Register(string building, string floor, string location)
		{
			InitializeComponent();
			switch (building)
			{
				case "동산상가":
					_building = "Dongsan";
					break;
				case "2지구":
					_building = "SecondBuilding";
					break;
				case "5지구":
					_building = "FifthBuilding";
					break;
				default:
					_building = "Empty";
					break;
			}
			_floor = floor;
			_location = location;
            Title = location + " 등록";
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
                _phoneNumber = userInfo["phoneNumber"];
				addInfo.Text = userInfo["addInfo"];
                switch(userInfo["payment"])
                {
                    case "계좌 이체":
                        paymentPicker.SelectedIndex = 0;
                        break;
					case "현장 결제":
						paymentPicker.SelectedIndex = 1;
                        break;
				}
				switch (userInfo["homepage"])
				{
					case "신청 함":
						homepagePicker.SelectedIndex = 0;
						break;
					case "신청 안함":
						homepagePicker.SelectedIndex = 1;
						break;
				}
                CancelRequest.IsVisible = true;
            }
        }
		private async void RegisterClicked(object sender, EventArgs e)
		{
            if(shopNameEntry.Text == null || phoneNumberEntry.Text == null || _payment == null || _homepage == null)
            {
                await DisplayAlert("빈 칸 있음", "매장 이름과 전화 번호, 결제 방법, 홈페이지 신청여부를 입력해 주세요.", "확인");
            }
            else
			{
				if (addInfo.Text == null)
				{
                    addInfo.Text = " ";
				}
				IDictionary<string, string> postDictionary = new Dictionary<string, string>
				{
					{ "id", App.userEmail},
					{ "name", shopNameEntry.Text },
					{ "building", _building},
					{ "floor", _floor},
					{ "location", _location },
					{ "number", _phoneNumber},
					{ "addInfo", addInfo.Text},
					{ "payment", _payment},
                    { "homepage", _homepage }
				};


				await App.Client.InvokeApiAsync("userInformation", System.Net.Http.HttpMethod.Post, postDictionary);

				switch (_payment)
				{
					case "계좌 이체":
						switch (_homepage)
						{
							case "신청 함":
                                await DisplayAlert("신청 완료", "2만원 입금 확인 후 서비스가 개시됩니다.", "확인");
								break;
							case "신청 안함":
								await DisplayAlert("신청 완료", "1만원 입금 확인 후 서비스가 개시됩니다.", "확인");
								break;
						}
						break;
					case "현장 결제":
						await DisplayAlert("신청 완료", "방문 일정 확인 후 연락 드리겠습니다.", "확인");
						break;
				}



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

		private void PhoneNumberEntryUnfocused(object sender, EventArgs e)
		{
            
            var phone = (Entry)sender;
            switch(Device.RuntimePlatform)
            {
				case Device.Android:
					if (phone.Text == null)
					{
						return;
					}
                    break;
				case Device.iOS:
					if (phone.Text.Length == 0)
					{
						return;
					}
                    break;
            }

            var number = phone.Text;

            if(phone.Text.Substring(0,2) == "02" && phone.Text.Length == 10)
            {

				number = number.Insert(2, "-");
				number = number.Insert(7, "-");
            }
            else if(phone.Text.Length == 10)
            {
                number = number.Insert(3, "-");
                number = number.Insert(7, "-");
            }
            else if(phone.Text.Length == 11)
            {
				number = number.Insert(3, "-");
				number = number.Insert(8, "-");
            }
            else
            {
                
            }
            _phoneNumber = number;
            phone.Text = number;


		}

		void OnPickerSelectedIndexChanged(object sender, EventArgs e)
		{
			var picker = (Picker)sender;
			int selectedIndex = picker.SelectedIndex;

			if (selectedIndex != -1)
			{
				_payment = picker.Items[selectedIndex];
			}
			switch (_payment)
			{
				case "계좌 이체":
                    bankAccouont.IsVisible = true;
					break;
				case "현장 결제":
                    bankAccouont.IsVisible = false;
					break;
			}
		}
		void HomepageChanged(object sender, EventArgs e)
		{
			var picker = (Picker)sender;
			int selectedIndex = picker.SelectedIndex;

			if (selectedIndex != -1)
			{
				_homepage = picker.Items[selectedIndex];
			}
		}
	}
}
