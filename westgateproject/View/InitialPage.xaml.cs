using System;
using Plugin.Connectivity;
using westgateproject.Helper;
using Xamarin.Forms;

namespace westgateproject
{
    public partial class InitialPage : ContentPage
	{
		public InitialPage()
		{
			InitializeComponent();

        }

        public Button GetStartButton()
        {
            return start;
        }

		public Label GetLoginStatus()
		{
			return loginStatus;
		}
		protected async override void OnAppearing()
		{
			switch (Device.RuntimePlatform)
			{
				case Device.Android:
                    
					MessagingCenter.Subscribe<object>(this, "OK", async (sender) =>
					{
						login.IsVisible = false;
						start.IsVisible = false;
						loginStatus.Text = "로그인 되었습니다!";
						await Navigation.PushAsync(new FirstPage());

					});

					break;
				//case Device.iOS:
					//if (CrossConnectivity.Current.IsConnected)
					//{
                        
					//}
					//else
					//{
                        
					//}
					//break;
			}


            // Handle when your app starts
            var shopSync = await SyncData.SyncShopInfo();
            var buildingSync = await SyncData.SyncBuildingInfo();


		}

		async void StartClicked(object sender, EventArgs e)
		{
			start.IsEnabled = false;
            await Navigation.PushAsync(new FirstPage());
		}

        async void LoginClicked(object sender, EventArgs e)
        {
            login.IsEnabled = false;
            if (App.userEmail == null)
            {
				if (CrossConnectivity.Current.IsConnected)
				{
					DependencyService.Get<ILoginHelper>().StartLogin();

				}
				else
				{
					await DisplayAlert("네트워크 연결 없음", "네트워크에 연결한 후 다시 시도해주세요.", "확인");
					loginStatus.Text = "로그인 버튼을 눌러주세요.";
                    login.IsEnabled = true;
				}
            }

    //        else
    //        {
				//await DisplayAlert("로그인 완료", "시작 버튼을 눌러 주세요.", "확인");
				//login.IsVisible = false;
				//start.IsVisible = true;
            //}
        }
	}
}
