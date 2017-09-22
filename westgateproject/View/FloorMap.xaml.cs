using System;
using System.Collections.Generic;
using westgateproject.Helper;
using Xamarin.Forms;

namespace westgateproject.View
{
    public partial class FloorMap : ContentPage
	{
        static public float scale;
		public bool onProcessing;
		bool backTouched;
		static public double maxScaleValue;
		static public double minScaleValue;
        string _building;
		string _floor;
		double currentScale = 1;
		double startScale = 1;
		int width = 0;
		int height = 0;
        bool isInitial;
        BoxView boundaryBox;


		protected override bool OnBackButtonPressed()
		{
			if (!backTouched)
			{
				backTouched = true;
				Navigation.PopAsync();
			}
			return true;
		}

        public FloorMap(string building, string floor)
        {
			InitializeComponent();
			backTouched = false;
			onProcessing = false;
			Title = floor;
            _building = building;
            _floor = floor;
			var pinchGesture = new PinchGestureRecognizer();
			pinchGesture.PinchUpdated += OnPinchUpdated;
			absL.GestureRecognizers.Add(pinchGesture);
            isInitial = true;

			MessagingCenter.Subscribe<object>(this, "PinchStart", (sender) =>
			{
                System.Diagnostics.Debug.WriteLine("PichStart");
                OnPinchUpdated(this, new PinchGestureUpdatedEventArgs(GestureStatus.Started, 1, new Point(0, 0)));

			});
			MessagingCenter.Subscribe<object>(this, "PinchRunning", (sender) =>
			{
				System.Diagnostics.Debug.WriteLine("PinchRunning");
				OnPinchUpdated(this, new PinchGestureUpdatedEventArgs(GestureStatus.Running, scale, new Point(0, 0)));

			});
			MessagingCenter.Subscribe<object>(this, "PinchCompleted", (sender) =>
			{
				System.Diagnostics.Debug.WriteLine("PinchCompleted");
				OnPinchUpdated(this, new PinchGestureUpdatedEventArgs(GestureStatus.Completed, 1, new Point(0, 0)));

			});
        }

		protected async override void OnAppearing()
		{
            if (isInitial)
            {
                isInitial = false;
            }
            else
            {
                return; 
            }
            
			var MapInfo = await SyncData.DownloadShopMapInfo(_building + _floor);
			if (MapInfo.Count == 0)
			{
                await DisplayAlert("매장 정보가 없습니다.", "매장 정보 없음", "확인");
				await Navigation.PopAsync();
                return;
			}
            System.Diagnostics.Debug.WriteLine("MapInfo Count : " + MapInfo.Count);
			foreach (var shopInfo in MapInfo)
			{
				Label shopLocation = new Label()
				{
					Text = shopInfo.RowKey,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Center,
					FontSize = shopInfo.FontSize
				};
				switch (shopInfo.BackgroundColor)
				{
					case "Aqua":
						shopLocation.BackgroundColor = Color.Aqua;
						break;
					case "Brown":
						shopLocation.BackgroundColor = Color.Brown;
						break;
					case "White":
						shopLocation.BackgroundColor = Color.White;
						break;
					default:
						shopLocation.BackgroundColor = Color.Default;
						break;
				}
				switch (shopInfo.TextColor)
				{
					case "Aqua":
						shopLocation.TextColor = Color.Aqua;
						break;
					case "Blue":
						shopLocation.TextColor = Color.Blue;
						break;
					case "White":
						shopLocation.TextColor = Color.White;
						break;
					case "Transparent":
						shopLocation.TextColor = Color.Transparent;
						break;
					default:
						shopLocation.TextColor = Color.Default;
						break;
				}

                if (!shopInfo.RowKey.Contains("계단"))
                {
                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += OnTapped;
                    shopLocation.GestureRecognizers.Add(tapGestureRecognizer);
                }
				AbsoluteLayout.SetLayoutBounds(shopLocation, new Rectangle(shopInfo.XPosition, shopInfo.YPosition, shopInfo.Width, shopInfo.Height));
				absL.Children.Add(shopLocation);

				width  = Math.Max(width,  (int)shopInfo.XPosition + (int)shopInfo.Width);
                height = Math.Max(height, (int)shopInfo.YPosition + (int)shopInfo.Height);
			}

			loadingLabel.IsVisible = false;
			switch (Device.RuntimePlatform)
			{
				case Device.Android:
					maxScaleValue = (App.ScreenHeight - 80) / height;
					break;
				default:
					maxScaleValue = (App.ScreenHeight - 65) / height;
					break;
			}
            minScaleValue = App.ScreenWidth / width;


              //System.Diagnostics.Debug.WriteLine("scaleValue : " + scaleValue);
			boundaryBox = new BoxView { Color = Color.Red };
            AbsoluteLayout.SetLayoutBounds(boundaryBox, new Rectangle(width * maxScaleValue , height * maxScaleValue, 0, 1));
			absL.Children.Add(boundaryBox);


		}
		async void OnTapped(object sender, EventArgs args)
		{
			if (!onProcessing)
			{
				System.Diagnostics.Debug.WriteLine("OnTapped Started");
				onProcessing = true;
				//ShopInforSQLDb infoFromSQLite = new ShopInforSQLDb();
				var temp = sender as Label;
				//infoFromSQLite = await App.Database.GetShopAsync("2지구", "1층", temp.Text);
				await Navigation.PushAsync(new ShopInfoPage(_building, _floor, temp.Text));
				onProcessing = false;
			}
		}
		void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
		{
			if (e.Status == GestureStatus.Started)
			{
                System.Diagnostics.Debug.WriteLine("OnPichUpdated Started");
				startScale = absL.Scale;
				absL.AnchorX = 0;
				absL.AnchorY = 0;
			}
			if (e.Status == GestureStatus.Running)
			{
                System.Diagnostics.Debug.WriteLine("scale value : " + scale);
				// Calculate the scale factor to be applied.
				currentScale += (e.Scale - 1) * startScale;
				currentScale = Math.Max(minScaleValue, currentScale);
				currentScale = Math.Min(currentScale, maxScaleValue);

				absL.Scale = currentScale;
			}
			if (e.Status == GestureStatus.Completed)
			{
                //int indexOfBoundaryBox = absL.Children.IndexOf(boundaryBox);
                //var temp = absL.Children[indexOfBoundaryBox].Bounds;
                //absL.Children[indexOfBoundaryBox].Layout(new Rectangle(width * currentScale, height * currentScale, 1, 1));
			}
		}
    }
}
