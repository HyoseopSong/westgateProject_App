using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;
using westgateproject.Helper;
using Xamarin.Forms;

namespace westgateproject.View
{
    public partial class WritingPage : TabbedPage
	{
		private bool onProcessing;
        private MediaFile photoStream;
        String imageURL;

        public WritingPage()
		{
			InitializeComponent();

			CameraButton.Clicked += CameraButton_Clicked;

            myIdLabel.Text = "내 계정 : " + App.userEmail;

            //syncLabel();
		}
        protected override async void OnAppearing()
        {
			IDictionary<string, string> imageSource = new Dictionary<string, string>
			{
				{ "2지구 1층 중앙 통로입니다~ 통로에도 많은 물건들을 구경할 수 있어요^^", "https://westgateproject.blob.core.windows.net/blob1/IMG_8289.jpg" },
				{ "2지구 지하1층에는 맛있는 먹거리들로 가득하답니다~", "https://westgateproject.blob.core.windows.net/blob1/IMG_8294.jpg" },
				{ "각 층마다 이렇게 쉴 수 있는 공간도 마련되어 있답니다 (^^)", "https://westgateproject.blob.core.windows.net/blob1/IMG_8304.jpg" },
				{ "서쪽 출입구에 있는 서문시장의 대표 조형물입니다~ ^^乃", "https://westgateproject.blob.core.windows.net/blob1/IMG_8315.jpg" }
			};
			foreach (var temp in imageSource)
			{
				var myImage = new Image { Aspect = Aspect.AspectFill, HorizontalOptions=LayoutOptions.FillAndExpand };
				imageURL = temp.Value;
				var contentText = temp.Key;

				switch (Device.RuntimePlatform)
				{
					case Device.Android:
						var tapGestureRecognizer = new TapGestureRecognizer();
						tapGestureRecognizer.Tapped += (s, e) =>
						{
							var img = s as Image;
							img.Rotation += 90;
						};
						myImage.GestureRecognizers.Add(tapGestureRecognizer);
						Debug.WriteLine("imageURL : " + imageURL);
						var imageByte = await DependencyService.Get<IImageScaleHelper>().GetImageStream(imageURL);
						myImage.Source = ImageSource.FromStream(() => new MemoryStream(imageByte));
						break;
					case Device.iOS:
						myImage.Source = ImageSource.FromUri(new Uri(imageURL));
						break;
				}

				myActivity.Children.Add(myImage);

				var myLabel = new Label()
				{
					Text = temp.Key
				};
				myActivity.Children.Add(myLabel);

				var myButton = new Button()
				{
				  Text = "삭제"
				};
				myActivity.Children.Add(myButton);

				var myBoxView = new BoxView();
				myBoxView.HeightRequest = 10;
				myBoxView.BackgroundColor = Color.LightGray;
				myActivity.Children.Add(myBoxView);
			}


			imageSource = new Dictionary<string, string>
			{
				{ "서문시장 동쪽 입구입니다.", "https://westgateproject.blob.core.windows.net/blob1/%EC%84%9C%EB%AC%B8%EC%8B%9C%EC%9E%A5%20%EC%9E%85%EA%B5%AC" },
				{ "동산상가 2층 동쪽 출입구 입니다.", "https://westgateproject.blob.core.windows.net/blob1/%EB%8F%99%EC%82%B0%EC%83%81%EA%B0%80" },
				{ "건해산물상가 입니다.", "https://westgateproject.blob.core.windows.net/blob1/%EA%B1%B4%ED%95%B4%EC%82%B0%EB%AC%BC%EC%83%81%EA%B0%80" },
				{ "2지구 입니다.", "https://westgateproject.blob.core.windows.net/blob1/2%EC%A7%80%EA%B5%AC" },
				{ "1지구 입구입니다.", "https://westgateproject.blob.core.windows.net/blob1/1%EC%A7%80%EA%B5%AC" },
				{ "아진상가 입니다.", "https://westgateproject.blob.core.windows.net/blob1/%EC%95%84%EC%A7%84%EC%83%81%EA%B0%80" },
				{ "5지구 입니다.", "https://westgateproject.blob.core.windows.net/blob1/5%EC%A7%80%EA%B5%AC" }
			};
			foreach (var temp in imageSource)
			{
				var myImage = new Image { Aspect = Aspect.AspectFit };
				imageURL = temp.Value;
				var contentText = temp.Key;

				switch (Device.RuntimePlatform)
				{
					case Device.Android:
						var tapGestureRecognizer = new TapGestureRecognizer();
						tapGestureRecognizer.Tapped += (s, e) =>
						{
							var img = s as Image;
							img.Rotation += 90;
						};
						myImage.GestureRecognizers.Add(tapGestureRecognizer);
						Debug.WriteLine("imageURL : " + imageURL);
                        var imageByte = await DependencyService.Get<IImageScaleHelper>().GetImageStream(imageURL);
						myImage.Source = ImageSource.FromStream(() => new MemoryStream(imageByte));
						break;
					case Device.iOS:
						myImage.Source = ImageSource.FromUri(new Uri(imageURL));
						break;
				}

				myRecent.Children.Add(myImage);

				var myLabel = new Label()
				{
					Text = temp.Key
				};
				myRecent.Children.Add(myLabel);

				//var myButton = new Button()
				//{
				//	Text = "삭제"
				//};
				//myRecent.Children.Add(myButton);

				var myBoxView = new BoxView();
				myBoxView.HeightRequest = 10;
				myBoxView.BackgroundColor = Color.LightGray;
				myRecent.Children.Add(myBoxView);
			}
        }


		//async void OnItemClicked(object sender, EventArgs e)
		//{
		//	var shopSync = await SyncData.SyncShopInfo();
		//	var buildingSync = await SyncData.SyncBuildingInfo();
		//	refresh.IsVisible = true;
		//	if (!shopSync || !buildingSync)
		//		refresh.Text = "서버에서 데이터를 가져올 수 없습니다. 마지막에 저장된 데이터를 사용합니다." + Environment.NewLine + System.DateTime.Now.ToString("G");
		//	else
		//		refresh.Text = "서버에서 지도 정보를 가져왔습니다." + Environment.NewLine + System.DateTime.Now.ToString("G");
		//}

		async void UploadButton_Clicked(object sender, EventArgs e)
		{

            await SyncData.UploadContents(photoStream, UploadTextEditor.Text);
            PhotoImage.IsVisible = false;
            UploadTextEditor.Text = "";
		}

		private async void CameraButton_Clicked(object sender, EventArgs e)
		{
            photoStream = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
			{
                PhotoSize = PhotoSize.Small
			});
            PhotoImage.IsVisible = true;

            Image temp = new Image();

            if (photoStream != null)
            {
                PhotoImage.Source = ImageSource.FromStream(photoStream.GetStream);
                PhotoImage.HeightRequest = App.ScreenHeight * 0.7;
            }
		}

		//async protected void syncLabel()
		//{
		//	IDictionary<string, string> result = new Dictionary<string, string>();
		//	try
		//	{
		//		result = await App.Client.InvokeApiAsync<IDictionary<string, string>>("notice", System.Net.Http.HttpMethod.Get, null);
		//	}
		//	catch (Exception ex)
		//	{
		//		Debug.WriteLine(ex.GetType());
		//		appState.Text = "서버에서 정보를 불러올 수 없습니다.";
		//		return;
		//	}
		//	string[] state = result["개발현황"].Split(':');
		//	appState.Text = "";
		//	for (int i = 0; i < state.Length; i++)
		//	{
		//		appState.Text += state[i] + Environment.NewLine;
		//	}

		//}

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
