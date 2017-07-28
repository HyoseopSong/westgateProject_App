using System;
using System.Collections.Generic;
using System.Diagnostics;
using westgateproject.Helper;
using Xamarin.Forms;

namespace westgateproject
{
	public partial class AboutPage : ContentPage
	{
        private bool onProcessing;
		public AboutPage()
		{
			InitializeComponent();

            syncLabel();
		}

        async void OnItemClicked(object sender, EventArgs e)
		{
			var shopSync = await SyncData.SyncShopInfo();
			var buildingSync = await SyncData.SyncBuildingInfo();
			refresh.IsVisible = true;
			if (!shopSync || !buildingSync)
				refresh.Text = "서버에서 데이터를 가져올 수 없습니다. 마지막에 저장된 데이터를 사용합니다." + Environment.NewLine + System.DateTime.Now.ToString("G");
            else
                refresh.Text = "서버에서 지도 정보를 가져왔습니다." + Environment.NewLine + System.DateTime.Now.ToString("G");
        }

        async protected void syncLabel()
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            try
            {
                result = await App.Client.InvokeApiAsync<IDictionary<string, string>>("notice", System.Net.Http.HttpMethod.Get, null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.GetType());
                appState.Text = "서버에서 정보를 불러올 수 없습니다.";
                return;
            }
            string[] state = result["개발현황"].Split(':');
            appState.Text = "";
            for (int i = 0; i < state.Length; i++)
            {
                appState.Text += state[i] + Environment.NewLine;
			}

		}
		void sendingEmail(Label sender, EventArgs args)
		{
			if (!onProcessing)
			{
				onProcessing = true;
				Device.OpenUri(new Uri("mailto:ChopsticksOfMetal@gmail.com"));
				onProcessing = false;
			}
		}
		void openHomepage(Label sender, EventArgs args)
		{
			if (!onProcessing)
			{
				onProcessing = true;
                Device.OpenUri(new Uri("http://xn--ok0bo23cmodlb.xn--3e0b707e"));
				onProcessing = false;
			}
		}
	}
}
