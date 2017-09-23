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
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace westgateproject
{
	public partial class FirstPage : TabbedPage
	{
		public bool onProcessing;
        bool isInitial;
        bool dataLoading;
        int recentPageNumer;
        int likePageNumber;
        const int numOfRecentPage = 10;
        const int numOfLikepage = 3;

		List<RecentEntity> recentSource = new List<RecentEntity>();
		List<ContentsEntity> likeSource = new List<ContentsEntity>();
        List<ContentsEntity> ShopInfoList = new List<ContentsEntity>();
		ObservableCollection<ContentsEntity> likeContents = new ObservableCollection<ContentsEntity>();
		ObservableCollection<RecentEntity> recentContents = new ObservableCollection<RecentEntity>();

		public FirstPage()
		{
            isInitial = true;
            dataLoading = false;
            recentPageNumer = 0;
            likePageNumber = 0;
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
            if(!isInitial)
            {
                return;
            }
            isInitial = false;
			recentSource = await App.Client.InvokeApiAsync<List<RecentEntity>>("recent", System.Net.Http.HttpMethod.Get, null);
            recentSource.Reverse();

			foreach (var t in recentSource)
			{
				t.RowKey = "https://westgateproject.blob.core.windows.net/" + t.ID.Split('@')[0] + "/" + t.RowKey;
			}
            for (int i = 0; i < numOfRecentPage && i < recentSource.Count; i++)
            {
                recentContents.Add(recentSource[i]);
            }

			RecentListView.ItemsSource = recentContents;
            RecentListView.ItemAppearing += (object sender, ItemVisibilityEventArgs e) =>
            {
                var item = e.Item as RecentEntity;
                int index = recentContents.IndexOf(item);
                if(recentContents.Count - 2 <= index)
                {
                    if(!dataLoading)
                    {
                        dataLoading = true;

						recentPageNumer++;
                        for (int i = recentPageNumer * numOfRecentPage; i < (recentPageNumer + 1) * numOfRecentPage && i < recentSource.Count; i++)
                        {
                            recentContents.Add(recentSource[i]);
                        }

                        dataLoading = false;
                    }
                }
            };


			Dictionary<string, string> getParamUserInfo = new Dictionary<string, string>
			{
				{ "userId", App.userEmail},
			};
			List<LikeEntity> likeEntity = await App.Client.InvokeApiAsync<List<LikeEntity>>("likeContents", System.Net.Http.HttpMethod.Get, getParamUserInfo);
            likeEntity.Reverse();

			foreach (var temp in likeEntity)
			{
				getParamUserInfo = new Dictionary<string, string>
						{
							{ "shopOwner", temp.PartitionKey },
							{ "blobName", temp.RowKey },
							{ "userID", App.userEmail.Split('@')[0] },
						};
				var content = await App.Client.InvokeApiAsync<ContentsEntity>("getShopContent", System.Net.Http.HttpMethod.Get, getParamUserInfo);
				content.RowKey = "https://westgateproject.blob.core.windows.net/" + content.PartitionKey.Split('@')[0] + "/" + content.RowKey;
				likeSource.Add(content);
			}

            for (int i = 0; i < numOfLikepage && i < likeSource.Count; i++)
            {
                likeContents.Add(likeSource[i]);
            }


			LikeListView.ItemsSource = likeContents;
            LikeListView.ItemAppearing += (object sender, ItemVisibilityEventArgs e) =>
            {
                var item = e.Item as ContentsEntity;
                int index = likeContents.IndexOf(item);
                if(likeContents.Count -2 <= index)
                {
                    if(!dataLoading)
                    {
                        dataLoading = true;

                        likePageNumber++;
                        for (int i = likePageNumber * numOfLikepage; i < (likePageNumber + 1) * numOfLikepage && i < likeSource.Count; i++)
                        {
                            likeContents.Add(likeSource[i]);
                        }

                        dataLoading = false;
                    }
                }
            };
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

        async void OnRecentSelection(object sender, SelectedItemChangedEventArgs e)
		{
			((ListView)sender).SelectedItem = null;
            if (!onProcessing)
            {
                onProcessing = true;
                if (e.SelectedItem == null)
                {
                    return;
                }
                var item = (RecentEntity)e.SelectedItem;

                IDictionary<string, string> getParam = new Dictionary<string, string>
            {
                { "id", item.ID},
                { "shopName", item.ShopName},
            };
                Dictionary<string, string> shopInfo = new Dictionary<string, string>();
                shopInfo = await App.Client.InvokeApiAsync<Dictionary<string, string>>("recent", System.Net.Http.HttpMethod.Get, getParam);

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
                onProcessing = false;
            }
        }


		async void OnLikeSelection(object sender, SelectedItemChangedEventArgs e)
		{
			((ListView)sender).SelectedItem = null;
            if (!onProcessing)
            {
                onProcessing = true;
                if (e.SelectedItem == null)
                {
                    return;
                }
                var item = (ContentsEntity)e.SelectedItem;

                IDictionary<string, string> getParam = new Dictionary<string, string>
            {
                { "id", item.PartitionKey},
                { "shopName", item.ShopName},
            };
                string shopInfo = await App.Client.InvokeApiAsync<string>("userInformation", System.Net.Http.HttpMethod.Get, getParam);

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
                onProcessing = false;
            }
        }

        async void RefreshRecent(object sender, EventArgs e)
        {
			Debug.WriteLine("RefreshRecent");
            recentPageNumer = 0;
			recentSource = await App.Client.InvokeApiAsync<List<RecentEntity>>("recent", System.Net.Http.HttpMethod.Get, null);
			recentSource.Reverse();

			foreach (var t in recentSource)
			{
				t.RowKey = "https://westgateproject.blob.core.windows.net/" + t.ID.Split('@')[0] + "/" + t.RowKey;
			}
            recentContents.Clear();
			for (int i = 0; i < numOfRecentPage && i < recentSource.Count; i++)
			{
				recentContents.Add(recentSource[i]);
			}

			RecentListView.ItemsSource = recentContents;
			RecentListView.IsRefreshing = false;
        }

        async void RefreshLike(object sender, EventArgs e)
		{
            
			Debug.WriteLine("RefreshLike");
            likePageNumber = 0;
			Dictionary<string, string> getParamUserInfo = new Dictionary<string, string>
			{
				{ "userId", App.userEmail},
			};
			List<LikeEntity> likeEntity = await App.Client.InvokeApiAsync<List<LikeEntity>>("likeContents", System.Net.Http.HttpMethod.Get, getParamUserInfo);
			likeEntity.Reverse();

            likeSource.Clear();
			foreach (var temp in likeEntity)
			{
				getParamUserInfo = new Dictionary<string, string>
						{
							{ "shopOwner", temp.PartitionKey },
							{ "blobName", temp.RowKey },
							{ "userID", App.userEmail.Split('@')[0] },
						};
				var content = await App.Client.InvokeApiAsync<ContentsEntity>("getShopContent", System.Net.Http.HttpMethod.Get, getParamUserInfo);
				content.RowKey = "https://westgateproject.blob.core.windows.net/" + content.PartitionKey.Split('@')[0] + "/" + content.RowKey;
				likeSource.Add(content);
			}
            likeContents.Clear();
			for (int i = 0; i < numOfLikepage && i < likeSource.Count; i++)
			{
				likeContents.Add(likeSource[i]);
			}


			LikeListView.ItemsSource = likeContents;


			LikeListView.IsRefreshing = false;
        }
		protected override bool OnBackButtonPressed()
		{
			MessagingCenter.Send<object>(this, "ActivityFinish");
			return true;
		}

	}
}
