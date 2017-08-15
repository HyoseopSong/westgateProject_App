using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using westgateproject.View;
using System.Threading.Tasks;
using System.IO;
using westgateproject.Helper;

namespace westgateproject
{
	public partial class FirstPage : ContentPage
	{
		public bool onProcessing;

		public FirstPage()
		{
            InitializeComponent();

            NavigationPage.SetHasBackButton(this, false);
			WGMarketMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(35.8687915, 128.580085), Distance.FromKilometers(0.12)));
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
