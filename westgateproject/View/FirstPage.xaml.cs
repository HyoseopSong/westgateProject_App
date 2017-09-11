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
		List<RecentEntity> recentSource;
		RecentEntity[] recentEntityArray;
		int moreButtonCount;
		String imageURL;
		bool isInitial;

		public FirstPage()
		{
            InitializeComponent();

			moreButtonCount = 0;

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
			isInitial = true;

		}

        protected override async void OnAppearing()
        {
			if (!isInitial)
			{
				myRecent.Children.Clear();
			}
			else
			{
				isInitial = false;
			}


			recentSource = new List<RecentEntity>();
			recentSource = await App.Client.InvokeApiAsync<List<RecentEntity>>("recent", System.Net.Http.HttpMethod.Get, null);
			//userInfo = await App.Client.InvokeApiAsync<List<UserInfoEntity>>("userInformation", System.Net.Http.HttpMethod.Get, getParamUserInfo);
			if (recentSource.Count > 0)
			{
				// imageSource에 있는 키값 배열에 다 넣고 앞에 10개만 보여주기
				recentEntityArray = new RecentEntity[recentSource.Count];
                recentSource.CopyTo(recentEntityArray, 0);
				int startIndex = 0;
				if (recentEntityArray.Length > 10)
				{
					startIndex = recentEntityArray.Length - 10;
				}
				else
				{
					startIndex = 0;
				}
				for (int i = startIndex; i < startIndex + 10 && i < recentEntityArray.Length; i++)
				{

					var ownerID = recentEntityArray[i].ID;
					var blobName = recentEntityArray[i].RowKey;

					imageURL = "https://westgateproject.blob.core.windows.net/" + ownerID.Split('@')[0] + "/" + blobName;

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

                    var labelButton = new Label()
					{
                        Text = recentEntityArray[i].ShopName + " : " + recentEntityArray[i].Context,
                        TextColor = Color.Blue
                    };
                    var tapLabelButton = new TapGestureRecognizer();
                    tapLabelButton.Tapped += LabelButton_Clicked;
                    labelButton.GestureRecognizers.Add(tapLabelButton);





					//var labelButton = new Button()
					//{
					//	Text = recentEntityArray[i].ShopName + " : " + recentEntityArray[i].Context,

					//};
					//labelButton.Clicked += LabelButton_Clicked;
                    if(labelButton.Text.Length > 20)
					{
                        labelButton.Text = labelButton.Text.Substring(0, 20) + "...";
                    }
					myRecent.Children.Insert(1, labelButton);




					var shopInfo = new Label()
					{
						Text = ownerID + ":" + recentEntityArray[i].ShopName,
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
				if (recentEntityArray.Length > 10)
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


        async void OnItemClicked(object sender, EventArgs args)
        {
            if (!onProcessing)
            {
                onProcessing = true;
                await Navigation.PushAsync(new WritingPage());
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



		private async void LabelButton_Clicked(object sender, EventArgs e)
		{
			var senderButton = sender as Label;
			senderButton.IsEnabled = false;
			int senderIndex = myRecent.Children.IndexOf(senderButton);
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
			senderButton.IsEnabled = false;

			moreButtonCount++;
			int startIndex = recentEntityArray.Length - (moreButtonCount * 10) - 1;
			if (startIndex < 0)
			{
				startIndex = 0;
			}
			Debug.WriteLine("startIndex : " + startIndex);

			for (int i = startIndex; (i > startIndex - 10) && i >= 0; i--)
			{
				Debug.WriteLine("for statement i : " + i);


				var ownerID = recentEntityArray[i].PartitionKey;
				var blobName = recentEntityArray[i].RowKey;

				imageURL = "https://westgateproject.blob.core.windows.net/" + ownerID.Split('@')[0] + "/" + blobName;

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

				var labelButton = new Label()
				{
					Text = recentEntityArray[i].ShopName + " : " + recentEntityArray[i].Context,
					TextColor = Color.Blue
				};
				var tapLabelButton = new TapGestureRecognizer();
				tapLabelButton.Tapped += LabelButton_Clicked;
				labelButton.GestureRecognizers.Add(tapLabelButton);





				//var labelButton = new Button()
				//{
				//  Text = recentEntityArray[i].ShopName + " : " + recentEntityArray[i].Context,

				//};
				//labelButton.Clicked += LabelButton_Clicked;
				if (labelButton.Text.Length > 20)
				{
					labelButton.Text = labelButton.Text.Substring(0, 20) + "...";
				}
				myRecent.Children.Insert(myRecent.Children.Count - 1, labelButton);

				var shopInfo = new Label()
				{
					Text = ownerID + ":" + recentEntityArray[i].ShopName,
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

			if (recentEntityArray.Length > (moreButtonCount + 1) * 10)
			{
				senderButton.IsVisible = true;
			}
			else
			{
				senderButton.IsVisible = false;
			}

			senderButton.IsEnabled = true;
		}
		//protected override void OnSizeAllocated(double width, double height)
		//{
		//	base.OnSizeAllocated(width, height);
		//	if (this.Width > this.Height)
		//	{
		//		layout.Orientation = StackOrientation.Horizontal;
		//	}
		//	else
		//	{
		//		layout.Orientation = StackOrientation.Vertical;
		//	}
		//}


		protected override bool OnBackButtonPressed()
		{
			MessagingCenter.Send<object>(this, "ActivityFinish");
			return true;
		}

	}
}
