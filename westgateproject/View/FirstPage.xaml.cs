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
		Dictionary<string, RecentEntity> recentSource;
		string[] recentKeyArray;
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
					WGMarketMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(35.8687915, 128.580085), Distance.FromKilometers(0.185)));
                    break;
				case Device.iOS:
					//WGMarketMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(35.8687915, 128.580085), Distance.FromKilometers(0.16)));
                    break;
            }
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
					Address = "화살 5개"
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
					Address = "화살 5개"
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
					Address = "화살 5개"
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
					Address = "화살 5개"
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

			onProcessing = false;
			isInitial = true;

		}

        protected override async void OnAppearing()
        {
			if (!isInitial)
			{
				return;
			}
			else
			{
				isInitial = false;
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
			if (recentSource.Count > 0)
			{
				// imageSource에 있는 키값 배열에 다 넣고 앞에 10개만 보여주기
				recentKeyArray = new string[recentSource.Count];
				Debug.WriteLine("recentKeyArray.Length : " + recentKeyArray.Length);
				recentSource.Keys.CopyTo(recentKeyArray, 0);
				int startIndex = 0;
				if (recentKeyArray.Length > 10)
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
				if (recentKeyArray.Length > 10)
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


		private async void MoreButton_Clicked(object sender, EventArgs e)
		{
			var senderButton = sender as Button;
			senderButton.IsEnabled = false;

			moreButtonCount++;
			int startIndex = recentKeyArray.Length - (moreButtonCount * 10) - 1;
			if (startIndex < 0)
			{
				startIndex = 0;
			}
			Debug.WriteLine("startIndex : " + startIndex);

			for (int i = startIndex; (i > startIndex - 10) && i >= 0; i--)
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

			if (recentKeyArray.Length > (moreButtonCount + 1) * 10)
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

	}
}
