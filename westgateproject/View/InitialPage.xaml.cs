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
		async protected override void OnAppearing()
		{
			switch (Device.RuntimePlatform)
			{
				case Device.Android:
                    
					MessagingCenter.Subscribe<object>(this, "OK", async (sender) =>
					{
						login.IsVisible = false;
						start.IsVisible = true;
						loginStatus.Text = "로그인 되었습니다!";
						await Navigation.PushAsync(new FirstPage());
						Navigation.RemovePage((Navigation.NavigationStack[0]));

					});

					MessagingCenter.Subscribe<object>(this, "Canceled", (sender) =>
					{
						login.IsVisible = true;
                        login.IsEnabled = true;
                        start.IsVisible = false;
						loginStatus.Text = "로그인 버튼을 눌러주세요.";

					});

					if (CrossConnectivity.Current.IsConnected)
					{
						DependencyService.Get<ILoginHelper>().StartLogin();
					}
					else
					{
						await DisplayAlert("네트워크 연결 없음", "네트워크에 연결한 후 다시 시도해주세요.", "확인");
						login.IsVisible = true;
                        start.IsVisible = false;
                        loginStatus.Text = "로그인 버튼을 눌러주세요.";
					}
					break;
				case Device.iOS:
					if (CrossConnectivity.Current.IsConnected)
					{
                        
					}
					else
					{
                        
					}
					break;
			}


            // Handle when your app starts
            var shopSync = await SyncData.SyncShopInfo();
            var buildingSync = await SyncData.SyncBuildingInfo();

    //        if(!shopSync || !buildingSync)
				//syncStatus.Text="서버에서 데이터를 가져올 수 없습니다. 앱정보 페이지에서 REFRESH를 눌러 다시 시도할 수 있습니다.";
            //else
                //syncStatus.Text="지도 정보 동기화가 완료되었습니다.";


     //       switch (Device.RuntimePlatform)
     //       {
     //           case Device.Android:
     //               if (App.userEmail != null)
     //               {
					//	login.IsVisible = false;
					//	start.IsVisible = true;
     //                   loginStatus.Text = "로그인 되었습니다!";
     //               }
     //               else
					//{
					//	login.IsVisible = true;
     //                   start.IsVisible = false;
					//}
					//break;
            //}

		}

		async void StartClicked(object sender, EventArgs e)
		{
   //         if(App.userEmail != null)
			//{
				start.IsEnabled = false;

                await Navigation.PushAsync(new FirstPage());
                Navigation.RemovePage((Navigation.NavigationStack[0]));
            //}
            //else
            //{
            //    await DisplayAlert("로그인 필요", "로그인 버튼을 눌러 주세요.", "확인");
            //}
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
