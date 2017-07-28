using System;
using westgateproject.Helper;
using Xamarin.Forms;

namespace westgateproject.View
{
    public partial class WritingPage : ContentPage
    {
        public WritingPage()
		{
			InitializeComponent();

			CameraButton.Clicked += CameraButton_Clicked;
		}

		async void OnItemClicked(object sender, EventArgs e)
		{
			var shopSync = await SyncData.SyncShopInfo();
			var buildingSync = await SyncData.syncBuildingInfo();
			refresh.IsVisible = true;
			if (!shopSync || !buildingSync)
				refresh.Text = "서버에서 데이터를 가져올 수 없습니다. 마지막에 저장된 데이터를 사용합니다." + Environment.NewLine + System.DateTime.Now.ToString("G");
			else
				refresh.Text = "서버에서 지도 정보를 가져왔습니다." + Environment.NewLine + System.DateTime.Now.ToString("G");
		}

		private async void CameraButton_Clicked(object sender, EventArgs e)
		{
			var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

			if (photo != null)
				PhotoImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
		}
    }
}
