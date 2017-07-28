using System;
using System.Diagnostics;
using westgateproject.Helper;
using Xamarin.Forms;

namespace westgateproject.View
{
    public partial class WritingPage : TabbedPage
	{
		private bool onProcessing;
        public WritingPage()
		{
			InitializeComponent();

			CameraButton.Clicked += CameraButton_Clicked;
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

		private async void CameraButton_Clicked(object sender, EventArgs e)
		{
			var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

            if (photo != null)
            {
                PhotoImage.Source = ImageSource.FromStream(photo.GetStream);
                PhotoImage.HeightRequest = App.ScreenHeight * 0.7;
            }
		}


		void EditorTextChanged(object sender, TextChangedEventArgs e)
		{
			//var oldText = e.OldTextValue;
			//var newText = e.NewTextValue;

			//Debug.WriteLine("newText : " + newText);
			//Debug.WriteLine("oldText : " + oldText);


   //         if(newText.Length > oldText.Length)
   //         {
			//	if (newText.EndsWith(System.Environment.NewLine))
			//		UploadTextEditor.HeightRequest += 10;
                
   //         }
   //         else
			//{
				//if (oldText.EndsWith(System.Environment.NewLine))
					//UploadTextEditor.HeightRequest -= 10;
                
            //}
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
