using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using westgateproject.View;
using System.Threading.Tasks;
using System.IO;
using westgateproject.Helper;
using westgateproject.Models;

namespace westgateproject
{
	public partial class FirstPage : TabbedPage
	{
		public bool onProcessing;
		//RecentEntity[] recentEntityArray;
		int moreButtonCount;
        int likeMoreButtonCount;
		String imageURL;
        List<string> likeInfoList;
        List<string> recentShopInfo;
        List<RecentEntity> recentSource;
        List<LikeEntity> likeEntity;
        List<int> likeNumList;


		public FirstPage()
		{
            InitializeComponent();

            NavigationPage.SetHasBackButton(this, false);
            switch(Device.RuntimePlatform)
            {
				case Device.Android:
					WGMarketMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(35.8680505, 128.5805485), Distance.FromKilometers(0.3)));
                    break;
				case Device.iOS:
					WGMarketMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(35.8680838081858, 128.580841355511), Distance.FromKilometers(0.25)));
                    break;
            }

            //(35.866364, 128.582999) (35.869737, 128.578098) => 
			WGMarketMap.ShapeCoordinates.Add(new Position(35.868344, 128.578239)); //     5지구
			WGMarketMap.ShapeCoordinates.Add(new Position(35.868150, 128.578100)); //     5지구
			WGMarketMap.ShapeCoordinates.Add(new Position(35.867876, 128.578124)); //     5지구
			WGMarketMap.ShapeCoordinates.Add(new Position(35.867772, 128.578225)); //     5지구
			WGMarketMap.ShapeCoordinates.Add(new Position(35.867816, 128.578844)); //     5지구
			WGMarketMap.ShapeCoordinates.Add(new Position(35.868368, 128.578799)); //     5지구
			WGMarketMap.ShapeCoordinates.Add(new Position(35.868594, 128.578901)); //  2지구
			WGMarketMap.ShapeCoordinates.Add(new Position(35.868671, 128.5801828)); //  2지구


            WGMarketMap.ShapeCoordinates.Add(new Position(35.868422, 128.579632)); //건해산물상가

            WGMarketMap.ShapeCoordinates.Add(new Position(35.868204, 128.579709)); //건해산물상가

            WGMarketMap.ShapeCoordinates.Add(new Position(35.868365, 128.581040)); //건해산물상가 OK



            WGMarketMap.ShapeCoordinates.Add(new Position(35.868555, 128.581540)); //건해산물상가OK



			

			WGMarketMap.ShapeCoordinates.Add(new Position(35.866887, 128.58244)); //4지구 대체 상가
			WGMarketMap.ShapeCoordinates.Add(new Position(35.866476, 128.582440)); //4지구 대체 상가
			WGMarketMap.ShapeCoordinates.Add(new Position(35.866472, 128.582837)); //4지구 대체 상가
			WGMarketMap.ShapeCoordinates.Add(new Position(35.866887, 128.582818)); //4지구 대체 상가
			WGMarketMap.ShapeCoordinates.Add(new Position(35.866887, 128.58244)); //4지구 대체 상가






			WGMarketMap.ShapeCoordinates.Add(new Position(35.868555, 128.581540)); //건해산물상가OK
			WGMarketMap.ShapeCoordinates.Add(new Position(35.868422, 128.579632)); //건해산물상가

            WGMarketMap.ShapeCoordinates.Add(new Position(35.868671, 128.580183)); //  2지구
			WGMarketMap.ShapeCoordinates.Add(new Position(35.869008, 128.580153)); //  2지구
			WGMarketMap.ShapeCoordinates.Add(new Position(35.869137, 128.580168)); // 1지구
			WGMarketMap.ShapeCoordinates.Add(new Position(35.869154, 128.580377)); //   4지구
		  //WGMarketMap.ShapeCoordinates.Add(new Position(35.869205, 128.581192)); //   4지구????
			WGMarketMap.ShapeCoordinates.Add(new Position(35.869205, 128.58119169)); //   4지구????yogi
			WGMarketMap.ShapeCoordinates.Add(new Position(35.869070, 128.581139)); // 상가연합회yogi
			WGMarketMap.ShapeCoordinates.Add(new Position(35.869022, 128.580369)); // 상가연합회
			WGMarketMap.ShapeCoordinates.Add(new Position(35.868706, 128.580396)); // 상가연합회
			WGMarketMap.ShapeCoordinates.Add(new Position(35.868751, 128.581173)); // 상가연합회
			WGMarketMap.ShapeCoordinates.Add(new Position(35.869070, 128.581139)); // 상가연합회
			WGMarketMap.ShapeCoordinates.Add(new Position(35.869107, 128.581328)); //  동산상가
			WGMarketMap.ShapeCoordinates.Add(new Position(35.868736, 128.581363)); //  동산상가

			WGMarketMap.ShapeCoordinates.Add(new Position(35.868779, 128.582042)); //  동산상가
			WGMarketMap.ShapeCoordinates.Add(new Position(35.868835, 128.582094)); //  동산상가
			WGMarketMap.ShapeCoordinates.Add(new Position(35.869158, 128.582090)); //  동산상가
			WGMarketMap.ShapeCoordinates.Add(new Position(35.869107, 128.581328)); //  동산상가
			WGMarketMap.ShapeCoordinates.Add(new Position(35.869218, 128.581310045)); // 아진상가???
			WGMarketMap.ShapeCoordinates.Add(new Position(35.869274, 128.582119)); // 아진상가
			WGMarketMap.ShapeCoordinates.Add(new Position(35.869788, 128.582090)); // 아진상가
			WGMarketMap.ShapeCoordinates.Add(new Position(35.869736, 128.581262)); // 아진상가
			WGMarketMap.ShapeCoordinates.Add(new Position(35.869218, 128.581310)); // 아진상가
			WGMarketMap.ShapeCoordinates.Add(new Position(35.869107, 128.581328)); //  동산상가
			WGMarketMap.ShapeCoordinates.Add(new Position(35.869070, 128.581139)); // 상가연합회jogi
			WGMarketMap.ShapeCoordinates.Add(new Position(35.869205, 128.581192)); //   4지구jogi
			WGMarketMap.ShapeCoordinates.Add(new Position(35.869739, 128.581140)); //   4지구!!!

            WGMarketMap.ShapeCoordinates.Add(new Position(35.869826, 128.581137)); //명품프라자
            WGMarketMap.ShapeCoordinates.Add(new Position(35.870096, 128.581101)); //명품프라자
            WGMarketMap.ShapeCoordinates.Add(new Position(35.870021, 128.580232)); //명품프라자
            WGMarketMap.ShapeCoordinates.Add(new Position(35.869770, 128.580256)); //명품프라자
            WGMarketMap.ShapeCoordinates.Add(new Position(35.869826, 128.581137)); //명품프라자

            WGMarketMap.ShapeCoordinates.Add(new Position(35.869739, 128.581140)); //   4지구
			WGMarketMap.ShapeCoordinates.Add(new Position(35.869685, 128.580332)); //   4지구
			WGMarketMap.ShapeCoordinates.Add(new Position(35.869154, 128.580377)); //   4지구
			WGMarketMap.ShapeCoordinates.Add(new Position(35.869137, 128.580168)); // 1지구
			WGMarketMap.ShapeCoordinates.Add(new Position(35.869672, 128.580127)); // 1지구
			WGMarketMap.ShapeCoordinates.Add(new Position(35.869639, 128.579596)); // 1지구
			WGMarketMap.ShapeCoordinates.Add(new Position(35.869564, 128.579522)); // 1지구
			WGMarketMap.ShapeCoordinates.Add(new Position(35.869096, 128.579571)); // 1지구
			WGMarketMap.ShapeCoordinates.Add(new Position(35.869137, 128.580168)); // 1지구
			WGMarketMap.ShapeCoordinates.Add(new Position(35.869008, 128.580153)); //  2지구
			WGMarketMap.ShapeCoordinates.Add(new Position(35.868936, 128.578874)); //  2지구
			WGMarketMap.ShapeCoordinates.Add(new Position(35.868594, 128.578901)); //  2지구
			WGMarketMap.ShapeCoordinates.Add(new Position(35.868368, 128.578799)); //     5지구

			var pin = new AdvertisementPin
			{
				Pin = new Pin
				{
					Type = PinType.Place,
					Position = new Position(35.869384, 128.579845),
					Label = "1지구",
					Address = "상가 정보 없음"
				},
				Id = "1지구",
                width = 40
			};

			WGMarketMap.AdvertisementPins = new List<AdvertisementPin> { pin };
			WGMarketMap.Pins.Add(pin.Pin);

			pin = new AdvertisementPin
			{
				Pin = new Pin
				{
					Type = PinType.Place,
					Position = new Position(35.868801, 128.5795285),
					Label = "2지구",
					Address = "서문시장"
				},
				Id = "2지구",
				width = 40
			};

			WGMarketMap.AdvertisementPins.Add(pin);
			WGMarketMap.Pins.Add(pin.Pin);

			pin = new AdvertisementPin
			{
				Pin = new Pin
				{
					Type = PinType.Place,
					Position = new Position(35.8694465, 128.580762),
					Label = "4지구",
					Address = "화재 현장 복구 중"
				},
				Id = "4지구",
				width = 40
			};

			WGMarketMap.AdvertisementPins.Add(pin);
			WGMarketMap.Pins.Add(pin.Pin);


			pin = new AdvertisementPin
			{
				Pin = new Pin
				{
					Type = PinType.Place,
					Position = new Position(35.86807, 128.578472),
					Label = "5지구",
					Address = "서문시장"
				},
				Id = "5지구",
				width = 40
			};

			WGMarketMap.AdvertisementPins.Add(pin);
			WGMarketMap.Pins.Add(pin.Pin);


			pin = new AdvertisementPin
			{
				Pin = new Pin
				{
					Type = PinType.Place,
					Position = new Position(35.868947, 128.581711),
					Label = "동산상가",
					Address = "서문시장"
				},
				Id = "동산상가",
				width = 60
			};

			WGMarketMap.AdvertisementPins.Add(pin);
			WGMarketMap.Pins.Add(pin.Pin);


			pin = new AdvertisementPin
			{
				Pin = new Pin
				{
					Type = PinType.Place,
					Position = new Position(35.869933, 128.5806845),
					Label = "명품프라자",
					Address = "상가 정보 없음"
				},
				Id = "명품프라자",
				width = 80
			};

			WGMarketMap.AdvertisementPins.Add(pin);
			WGMarketMap.Pins.Add(pin.Pin);


			pin = new AdvertisementPin
			{
				Pin = new Pin
				{
					Type = PinType.Place,
					Position = new Position(35.868888, 128.580771),
					Label = "상가연합회",
					Address = "서문시장"
				},
				Id = "상가연합회",
				width = 80
			};

			WGMarketMap.AdvertisementPins.Add(pin);
			WGMarketMap.Pins.Add(pin.Pin);


			pin = new AdvertisementPin
			{
				Pin = new Pin
				{
					Type = PinType.Place,
					Position = new Position(35.8683795, 128.580586),
					Label = "건해산물상가",
					Address = "상가 정보 없음"
				},
				Id = "건해산물상가",
				width = 100
			};

			WGMarketMap.AdvertisementPins.Add(pin);
			WGMarketMap.Pins.Add(pin.Pin);


			pin = new AdvertisementPin
			{
				Pin = new Pin
				{
					Type = PinType.Place,
					Position = new Position(35.869503, 128.5816905),
					Label = "아진상가",
					Address = "상가 정보 없음"
				},
				Id = "아진상가",
				width = 60
			};

			WGMarketMap.AdvertisementPins.Add(pin);
			WGMarketMap.Pins.Add(pin.Pin);


			pin = new AdvertisementPin
			{
				Pin = new Pin
				{
					Type = PinType.Place,
					Position = new Position(35.8666795, 128.5826385),
					Label = "4지구 대체상가",
					Address = "상가 정보 없음"
				},
				Id = "4지구 대체상가",
				width = 120
			};

			WGMarketMap.AdvertisementPins.Add(pin);
			WGMarketMap.Pins.Add(pin.Pin);


			onProcessing = false;
		}

        protected override async void OnAppearing()
		{
			moreButtonCount = 0;
			likeMoreButtonCount = 0;
            myRecent.Children.Clear();
            myLike.Children.Clear();

            recentShopInfo = new List<string>();
			recentSource = await App.Client.InvokeApiAsync<List<RecentEntity>>("recent", System.Net.Http.HttpMethod.Get, null);

			var LoadingLabel = new Label()
			{
				Text = "찜한 소식을 불러오고 있습니다..."
			};
			myLike.Children.Add(LoadingLabel);

			if (recentSource.Count > 0)
			{
                recentSource.Reverse();

				for (int i = 0; i < 10 && i < recentSource.Count; i++)
				{

					var ownerID = recentSource[i].ID;
					var blobName = recentSource[i].RowKey;

					imageURL = "https://westgateproject.blob.core.windows.net/" + ownerID.Split('@')[0] + "/" + blobName;

					switch (Device.RuntimePlatform)
					{
						case Device.Android:
							var myImage_Android = new Image { Aspect = Aspect.AspectFit, HeightRequest = App.ScreenWidth };
							//var imageByte = await DependencyService.Get<IImageScaleHelper>().GetImageStream(imageURL);
							//myImage_Android.Source = ImageSource.FromStream(() => new MemoryStream(imageByte));

							myImage_Android.Source = ImageSource.FromUri(new Uri(imageURL));
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
							myRecent.Children.Add(myImage_Android);
							break;
						case Device.iOS:
							var myImage_iOS = new Image { Aspect = Aspect.AspectFit, HeightRequest = App.ScreenWidth };
							myImage_iOS.Source = ImageSource.FromUri(new Uri(imageURL));
							myRecent.Children.Add(myImage_iOS);
							break;
					}

                    var labelButton = new Label()
					{
                        Text = recentSource[i].ShopName + " : " + recentSource[i].Context,
                        TextColor = Color.Blue
                    };
                    var tapLabelButton = new TapGestureRecognizer();
                    tapLabelButton.Tapped += LabelButton_Clicked;
                    labelButton.GestureRecognizers.Add(tapLabelButton);


                    if(labelButton.Text.Length > 20)
					{
                        labelButton.Text = labelButton.Text.Substring(0, 20) + "...";
                    }
					myRecent.Children.Add(labelButton);

                    recentShopInfo.Add(ownerID + ":" + recentSource[i].ShopName);

					var myBoxView = new BoxView()
					{
						HeightRequest = 10,
						BackgroundColor = Color.LightGray
					};
					myRecent.Children.Add(myBoxView);
				}
				if (recentSource.Count > 10)
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



            likeInfoList = new List<string>();
			Dictionary<string, string> getParamUserInfo = new Dictionary<string, string>
			{
				{ "userId", App.userEmail},
			};
            likeEntity = await App.Client.InvokeApiAsync<List<LikeEntity>>("likeContents", System.Net.Http.HttpMethod.Get, getParamUserInfo);
            LoadingLabel.IsVisible = false;
            if (likeEntity.Count > 0)
            {
                likeEntity.Reverse();
                likeNumList = new List<int>();
                for (int i = 0; i < 10 && i < likeEntity.Count; i++)
                {
					getParamUserInfo = new Dictionary<string, string>
					{
						{ "shopOwner", likeEntity[i].PartitionKey },
						{ "blobName", likeEntity[i].RowKey },
						{ "userID", App.userEmail.Split('@')[0] },
					};
					var content = await App.Client.InvokeApiAsync<ContentsEntity>("getShopContent", System.Net.Http.HttpMethod.Get, getParamUserInfo);


					var contentImageURL = "https://westgateproject.blob.core.windows.net/" + content.PartitionKey.Split('@')[0] + "/" + content.RowKey;
					switch (Device.RuntimePlatform)
					{
						case Device.Android:
							var myImage_Android = new Image { Aspect = Aspect.AspectFit, HeightRequest = App.ScreenWidth };
							//var imageByte = await DependencyService.Get<IImageScaleHelper>().GetImageStream(contentImageURL);
							//myImage_Android.Source = ImageSource.FromStream(() => new MemoryStream(imageByte));
							myImage_Android.Source = ImageSource.FromUri(new Uri(contentImageURL));
							string OrientationOfImage = await DependencyService.Get<IImageScaleHelper>().OrientationOfImage(contentImageURL);
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
							myLike.Children.Add(myImage_Android);
							break;
						case Device.iOS:
							var myImage_iOS = new Image { Aspect = Aspect.AspectFit, HeightRequest = App.ScreenWidth };
							myImage_iOS.Source = ImageSource.FromUri(new Uri(contentImageURL));
							myLike.Children.Add(myImage_iOS);
							break;
					}



					var layout = new StackLayout()
					{
						Orientation = StackOrientation.Horizontal
					};
					var heartEmtpyIcon = new Image { Source = "HeartEmpty.png" };
					var heartFilledIcon = new Image { Source = "HeartFilled.png" };
					var likeNumber = new Label
					{
						Text = content.Like.ToString(),
						VerticalTextAlignment = TextAlignment.Center
					};
					likeNumList.Add(content.Like);
					likeInfoList.Add(content.PartitionKey + ":" + content.RowKey + ":" + content.ShopName);
					var shopInfoLabel = new Label()
					{
						Text = "HeartEmpty",
						IsVisible = false
					};

					switch (content.LikeMember)
					{
						case "True":
							heartFilledIcon.IsVisible = true;
							heartEmtpyIcon.IsVisible = false;
							shopInfoLabel.Text = "HeartFilled";
							break;
						case "False":
							heartFilledIcon.IsVisible = false;
							heartEmtpyIcon.IsVisible = true;
							shopInfoLabel.Text = "HeartEmpty";
							break;

					}
					layout.Children.Add(heartEmtpyIcon);
					layout.Children.Add(heartFilledIcon);
					layout.Children.Add(likeNumber);
					layout.Children.Add(shopInfoLabel);
					var heartTapGestureRecognizer = new TapGestureRecognizer();
					heartTapGestureRecognizer.Tapped += async (s, e) =>
					{
						var thisLayout = s as StackLayout;
						var indexOfThisLayout = myLike.Children.IndexOf(thisLayout) / 4;
						var heartEmpty = thisLayout.Children[0] as Image;
						var heartFilled = thisLayout.Children[1] as Image;
						var likeNum = thisLayout.Children[2] as Label;
						var imgSource = thisLayout.Children[3] as Label;
						var likeInfo = likeInfoList[indexOfThisLayout].Split(':');
						switch (imgSource.Text)
						{
							case "HeartFilled":
								heartEmpty.IsVisible = true;
								heartFilled.IsVisible = false;
								likeNum.Text = (--likeNumList[indexOfThisLayout]).ToString();
								imgSource.Text = "HeartEmpty";
								await SyncData.UpdateLikeNum(likeInfo[0], likeInfo[1], App.userEmail.Split('@')[0], "down");
								break;
							default:
								heartEmpty.IsVisible = false;
								heartFilled.IsVisible = true;
								likeNum.Text = (++likeNumList[indexOfThisLayout]).ToString();
								imgSource.Text = "HeartFilled";
								await SyncData.UpdateLikeNum(likeInfo[0], likeInfo[1], App.userEmail.Split('@')[0], "up");
								break;

						}
					};
					layout.GestureRecognizers.Add(heartTapGestureRecognizer);

					myLike.Children.Add(layout);

					var myLabel = new Label()
					{
						Text = content.ShopName + ":" + content.Context,
						TextColor = Color.Blue
					};
					var tapLabelButton = new TapGestureRecognizer();
					tapLabelButton.Tapped += LikeLabel_Clicked;
					myLabel.GestureRecognizers.Add(tapLabelButton);
					myLike.Children.Add(myLabel);

					var myBoxView = new BoxView()
					{
						HeightRequest = 10,
						BackgroundColor = Color.LightGray
					};
					myLike.Children.Add(myBoxView);
                }

				if (likeEntity.Count > 10)
				{

					var labelButton = new Button()
					{
						Text = "더 불러오기"
					};
					labelButton.Clicked += LikeMoreButton_Clicked;
					myLike.Children.Add(labelButton);
				}

            }
            else
            {

				var myLabel = new Label()
				{
					Text = "찜한 소식이 없습니다."
				};
				myLike.Children.Add(myLabel);
            }







        }


        async void OnItemClicked(object sender, EventArgs args)
        {
            if (!onProcessing)
            {
				onProcessing = true;
				List<UserInfoEntity> userInfo = new List<UserInfoEntity>();
				Dictionary<string, string> getParamUserInfo = new Dictionary<string, string>
    			{
    				{ "id", App.userEmail},
    			};

				userInfo = await App.Client.InvokeApiAsync<List<UserInfoEntity>>("userInformation", System.Net.Http.HttpMethod.Get, getParamUserInfo);
                if (userInfo.Count > 0)
                {
					await Navigation.PushAsync(new WritingPage(userInfo));
                }
                else
                {
                    await DisplayAlert("내 매장이 없습니다.", "내 매장을 등록하시면 이용 하실 수 있습니다.", "확인");
                }
                onProcessing = false;
            }
        }

		private class ActivityIndicatorScope : IDisposable
		{
			private bool showIndicator;
			private ActivityIndicator indicator;
			private Task indicatorDelay;

			public ActivityIndicatorScope(ActivityIndicator indicator, bool showIndicator)
			{
				this.indicator = indicator;
				this.showIndicator = showIndicator;

				if (showIndicator)
				{
					indicatorDelay = Task.Delay(0);
					SetIndicatorActivity(true);
				}
				else
				{
					indicatorDelay = Task.FromResult(0);
				}
			}

			private void SetIndicatorActivity(bool isActive)
			{
				this.indicator.IsVisible = isActive;
				this.indicator.IsRunning = isActive;
			}

			public void Dispose()
			{
				if (showIndicator)
				{
					indicatorDelay.ContinueWith(t => SetIndicatorActivity(false), TaskScheduler.FromCurrentSynchronizationContext());
				}
			}
		}

		private async void LikeLabel_Clicked(object sender, EventArgs e)
		{
			var senderButton = sender as Label;
			senderButton.IsEnabled = false;
			int senderIndex = myLike.Children.IndexOf(senderButton) / 3;
            var likeInfo = likeInfoList[senderIndex].Split(':');

			var id = likeInfo[0];
			var shopName = likeInfo[2];

			IDictionary<string, string> getParam = new Dictionary<string, string>
			{
				{ "id", id},
				{ "shopName", shopName},
			};
            string shopInfo = "";
			shopInfo = await App.Client.InvokeApiAsync<string>("userInformation", System.Net.Http.HttpMethod.Get, getParam);

            var shopLocation = shopInfo.Split(':');
			string _building = "";
			switch (shopLocation[0])
			{
				case "Dongsan":
					_building = "동산상가";
					break;
				case "SecondBuilding":
					_building = "2지구";
					break;
				case "FifthBuilding":
					_building = "5지구";
					break;
			}
			await Navigation.PushAsync(new ShopInfoPage(_building, shopLocation[1], shopLocation[2]));
			senderButton.IsEnabled = true;
		}

		private async void LabelButton_Clicked(object sender, EventArgs e)
		{
			var senderButton = sender as Label;
			senderButton.IsEnabled = false;
			int senderIndex = myRecent.Children.IndexOf(senderButton) / 3;
			Debug.WriteLine("senderIndex : " + senderIndex);
            Debug.WriteLine("recentShopInfo[senderIndex] : " + recentShopInfo[senderIndex]);
            var id = recentShopInfo[senderIndex].Split(':')[0];
            var shopName = recentShopInfo[senderIndex].Split(':')[1];


			IDictionary<string, string> getParam = new Dictionary<string, string>
			{
				{ "id", id},
				{ "shopName", shopName},
			};
			Dictionary<string, string> shopInfo = new Dictionary<string, string>();
			shopInfo = await App.Client.InvokeApiAsync<Dictionary<string, string>>("recent", System.Net.Http.HttpMethod.Get, getParam);

			Debug.WriteLine("building : " + shopInfo["building"] + "floor : " + shopInfo["floor"] + "location : " + shopInfo["location"]);
            string _building = "";
            switch (shopInfo["building"])
            {
                case "Dongsan":
                    _building = "동산상가";
                    break;
                case "SecondBuilding":
                    _building = "2지구";
                    break;
                case "FifthBuilding":
                    _building = "5지구";
                    break;
            }
			await Navigation.PushAsync(new ShopInfoPage(_building, shopInfo["floor"], shopInfo["location"]));
			senderButton.IsEnabled = true;
		}


		private async void MoreButton_Clicked(object sender, EventArgs e)
		{
			var senderButton = sender as Button;
			senderButton.IsVisible = false;

			moreButtonCount++;
			int startIndex = moreButtonCount * 10;
            for (int i = startIndex; (i < startIndex + 10) && (i < recentSource.Count); i++)
			{
				var ownerID = recentSource[i].ID;
				var blobName = recentSource[i].RowKey;

				imageURL = "https://westgateproject.blob.core.windows.net/" + ownerID.Split('@')[0] + "/" + blobName;

				switch (Device.RuntimePlatform)
				{
					case Device.Android:
						var myImage_Android = new Image { Aspect = Aspect.AspectFit, HeightRequest = App.ScreenWidth };
						//var imageByte = await DependencyService.Get<IImageScaleHelper>().GetImageStream(imageURL);
						//myImage_Android.Source = ImageSource.FromStream(() => new MemoryStream(imageByte));
						myImage_Android.Source = ImageSource.FromUri(new Uri(imageURL));

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


						myRecent.Children.Add(myImage_Android);


						break;
					case Device.iOS:
						var myImage_iOS = new Image { Aspect = Aspect.AspectFit, HeightRequest = App.ScreenWidth };
						myImage_iOS.Source = ImageSource.FromUri(new Uri(imageURL));
						myRecent.Children.Add(myImage_iOS);
						break;
				}

				var labelButton = new Label()
				{
					Text = recentSource[i].ShopName + " : " + recentSource[i].Context,
					TextColor = Color.Blue
				};
				var tapLabelButton = new TapGestureRecognizer();
				tapLabelButton.Tapped += LabelButton_Clicked;
				labelButton.GestureRecognizers.Add(tapLabelButton);

				if (labelButton.Text.Length > 20)
				{
					labelButton.Text = labelButton.Text.Substring(0, 20) + "...";
				}
				myRecent.Children.Add(labelButton);


				recentShopInfo.Add(ownerID + ":" + recentSource[i].ShopName);

				var myBoxView = new BoxView()
				{
					HeightRequest = 10,
					BackgroundColor = Color.LightGray
				};
				myRecent.Children.Add(myBoxView);
			}

			if (recentSource.Count > (moreButtonCount + 1) * 10)
			{
				var labelButton = new Button()
				{
					Text = "더 불러오기"
				};
				labelButton.Clicked += MoreButton_Clicked;
				myRecent.Children.Add(labelButton);
			}
		}

        private async void LikeMoreButton_Clicked(object sender, EventArgs e)
        {
            var senderButton = sender as Button;
            senderButton.IsVisible = false;

            likeMoreButtonCount++;
            int startIndex = likeMoreButtonCount * 10;

            for (int i = startIndex; (i < startIndex + 10) && i < likeEntity.Count; i++)
            {
                var getParamUserInfo = new Dictionary<string, string>
                    {
                        { "shopOwner", likeEntity[i].PartitionKey },
                        { "blobName", likeEntity[i].RowKey },
                        { "userID", App.userEmail.Split('@')[0] },
                    };
                var content = await App.Client.InvokeApiAsync<ContentsEntity>("getShopContent", System.Net.Http.HttpMethod.Get, getParamUserInfo);


                var contentImageURL = "https://westgateproject.blob.core.windows.net/" + content.PartitionKey.Split('@')[0] + "/" + content.RowKey;
                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                        var myImage_Android = new Image { Aspect = Aspect.AspectFit, HeightRequest = App.ScreenWidth };
      //                  var imageByte = await DependencyService.Get<IImageScaleHelper>().GetImageStream(contentImageURL);
						//myImage_Android.Source = ImageSource.FromStream(() => new MemoryStream(imageByte));
						myImage_Android.Source = ImageSource.FromUri(new Uri(contentImageURL));
                        string OrientationOfImage = await DependencyService.Get<IImageScaleHelper>().OrientationOfImage(contentImageURL);
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
                                tapGestureRecognizer.Tapped += (s, ee) =>
                                {
                                    var img = s as Image;
                                    img.Rotation += 90;
                                };
                                myImage_Android.GestureRecognizers.Add(tapGestureRecognizer);
                                break;
                        }
                        myLike.Children.Add(myImage_Android);
                        break;
                    case Device.iOS:
                        var myImage_iOS = new Image { Aspect = Aspect.AspectFit, HeightRequest = App.ScreenWidth };
                        myImage_iOS.Source = ImageSource.FromUri(new Uri(contentImageURL));
                        myLike.Children.Add(myImage_iOS);
                        break;
                }



                var layout = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal
                };
                var heartEmtpyIcon = new Image { Source = "HeartEmpty.png" };
                var heartFilledIcon = new Image { Source = "HeartFilled.png" };
                var likeNumber = new Label
                {
                    Text = content.Like.ToString(),
                    VerticalTextAlignment = TextAlignment.Center
                };
                likeNumList.Add(content.Like);
                likeInfoList.Add(content.PartitionKey + ":" + content.RowKey + ":" + content.ShopName);
                var shopInfoLabel = new Label()
                {
                    Text = "HeartEmpty",
                    IsVisible = false
                };

                switch (content.LikeMember)
                {
                    case "True":
                        heartFilledIcon.IsVisible = true;
                        heartEmtpyIcon.IsVisible = false;
                        shopInfoLabel.Text = "HeartFilled";
                        break;
                    case "False":
                        heartFilledIcon.IsVisible = false;
                        heartEmtpyIcon.IsVisible = true;
                        shopInfoLabel.Text = "HeartEmpty";
                        break;

                }
                layout.Children.Add(heartEmtpyIcon);
                layout.Children.Add(heartFilledIcon);
                layout.Children.Add(likeNumber);
                layout.Children.Add(shopInfoLabel);
                var heartTapGestureRecognizer = new TapGestureRecognizer();
                heartTapGestureRecognizer.Tapped += async (s, ee) =>
                {
                    var thisLayout = s as StackLayout;
                    var indexOfThisLayout = myLike.Children.IndexOf(thisLayout) / 4;
                    var heartEmpty = thisLayout.Children[0] as Image;
                    var heartFilled = thisLayout.Children[1] as Image;
                    var likeNum = thisLayout.Children[2] as Label;
                    var imgSource = thisLayout.Children[3] as Label;
                    var likeInfo = likeInfoList[indexOfThisLayout].Split(':');
                    switch (imgSource.Text)
                    {
                        case "HeartFilled":
                            heartEmpty.IsVisible = true;
                            heartFilled.IsVisible = false;
                            likeNum.Text = (--likeNumList[indexOfThisLayout]).ToString();
                            imgSource.Text = "HeartEmpty";
                            await SyncData.UpdateLikeNum(likeInfo[0], likeInfo[1], App.userEmail.Split('@')[0], "down");
                            break;
                        default:
                            heartEmpty.IsVisible = false;
                            heartFilled.IsVisible = true;
                            likeNum.Text = (++likeNumList[indexOfThisLayout]).ToString();
                            imgSource.Text = "HeartFilled";
                            await SyncData.UpdateLikeNum(likeInfo[0], likeInfo[1], App.userEmail.Split('@')[0], "up");
                            break;

                    }
                };
                layout.GestureRecognizers.Add(heartTapGestureRecognizer);

                myLike.Children.Add(layout);

                var myLabel = new Label()
                {
                    Text = content.ShopName + ":" + content.Context,
                    TextColor = Color.Blue
                };
                var tapLabelButton = new TapGestureRecognizer();
                tapLabelButton.Tapped += LikeLabel_Clicked;
                myLabel.GestureRecognizers.Add(tapLabelButton);
                myLike.Children.Add(myLabel);

                var myBoxView = new BoxView()
                {
                    HeightRequest = 10,
                    BackgroundColor = Color.LightGray
                };
                myLike.Children.Add(myBoxView);
            }

            if (likeEntity.Count > (likeMoreButtonCount + 1) * 10)
            {
                var labelButton = new Button()
                {
                    Text = "더 불러오기"
                };
                labelButton.Clicked += LikeMoreButton_Clicked;
                myLike.Children.Add(labelButton);
            }
        }

		protected override bool OnBackButtonPressed()
		{
			MessagingCenter.Send<object>(this, "ActivityFinish");
			return true;
		}

	}
}
