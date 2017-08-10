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
        string[] imageArray;

        public WritingPage()
		{
			InitializeComponent();


            myIdLabel.Text = "내 계정 : " + App.userEmail;

            CameraButton.WidthRequest = App.ScreenWidth / 2;
            PictureButton.WidthRequest = App.ScreenWidth / 2;

            //syncLabel();
		}
        protected override async void OnAppearing()
		{
			Dictionary<string, string> imageSource = new Dictionary<string, string>();
            //Dictionary<string, string> imageSource = new Dictionary<string, string>
            //{
            //{ "2지구 1층 중앙 통로입니다~ 통로에도 많은 물건들을 구경할 수 있어요^^", "https://westgateproject.blob.core.windows.net/blob1/2017-08-08%20PM%208%3A29%3A55.jpg" },



            //{ "2지구 1층 중앙 통로입니다~ 통로에도 많은 물건들을 구경할 수 있어요^^", "https://westgateproject.blob.core.windows.net/blob1/IMG_8289.jpg" },
            //{ "2지구 지하1층에는 맛있는 먹거리들로 가득하답니다~", "https://westgateproject.blob.core.windows.net/blob1/IMG_8294.jpg" },
            //{ "각 층마다 이렇게 쉴 수 있는 공간도 마련되어 있답니다 (^^)", "https://westgateproject.blob.core.windows.net/blob1/IMG_8304.jpg" },
            //{ "서쪽 출입구에 있는 서문시장의 대표 조형물입니다~ ^^乃", "https://westgateproject.blob.core.windows.net/blob1/IMG_8315.jpg" },



            //{ "2지구 1층 중앙 통로입니다~ 통로에도 많은 물건들을 구경할 수 있어요^^", "https://westgateproject.blob.core.windows.net/blob1/2017-08-06%20PM%206%3A39%3A15.jpg" },
            //{ "2지구 지하1층에는 맛있는 먹거리들로 가득하답니다~", "https://westgateproject.blob.core.windows.net/blob1/2017-08-06%20PM%206%3A39%3A15.jpg" },
            //{ "각 층마다 이렇게 쉴 수 있는 공간도 마련되어 있답니다 (^^)", "https://westgateproject.blob.core.windows.net/blob1/2017-08-06%20PM%206%3A39%3A15.jpg" },
            //{ "서쪽 출입구에 있는 서문시장의 대표 조형물입니다~ ^^乃", "https://westgateproject.blob.core.windows.net/blob1/2017-08-06%20PM%206%3A39%3A15.jpg" }
            //};
			try
			{
				Dictionary<string, string> getParam = new Dictionary<string, string>
				{
					{ "id", App.userEmail},
				};
				imageSource = await App.Client.InvokeApiAsync<Dictionary<string, string>>("upload", System.Net.Http.HttpMethod.Get, getParam);
				imageArray = new string[imageSource.Count];
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.GetType());
				return;
			}
            int i = imageSource.Count - 1;
			foreach (var temp in imageSource)
			{
                imageURL = "https://westgateproject.blob.core.windows.net/" + App.userEmail.Split('@')[0] + "/" +  temp.Key;

                imageArray[i--] = temp.Key;

                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                        var myImage_Android = new Image { Aspect = Aspect.AspectFit, HeightRequest = App.ScreenWidth };
						var imageByte = await DependencyService.Get<IImageScaleHelper>().GetImageStream(imageURL);
						//Debug.WriteLine("imageURL : " + imageURL);
						//Debug.WriteLine("Orientation value : " + DependencyService.Get<IImageScaleHelper>().OrientationOfImage(imageByte));
                        myImage_Android.Source = ImageSource.FromStream(() => new MemoryStream(imageByte));

                        string OrientationOfImage = await DependencyService.Get<IImageScaleHelper>().OrientationOfImage(imageURL);
                        switch(OrientationOfImage)
                        {
                            
                            case "1":
								break;
							case "2":
								break;
							case "3":
								break;
							case "4":
								break;
							case "5":
								break;
							case "6":
                                myImage_Android.Rotation = 90;
								break;
							case "7":
								break;
							case "8":
								break;
							default:
								var tapGestureRecognizer = new TapGestureRecognizer();
								tapGestureRecognizer.Tapped += (s, e) =>
								{
									var img = s as Image;
									img.Rotation += 90;
								};
								myImage_Android.GestureRecognizers.Add(tapGestureRecognizer);
								break;
                        }
						myActivity.Children.Insert(0, myImage_Android);
                        break;
                    case Device.iOS:
                        var myImage_iOS = new Image { Aspect = Aspect.AspectFit };
						myImage_iOS.Source = ImageSource.FromUri(new Uri(imageURL));
						myActivity.Children.Insert(0, myImage_iOS);
						break;
				}


				var myLabel = new Label()
				{
					Text = temp.Value
				};
				myActivity.Children.Insert(1, myLabel);

				var imageName = new Label()
				{
					Text = temp.Key,
                    IsVisible = false
				};
				myActivity.Children.Insert(2, imageName);

                var myButton = new Button()
                {
                    Text = "삭제"
				};
                myButton.Clicked += DeleteButton_Clicked;
                myActivity.Children.Insert(3, myButton);

                var myBoxView = new BoxView()
                {
                    HeightRequest = 10,
                    BackgroundColor = Color.LightGray
                };
				myActivity.Children.Insert(4, myBoxView);
			}


			imageSource = new Dictionary<string, string>
			{
				{ "서문시장 동쪽 입구입니다.", "https://westgateproject.blob.core.windows.net/blob1/2017-08-08%20PM%208%3A22%3A37.jpg" },



				//{ "서문시장 동쪽 입구입니다.", "https://westgateproject.blob.core.windows.net/blob1/%EC%84%9C%EB%AC%B8%EC%8B%9C%EC%9E%A5%20%EC%9E%85%EA%B5%AC" },
				//{ "동산상가 2층 동쪽 출입구 입니다.", "https://westgateproject.blob.core.windows.net/blob1/%EB%8F%99%EC%82%B0%EC%83%81%EA%B0%80" },
				//{ "건해산물상가 입니다.", "https://westgateproject.blob.core.windows.net/blob1/%EA%B1%B4%ED%95%B4%EC%82%B0%EB%AC%BC%EC%83%81%EA%B0%80" },
				//{ "2지구 입니다.", "https://westgateproject.blob.core.windows.net/blob1/2%EC%A7%80%EA%B5%AC" },
				//{ "1지구 입구입니다.", "https://westgateproject.blob.core.windows.net/blob1/1%EC%A7%80%EA%B5%AC" },
				//{ "아진상가 입니다.", "https://westgateproject.blob.core.windows.net/blob1/%EC%95%84%EC%A7%84%EC%83%81%EA%B0%80" },
				//{ "5지구 입니다.", "https://westgateproject.blob.core.windows.net/blob1/5%EC%A7%80%EA%B5%AC" },


                //{ "서문시장 동쪽 입구입니다.", "https://westgateproject.blob.core.windows.net/blob1/2017-08-06%20PM%206%3A39%3A15.jpg" },
				{ "동산상가 2층 동쪽 출입구 입니다.", "https://westgateproject.blob.core.windows.net/blob1/2017-08-06%20PM%206%3A39%3A15.jpg" },
				//{ "건해산물상가 입니다.", "https://westgateproject.blob.core.windows.net/blob1/2017-08-06%20PM%206%3A39%3A15.jpg" },
				//{ "2지구 입니다.", "https://westgateproject.blob.core.windows.net/blob1/2017-08-06%20PM%206%3A39%3A15.jpg" },
				//{ "1지구 입구입니다.", "https://westgateproject.blob.core.windows.net/blob1/2017-08-06%20PM%206%3A39%3A15.jpg" },
				//{ "아진상가 입니다.", "https://westgateproject.blob.core.windows.net/blob1/2017-08-06%20PM%206%3A39%3A15.jpg" },
				//{ "5지구 입니다.", "https://westgateproject.blob.core.windows.net/blob1/2017-08-06%20PM%206%3A39%3A15.jpg" }
			};
			foreach (var temp in imageSource)
			{
				imageURL = temp.Value;

				switch (Device.RuntimePlatform)
				{
					case Device.Android:
						var myImage_Android = new Image { Aspect = Aspect.AspectFit, HeightRequest = App.ScreenWidth };
                        Image tempImage = new Image();
						var tapGestureRecognizer = new TapGestureRecognizer();
						tapGestureRecognizer.Tapped += (s, e) =>
						{
							var img = s as Image;
							img.Rotation += 90;
						};
						myImage_Android.GestureRecognizers.Add(tapGestureRecognizer);
						//Debug.WriteLine("imageURL : " + imageURL);
						var imageByte = await DependencyService.Get<IImageScaleHelper>().GetImageStream(imageURL);
						//Debug.WriteLine("Orientation value : " + DependencyService.Get<IImageScaleHelper>().OrientationOfImage(imageByte));
						myImage_Android.Source = ImageSource.FromStream(() => new MemoryStream(imageByte));
						myRecent.Children.Add(myImage_Android);
						break;
					case Device.iOS:
                        var myImage_iOS = new Image { Aspect = Aspect.AspectFit };
						myImage_iOS.Source = ImageSource.FromUri(new Uri(imageURL));
						myRecent.Children.Add(myImage_iOS);
						break;
				}


				var myLabel = new Label()
				{
					Text = temp.Key
				};
				myRecent.Children.Add(myLabel);

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
            var result = await SyncData.UploadContents(photoStream, UploadTextEditor.Text);
            switch(result)
            {
				case null:
					await DisplayAlert("No blank", "You must fill both text and picture", "OK");
                    break;
				default:
					result += ".jpg";
					var myImage = new Image { Aspect = Aspect.AspectFit, HeightRequest = App.ScreenWidth };
					myImage.Source = ImageSource.FromStream(photoStream.GetStream);
					myActivity.Children.Insert(0, myImage);

					var myLabel = new Label()
					{
						Text = UploadTextEditor.Text
					};
					myActivity.Children.Insert(1, myLabel);


					var imageName = new Label()
					{
						Text = result,
						IsVisible = false
					};
					myActivity.Children.Insert(2, imageName);


					var myButton = new Button()
					{
						Text = "삭제"
					};
					myButton.Clicked += DeleteButton_Clicked;
					myActivity.Children.Insert(3, myButton);

					var myBoxView = new BoxView()
					{
						HeightRequest = 10,
						BackgroundColor = Color.LightGray
					};
					myActivity.Children.Insert(4, myBoxView);

					PhotoImage.IsVisible = false;
					UploadTextEditor.Text = "";
                    break;
                    
            }
		}

		private async void CameraButton_Clicked(object sender, EventArgs e)
		{
            photoStream = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
			{
                PhotoSize = PhotoSize.Small,
                RotateImage = true

			});

            PhotoImage.IsVisible = true;

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

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            int senderIndex = myActivity.Children.IndexOf(sender as Button);
			var imageName = myActivity.Children[senderIndex - 1] as Label;
			var result = await SyncData.DeleteContents(imageName.Text);

			int indexNumber = senderIndex / 5;
			for (int ii = indexNumber; ii < indexNumber + 5; ii++)
			{
				myActivity.Children[ii].IsVisible = false;
			}

        }


        private async void PicturePicker_Clicked(object sender, EventArgs e)
        {
            photoStream = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
            {
                PhotoSize = PhotoSize.Small
            });

			PhotoImage.IsVisible = true;

			if (photoStream != null)
			{
				PhotoImage.Source = ImageSource.FromStream(photoStream.GetStream);
				PhotoImage.HeightRequest = App.ScreenHeight * 0.7;
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
