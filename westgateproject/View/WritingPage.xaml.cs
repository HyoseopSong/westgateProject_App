using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;
using westgateproject.Helper;
using westgateproject.Models;
using Xamarin.Forms;

namespace westgateproject.View
{
    public partial class WritingPage : TabbedPage
	{
        private MediaFile photoStream;
        String imageURL;

        public WritingPage()
		{
			InitializeComponent();


            myIdLabel.Text = "내 계정 : " + App.userEmail;
            myIdLabel.VerticalTextAlignment = TextAlignment.Center;

            CameraButton.WidthRequest = App.ScreenWidth / 2;
            PictureButton.WidthRequest = App.ScreenWidth / 2;

            switch (Device.RuntimePlatform)
            {
				case Device.iOS:
                    //shopName.HeightRequest = 30;
                    //shopLocation.HeightRequest = 30;
                    //phoneNumber.HeightRequest = 30;
                    myIdLabel.HeightRequest = 30;
                    break;
				case Device.Android:
					//shopName.HeightRequest = 40;
					//shopLocation.HeightRequest = 40;
					//phoneNumber.HeightRequest = 40;
					myIdLabel.HeightRequest = 40;
                    break;
            }



            //syncLabel();
		}
        protected override async void OnAppearing()
		{
			Dictionary<string, UserInfoEntity> userInfo = new Dictionary<string, UserInfoEntity>();
			Dictionary<string, string> getParamUserInfo = new Dictionary<string, string>
			{
				{ "id", App.userEmail},
			};
            try
            {
                userInfo = await App.Client.InvokeApiAsync<Dictionary<string, UserInfoEntity>>("userInformation", System.Net.Http.HttpMethod.Get, getParamUserInfo);
            }
            catch
            {
                var myLabel = new Label()
                {
                    Text = "내 매장 정보가 없습니다."
                };
                MyInformation.Children.Add(myLabel);
            }
            if (userInfo.Count > 0)
            {
                foreach (var UserInfo in userInfo)
                {
                    Label shopName = new Label()
                    {
                        Text = "Shop Name : " + UserInfo.Value.RowKey,
                        VerticalTextAlignment = TextAlignment.Center
                    };
                    Label shopBuilding = new Label()
                    {
                        Text = "Shop Building : " + UserInfo.Value.ShopBuilding,
                        VerticalTextAlignment = TextAlignment.Center
                    };
                    Label shopFloor = new Label()
                    {
                        Text = "Shop Floor : " + UserInfo.Value.ShopFloor,
                        VerticalTextAlignment = TextAlignment.Center
                    };
                    Label shopLocation = new Label()
                    {
                        Text = "Shop Location : " + UserInfo.Value.ShopLocation,
                        VerticalTextAlignment = TextAlignment.Center
                    };
                    Label phoneNumber = new Label()
                    {
                        Text = "Phone Number : " + UserInfo.Value.PhoneNumber,
                        VerticalTextAlignment = TextAlignment.Center
                    };
                    BoxView myBox = new BoxView()
                    {
                        HeightRequest = 10,
                        BackgroundColor = Color.LightGray
                    };

                    switch (Device.RuntimePlatform)
                    {
                        case Device.iOS:
                            shopName.HeightRequest = 30;
                            shopBuilding.HeightRequest = 30;
                            shopFloor.HeightRequest = 30;
                            shopLocation.HeightRequest = 30;
                            phoneNumber.HeightRequest = 30;
                            break;
                        case Device.Android:
                            shopName.HeightRequest = 40;
                            shopBuilding.HeightRequest = 40;
                            shopFloor.HeightRequest = 40;
                            shopLocation.HeightRequest = 40;
                            phoneNumber.HeightRequest = 40;
                            break;
                    }

                    MyInformation.Children.Add(shopName);
                    MyInformation.Children.Add(shopBuilding);
                    MyInformation.Children.Add(shopFloor);
                    MyInformation.Children.Add(shopLocation);
                    MyInformation.Children.Add(phoneNumber);
                    MyInformation.Children.Add(myBox);
                }
            }
            else
            {
				var myLabel = new Label()
				{
					Text = "내 매장 정보가 없습니다."
				};
				MyInformation.Children.Add(myLabel);
            }

            bool IsSuccess = true;
			Dictionary<string, string> imageSource = new Dictionary<string, string>();
			try
			{
				Dictionary<string, string> getParam = new Dictionary<string, string>
				{
					{ "id", App.userEmail},
				};
				imageSource = await App.Client.InvokeApiAsync<Dictionary<string, string>>("upload", System.Net.Http.HttpMethod.Get, getParam);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.GetType());
                IsSuccess = false;

                var myLabel = new Label()
                {
                    Text = "내 활동 내역이 없습니다."
				};
				myActivity.Children.Add(myLabel);

			}

