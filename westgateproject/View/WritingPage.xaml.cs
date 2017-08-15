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
        string _shopName;
        bool isInitial;
		int deleteCount;
		int moreButtonCount;
        string[] recentKeyArray;
        Dictionary<string, RecentEntity> recentSource;

        public WritingPage()
		{
			InitializeComponent();
            deleteCount = 0;
            moreButtonCount = 0;

            myIdLabel.Text = "내 계정 : " + App.userEmail;
            myIdLabel.VerticalTextAlignment = TextAlignment.Center;

            CameraButton.WidthRequest = App.ScreenWidth / 2;
            PictureButton.WidthRequest = App.ScreenWidth / 2;
            UploadTextEditor.BackgroundColor = Color.Lime;
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

            isInitial = true;

            //syncLabel();
		}
        protected override async void OnAppearing()
		{
            if(!isInitial)
            {
                Debug.WriteLine("OnAppearing if");
                return;
            }
            else
			{
				Debug.WriteLine("OnAppearing else");
                isInitial = false;
            }
			Dictionary<string, UserInfoEntity> userInfo = new Dictionary<string, UserInfoEntity>();
			Dictionary<string, string> getParamUserInfo = new Dictionary<string, string>
			{
				{ "id", App.userEmail},
			};

            userInfo = await App.Client.InvokeApiAsync<Dictionary<string, UserInfoEntity>>("userInformation", System.Net.Http.HttpMethod.Get, getParamUserInfo);

			Picker shopPicker = new Picker
			{
				Title = "게시 할 매장",
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

            if (userInfo.Count > 0)
			{
                int paidCount = 0;
                int unpaidCount = 0;

				BoxView myBoxView = new BoxView()
				{
					HeightRequest = 10,
					BackgroundColor = Color.LightGray
				};
				MyInformation.Children.Add(myBoxView);
                foreach (var UserInfo in userInfo)
                {
                    if (UserInfo.Value.Paid)
					{
                        paidCount++;
                        _shopName = UserInfo.Key;
						shopPicker.Items.Add(UserInfo.Key);

                        var temp = UserInfo.Value;
                        var shopInfo = temp.RowKey.Split(':');

                        Label shopName = new Label()
                        {
                            Text = "매장 이름 : " + UserInfo.Key,
                            VerticalTextAlignment = TextAlignment.Center
                        };
                        Label shopLocation = new Label()
                        {
                            Text = "위치 : " + shopInfo[0] + " " + shopInfo[1] + " " + shopInfo[2],
                            VerticalTextAlignment = TextAlignment.Center
                        };
                        Label phoneNumber = new Label()
                        {
                            Text = "전화 번호 : " + UserInfo.Value.PhoneNumber,
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
                                shopLocation.HeightRequest = 30;
                                phoneNumber.HeightRequest = 30;
                                break;
                            case Device.Android:
                                shopName.HeightRequest = 40;
                                shopLocation.HeightRequest = 40;
                                phoneNumber.HeightRequest = 40;
                                break;
                        }

                        MyInformation.Children.Add(shopName);
                        MyInformation.Children.Add(shopLocation);
                        MyInformation.Children.Add(phoneNumber);
                        MyInformation.Children.Add(myBox);
                    }
                    else
					{
                        unpaidCount++;
						var temp = UserInfo.Value;
						var rawShopInfo = temp.RowKey.Split(':');
						Label shopInfo = new Label()
						{
							Text = rawShopInfo[0] + " " + rawShopInfo[1] + " " + rawShopInfo[2] + " " + UserInfo.Value.ShopName + " 등록 대기 중",
							VerticalTextAlignment = TextAlignment.Center
						};

						BoxView myBox = new BoxView()
						{
							HeightRequest = 10,
							BackgroundColor = Color.LightGray
						};

						MyInformation.Children.Insert(0, shopInfo);
                    }
                }
                if (paidCount > 1)
                {
					shopPicker.SelectedIndexChanged += (sender, args) =>
					{
						_shopName = shopPicker.Items[shopPicker.SelectedIndex];
				    };
                    MyInformation.Children.Add(shopPicker);
                }

                if (unpaidCount < 1)
                {
                    referenceBoxView.IsVisible = false;
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
				Debug.WriteLine(ex.GetType() + "내 활동 내역이 없습니다.");

                statusLabel.IsVisible = true;

			}

            if (imageSource.Count > 0)
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
									myImage_Android.RotationY = 180;
									break;
								case "3":
									myImage_Android.RotationX = 180;
									myImage_Android.RotationY = 180;
									break;
								case "4":
									myImage_Android.RotationX = 180;
									break;
								case "5":
									myImage_Android.Rotation = 90;
									myImage_Android.RotationY = 180;
									break;
								case "6":
									myImage_Android.Rotation = 90;
									break;
								case "7":
									myImage_Android.Rotation = 90;
									myImage_Android.RotationX = 180;
									break;
								case "8":
									myImage_Android.Rotation = 270;
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
            else
            {
				Debug.WriteLine("내 활동 내역이 없습니다.");

                statusLabel.IsVisible = true;
            }

			//imageSource = new Dictionary<string, string>
			//{
				//{ "서문시장 동쪽 입구입니다.", "https://westgateproject.blob.core.windows.net/blob1/2017-08-08%20PM%208%3A22%3A37.jpg" },



            //    //{ "서문시장 동쪽 입구입니다.", "https://westgateproject.blob.core.windows.net/blob1/%EC%84%9C%EB%AC%B8%EC%8B%9C%EC%9E%A5%20%EC%9E%85%EA%B5%AC" },
            //    //{ "동산상가 2층 동쪽 출입구 입니다.", "https://westgateproject.blob.core.windows.net/blob1/%EB%8F%99%EC%82%B0%EC%83%81%EA%B0%80" },
            //    //{ "건해산물상가 입니다.", "https://westgateproject.blob.core.windows.net/blob1/%EA%B1%B4%ED%95%B4%EC%82%B0%EB%AC%BC%EC%83%81%EA%B0%80" },
            //    //{ "2지구 입니다.", "https://westgateproject.blob.core.windows.net/blob1/2%EC%A7%80%EA%B5%AC" },
            //    //{ "1지구 입구입니다.", "https://westgateproject.blob.core.windows.net/blob1/1%EC%A7%80%EA%B5%AC" },
            //    //{ "아진상가 입니다.", "https://westgateproject.blob.core.windows.net/blob1/%EC%95%84%EC%A7%84%EC%83%81%EA%B0%80" },
            //    //{ "5지구 입니다.", "https://westgateproject.blob.core.windows.net/blob1/5%EC%A7%80%EA%B5%AC" },


            //    //{ "서문시장 동쪽 입구입니다.", "https://westgateproject.blob.core.windows.net/blob1/2017-08-06%20PM%206%3A39%3A15.jpg" },
            //    { "동산상가 2층 동쪽 출입구 입니다.", "https://westgateproject.blob.core.windows.net/blob1/2017-08-06%20PM%206%3A39%3A15.jpg" },
            //    //{ "건해산물상가 입니다.", "https://westgateproject.blob.core.windows.net/blob1/2017-08-06%20PM%206%3A39%3A15.jpg" },
            //    //{ "2지구 입니다.", "https://westgateproject.blob.core.windows.net/blob1/2017-08-06%20PM%206%3A39%3A15.jpg" },
            //    //{ "1지구 입구입니다.", "https://westgateproject.blob.core.windows.net/blob1/2017-08-06%20PM%206%3A39%3A15.jpg" },
            //    //{ "아진상가 입니다.", "https://westgateproject.blob.core.windows.net/blob1/2017-08-06%20PM%206%3A39%3A15.jpg" },
            //    //{ "5지구 입니다.", "https://westgateproject.blob.core.windows.net/blob1/2017-08-06%20PM%206%3A39%3A15.jpg" }
            //};


            recentSource = new Dictionary<string, RecentEntity>();
            recentSource = await App.Client.InvokeApiAsync<Dictionary<string, RecentEntity>>("recent", System.Net.Http.HttpMethod.Get, null);
            Debug.WriteLine("recentSource.Count : " + recentSource.Count);
            if(recentSource.Count > 0)
            {
				// imageSource에 있는 키값 배열에 다 넣고 앞에 10개만 보여주기
				recentKeyArray = new string[recentSource.Count];
				Debug.WriteLine("recentKeyArray.Length : " + recentKeyArray.Length);
                recentSource.Keys.CopyTo(recentKeyArray,0);
                int startIndex = 0;
                if(recentKeyArray.Length > 10)
                {
                    startIndex = recentKeyArray.Length - 10;
                }
                else
                {
                    startIndex = 0;
                }
                for (int i = startIndex; i < startIndex + 10 && i < recentKeyArray.Length; i++)
                {
                    var blobName = recentKeyArray[i];
                    var recentEnt = recentSource[recentKeyArray[i]];

                    imageURL = "https://westgateproject.blob.core.windows.net/" + recentEnt.PartitionKey.Split('@')[0] + "/" + blobName;

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
									myImage_Android.RotationY = 180;
									break;
								case "3":
									myImage_Android.RotationX = 180;
									myImage_Android.RotationY = 180;
									break;
								case "4":
									myImage_Android.RotationX = 180;
									break;
								case "5":
									myImage_Android.Rotation = 90;
									myImage_Android.RotationY = 180;
									break;
								case "6":
									myImage_Android.Rotation = 90;
									break;
								case "7":
									myImage_Android.Rotation = 90;
									myImage_Android.RotationX = 180;
									break;
								case "8":
									myImage_Android.Rotation = 270;
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
							myRecent.Children.Insert(0, myImage_Android);
							break;
						case Device.iOS:
							var myImage_iOS = new Image { Aspect = Aspect.AspectFit, HeightRequest = App.ScreenWidth };
							myImage_iOS.Source = ImageSource.FromUri(new Uri(imageURL));
							myRecent.Children.Insert(0, myImage_iOS);
							break;
					}

					var labelButton = new Button()
					{
						Text = recentEnt.Text
					};
					labelButton.Clicked += LabelButton_Clicked;
					myRecent.Children.Insert(1, labelButton);

					var shopInfo = new Label()
					{
						Text = recentEnt.PartitionKey + ":" + recentEnt.ShopName,
						IsVisible = false
					};
					myRecent.Children.Insert(2, shopInfo);

					var myBoxView = new BoxView()
					{
						HeightRequest = 10,
						BackgroundColor = Color.LightGray
					};
					myRecent.Children.Insert(3, myBoxView);
                }
                if(recentKeyArray.Length > 10)
                {

					var labelButton = new Button()
					{
						Text = "더 불러오기"
					};
					labelButton.Clicked += MoreButton_Clicked;
					myRecent.Children.Add(labelButton);
                }

            }
            else
            {

				var myLabel = new Label()
				{
					Text = "최근 정보가 없습니다."
				};
				myRecent.Children.Add(myLabel);
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
            if(_shopName == null)
            {
                await DisplayAlert("등록된 매장 없음", "내 매장을 등록하시면 게시 하실 수 있습니다.", "확인");
                return;
			}
			var senderButton = sender as Button;
			senderButton.IsEnabled = false;

            var result = await SyncData.UploadContents(photoStream, UploadTextEditor.Text, _shopName);
            switch(result)
            {
				case null:
					await DisplayAlert("빈 칸 있음", "사진과 글 모두 채워주세요.", "확인");
                    break;
				default:
                    statusLabel.IsVisible = false;
					result += ".jpg";
					var myImage = new Image { Aspect = Aspect.AspectFit, HeightRequest = App.ScreenWidth };
					myImage.Source = ImageSource.FromStream(photoStream.GetStream);
					myActivity.Children.Insert(0, myImage);

					var mySubLabel = new Label()
					{
						Text = UploadTextEditor.Text
					};
					myActivity.Children.Insert(1, mySubLabel);


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
			senderButton.IsEnabled = true;
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
			var senderButton = sender as Button;
			senderButton.IsEnabled = false;
            deleteCount++;
            int senderIndex = myActivity.Children.IndexOf(sender as Button);
			var imageName = myActivity.Children[senderIndex - 1] as Label;
			var result = await SyncData.DeleteContents(imageName.Text);

			int indexNumber = senderIndex - 3;
			for (int ii = indexNumber; ii < indexNumber + 5; ii++)
			{
				myActivity.Children[ii].IsVisible = false;
			}
            if(deleteCount == myActivity.Children.Count / 5)
            {
                statusLabel.IsVisible = true;
			}
			senderButton.IsEnabled = true;
        }

		private async void MoreButton_Clicked(object sender, EventArgs e)
		{
			var senderButton = sender as Button;
			senderButton.IsEnabled = false;

            moreButtonCount++;
            int startIndex = recentKeyArray.Length - (moreButtonCount * 10) - 1;
            if(startIndex < 0)
            {
                startIndex = 0;
            }
            Debug.WriteLine("startIndex : " + startIndex);

            for (int i = startIndex; (i > startIndex - 10) && i >= 0 ; i--)
			{
				Debug.WriteLine("for statement i : " + i);


				var blobName = recentKeyArray[i];
				var recentEnt = recentSource[recentKeyArray[i]];

				imageURL = "https://westgateproject.blob.core.windows.net/" + recentEnt.PartitionKey.Split('@')[0] + "/" + blobName;

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
                                myImage_Android.RotationY = 180;
								break;
							case "3":
								myImage_Android.RotationX = 180;
								myImage_Android.RotationY = 180;
								break;
							case "4":
								myImage_Android.RotationX = 180;
								break;
							case "5":
								myImage_Android.Rotation = 90;
								myImage_Android.RotationY = 180;
								break;
							case "6":
								myImage_Android.Rotation = 90;
								break;
							case "7":
								myImage_Android.Rotation = 90;
								myImage_Android.RotationX = 180;
								break;
							case "8":
								myImage_Android.Rotation = 270;
								break;
							default:
								var tapGestureRecognizer = new TapGestureRecognizer();
								tapGestureRecognizer.Tapped += (s, ea) =>
								{
									var img = s as Image;
									img.Rotation += 90;
								};
								myImage_Android.GestureRecognizers.Add(tapGestureRecognizer);
								break;
						}


                        myRecent.Children.Insert(myRecent.Children.Count - 1, myImage_Android);


						break;
					case Device.iOS:
						var myImage_iOS = new Image { Aspect = Aspect.AspectFit, HeightRequest = App.ScreenWidth };
						myImage_iOS.Source = ImageSource.FromUri(new Uri(imageURL));
						myRecent.Children.Insert(myRecent.Children.Count - 1, myImage_iOS);
						break;
				}

				var labelButton = new Button()
				{
					Text = recentEnt.Text
				};
				labelButton.Clicked += LabelButton_Clicked;
				myRecent.Children.Insert(myRecent.Children.Count - 1, labelButton);

				var shopInfo = new Label()
				{
					Text = recentEnt.PartitionKey + ":" + recentEnt.ShopName,
					IsVisible = false
				};
				myRecent.Children.Insert(myRecent.Children.Count - 1, shopInfo);

				var myBoxView = new BoxView()
				{
					HeightRequest = 10,
					BackgroundColor = Color.LightGray
				};
				myRecent.Children.Insert(myRecent.Children.Count - 1, myBoxView);
			}

            if (recentKeyArray.Length > (moreButtonCount+1) * 10)
            {
                senderButton.IsVisible = true;
            }
            else
            {
                senderButton.IsVisible = false;
            }

            senderButton.IsEnabled = true;
		}

		private async void LabelButton_Clicked(object sender, EventArgs e)
		{
			var senderButton = sender as Button;
			senderButton.IsEnabled = false;
			int senderIndex = myRecent.Children.IndexOf(sender as Button);
			var imageName = myRecent.Children[senderIndex + 1] as Label;
            Debug.WriteLine("imageName.text : " + imageName.Text);
			var id = imageName.Text.Split(':')[0];
			var shopName = imageName.Text.Split(':')[1];

			IDictionary<string, string> getParam = new Dictionary<string, string>
			{
				{ "id", id},
				{ "shopName", shopName},
			};
            Dictionary<string, string> shopInfo = new Dictionary<string, string>();
            shopInfo = await App.Client.InvokeApiAsync<Dictionary<string, string>>("recent", System.Net.Http.HttpMethod.Get, getParam);

            Debug.WriteLine("building : " + shopInfo["building"] + "floor : " + shopInfo["floor"] + "location : " + shopInfo["location"]);
			await Navigation.PushAsync(new ShopInfoPage(shopInfo["building"], shopInfo["floor"], shopInfo["location"]));
			senderButton.IsEnabled = true;
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

    }
}