            if (IsSuccess)
            {
                foreach (var temp in imageSource)
                {
                    imageURL = "https://westgateproject.blob.core.windows.net/" + App.userEmail.Split('@')[0] + "/" + temp.Key;

                    switch (Device.RuntimePlatform)
                    {
                        case Device.Android:
                            var myImage_Android = new Image { Aspect = Aspect.AspectFit, HeightRequest = App.ScreenWidth };
                            var imageByte = await DependencyService.Get<IImageScaleHelper>().GetImageStream(imageURL);
                            //Debug.WriteLine("imageURL : " + imageURL);
                            //Debug.WriteLine("Orientation value : " + DependencyService.Get<IImageScaleHelper>().OrientationOfImage(imageByte));
                            myImage_Android.Source = ImageSource.FromStream(() => new MemoryStream(imageByte));

                            string OrientationOfImage = await DependencyService.Get<IImageScaleHelper>().OrientationOfImage(imageURL);
                            switch (OrientationOfImage)
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
                            var myImage_iOS = new Image { Aspect = Aspect.AspectFit, HeightRequest = App.ScreenWidth };
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
                        var myImage_iOS = new Image { Aspect = Aspect.AspectFit, HeightRequest = App.ScreenWidth };
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

		private void EditPropertyClicked(object sender, EventArgs e)
		{
   //         shopName.Text = "Shop Name : ";
   //         shopNameEntry.IsVisible = true;
			//shopNameEntry.Placeholder = "Type Shop Name";

			//shopBuilding.Text = "Shop Building : ";
			//shopBuildingEntry.IsVisible = true;
			//shopBuildingEntry.Placeholder = "Type Shop Location";

			//shopFloor.Text = "Shop Location : ";
			//shopFloorEntry.IsVisible = true;
			//shopFloorEntry.Placeholder = "Type Shop Location";

            //shopLocation.Text = "Shop Location : ";
            //shopLocationEntry.IsVisible = true;
            //shopLocationEntry.Placeholder = "Type Shop Location";

            //phoneNumber.Text = "Phone Number : ";
            //phoneNumberEntry.IsVisible = true;
            //phoneNumberEntry.Placeholder = "Type Phone Number";

            //editProperty.IsVisible = false;
            //confirmProperty.IsVisible = true;
		}

		private async void ConfirmPropertyClicked(object sender, EventArgs e)
		{
			//Dictionary<string, string> postParam = new Dictionary<string, string>
			//	{
			//		{ "id", App.userEmail},
			//		{ "name", shopNameEntry.Text},
			//		{ "building", shopBuildingEntry.Text},
			//		{ "floor", shopFloorEntry.Text},
			//		{ "location", shopLocationEntry.Text},
			//		{ "number", phoneNumberEntry.Text},
			//	};
			//try
			//{
			//	await App.Client.InvokeApiAsync("upload", System.Net.Http.HttpMethod.Post, postParam);
			//}
			//catch (Exception ex)
			//{
			//	Debug.WriteLine(ex.GetType());

			//}

            //shopName.Text = "Shop Name : " + shopNameEntry.Text;
            //shopNameEntry.IsVisible = false;
            //shopNameEntry.Text = "";

            //shopBuilding.Text = "Shop Location : " + shopLocationEntry.Text;
            //shopBuildingEntry.IsVisible = false;
            //shopBuildingEntry.Text = "";

            //shopFloor.Text = "Shop Location : " + shopLocationEntry.Text;
            //shopFloorEntry.IsVisible = false;
            //shopFloorEntry.Text = "";

            //shopLocation.Text = "Shop Location : " + shopLocationEntry.Text;
            //shopLocationEntry.IsVisible = false;
            //shopLocationEntry.Text = "";

            //phoneNumber.Text = "Phone Number : " + phoneNumberEntry.Text;
            //phoneNumberEntry.IsVisible = false;
            //phoneNumberEntry.Text = "";

            //editProperty.IsVisible = true;
            //confirmProperty.IsVisible = false;

		}
    }
}
