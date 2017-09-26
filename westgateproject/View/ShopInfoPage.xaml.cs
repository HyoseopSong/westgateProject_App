using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using westgateproject.Helper;
using westgateproject.Models;
using Xamarin.Forms;

namespace westgateproject.View
{
    public partial class ShopInfoPage : ContentPage, INotifyPropertyChanged
    {
        string _building;
        string _floor;
		string _location;
		string _shopOwnerID;
		string shopOwner;
		bool gotoRegister;
		bool backTouched;
		bool isInitial;
        bool onProcessing;
        bool dataLoading;
        int shopContentsPageNumber;

        const int numOfShopContentPage = 10;
        List<ContentsEntity> shopEntity = new List<ContentsEntity>();
        ObservableCollection<ContentsEntity> shopContents = new ObservableCollection<ContentsEntity>();

		protected override bool OnBackButtonPressed()
		{
			if (!backTouched)
			{
				backTouched = true;
				Navigation.PopAsync();
			}
			return true;
		}
        public ShopInfoPage()
        {
			InitializeComponent();
			backTouched = false;
			isInitial = true;
        }

        public ShopInfoPage(string building, string floor, string location)
        {
			InitializeComponent();
            _building = building;
            _floor = floor;
			_location = location;
            gotoRegister = false;
			NavigationPage.SetHasBackButton(this, false);
			shopLabel.Text += _building + " " + _floor + " " + _location;
			//Debug.WriteLine("shopLabel.Text = " + shopLabel.Text);

			backTouched = false;
			isInitial = true;

        }

		protected override async void OnAppearing()
		{
            if(gotoRegister)
            {
                await Navigation.PopAsync();
                return;
            }

			if (!isInitial)
			{
				Debug.WriteLine("OnAppearing if");
				return;
			}
			else
			{
				Debug.WriteLine("OnAppearing else");
				isInitial = false;
                onProcessing = false;
                dataLoading = false;
                shopContentsPageNumber = 0;
			}

            string _building_Converted;
            switch(_building)
            {
                case "동산상가":
                    _building_Converted = "Dongsan";
					break;
				case "2지구":
					_building_Converted = "SecondBuilding";
					break;
				case "5지구":
					_building_Converted = "FifthBuilding";
					break;
				default:
					_building_Converted = "Empty";
                    break;
            }
			Dictionary<string, string> getParam = new Dictionary<string, string>
			{
				{ "building", _building_Converted},
				{ "floor", _floor},
				{ "location", _location},
			};

			IDictionary<string, string> shopInfo = await App.Client.InvokeApiAsync<IDictionary<string, string>>("getShopInformation", System.Net.Http.HttpMethod.Get, getParam);
            

            if (shopInfo != null)
            {

				NavigationPage.SetHasBackButton(this, true);
 
                foreach (var temp in shopInfo)
				{
					Debug.WriteLine("temp.Key : " + temp.Key);
                    switch(temp.Key)
                    {
						case "shopName":
                            Debug.WriteLine("temp.Value : " + temp.Value);
                            this.Title = temp.Value;
							break;
 						case "shopOwner":
							Debug.WriteLine("temp.Value : " + temp.Value);
							shopOwner = temp.Value;
                            _shopOwnerID = shopOwner.Split('@')[0];
							break;
						case "phoneNumber":
							Debug.WriteLine("temp.Value : " + temp.Value);
							shopPhoneNumber.Text = temp.Value;
							break;
						case "homepage":
							Debug.WriteLine("temp.Value : " + temp.Value);
                            switch(temp.Value)
                            {
                                case "제작 완료":
									IdnMapping idn = new IdnMapping();
                                    var titleWithoutBlank = Title.Replace(" ", "");
                                    homePage.Text = "https://서문시장.net/" + titleWithoutBlank;
									homePage.Clicked += (object sender, EventArgs e) => {
										Device.OpenUri(new Uri("https://xn--z92bt1ipqb91i.net/" + titleWithoutBlank));
									};
                                    break;
                                default:
                                    shopHomepage.IsVisible = false;
                                    break;
                            }
							break;
                        case "notOnService":
                            if (App.userEmail == shopOwner)
                            {
                                await Navigation.PushAsync(new Register(_building, _floor, _location));
                                gotoRegister = true;
                                //Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                                break;
                            }
                            else
                            {
                                await DisplayAlert("등록 진행 중", "이미 등록 신청 된 매장입니다.", "확인");
                                await Navigation.PopAsync(true);
                                return;
                            }
    			    }
    			}



				Dictionary<string, string> getShopContentsParam = new Dictionary<string, string>
    			{
    				{ "shopOwner", _shopOwnerID},
    				{ "shopName", this.Title},
                    { "userID", App.userEmail.Split('@')[0] }
    			};

				shopEntity = await App.Client.InvokeApiAsync<List<ContentsEntity>>("getShopContents", System.Net.Http.HttpMethod.Get, getShopContentsParam);

				shopEntity.Reverse();
				ShopContentsListView.ItemsSource = shopContents;
                int ii = 0;
				foreach (var s in shopEntity)
                {
                    s.RowKey = "https://westgateproject.blob.core.windows.net/" + _shopOwnerID + "/" + s.RowKey;
                    switch(s.LikeMember)
                    {
                        case "True":
                            s.LikeMember = "HeartFilled.png";
                            break;
                        case "False":
                            s.LikeMember = "HeartEmpty.png";
                            break;
                    }
                    if (ii++ < numOfShopContentPage)
                    {
                        shopContents.Add(s);
                    }
				}

                ShopContentsListView.ItemAppearing += (object sender, ItemVisibilityEventArgs e) =>
                {
                    var item = e.Item as ContentsEntity;
                    int index = shopContents.IndexOf(item);
                    if(shopContents.Count - 2 <= index)
                    {
                        if(!dataLoading)
                        {
                            dataLoading = true;

                            shopContentsPageNumber++;
                            for (int i = shopContentsPageNumber * numOfShopContentPage; i < (shopContentsPageNumber + 1) * numOfShopContentPage && i < shopEntity.Count; i++)
                            {
                                shopContents.Add(shopEntity[i]);
                            }
                            dataLoading = false;
                        }
                    }
                };
            }
            else
            {
                var answer = await DisplayAlert("비어있는 매장", "내 매장으로 등록하시겠습니까?", "등록", "무시");
                if(answer)
                {
                    gotoRegister = true;
					await Navigation.PushAsync(new Register(_building, _floor, _location));
					//Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                    // Register process
                    // 1. Input relational information - Shop Name, Phone Number, if it doens't exist, additional information about shop location.
                    // 2. Touch Register button
                    // 3. Display Info window that indicate the process is done and you need to pay the money.
                    // 4. Touch OK button and then return to shop map.
                }
                else
                {
                    await Navigation.PopAsync(true);
                }

            }

		}

		async void OnCall(object sender, EventArgs e)
		{
            switch(Device.RuntimePlatform)
            {
				case Device.Android:
					if (await this.DisplayAlert(
							shopPhoneNumber.Text,
							"전화를 거시겠습니까?",
							"네",
							"아니오"))
					{
						var dialerAnd = DependencyService.Get<IDialer>();
						if (dialerAnd != null)
							dialerAnd.Dial(shopPhoneNumber.Text);
					}
                    break;
				case Device.iOS:
					var button = (Button)sender;
					var dialerIOS = DependencyService.Get<IDialer>();
					if (dialerIOS != null)
						dialerIOS.Dial(shopPhoneNumber.Text);
                    break;
            }


		}

        async void RefreshShopContents(object sender, EventArgs e)
        {
            shopContentsPageNumber = 0;


			Dictionary<string, string> getShopContentsParam = new Dictionary<string, string>
				{
					{ "shopOwner", _shopOwnerID},
					{ "shopName", this.Title},
					{ "userID", App.userEmail.Split('@')[0] }
				};

			shopEntity = await App.Client.InvokeApiAsync<List<ContentsEntity>>("getShopContents", System.Net.Http.HttpMethod.Get, getShopContentsParam);

			shopEntity.Reverse();
			shopContents.Clear();
            int i = 0;
			foreach (var s in shopEntity)
			{
				s.RowKey = "https://westgateproject.blob.core.windows.net/" + _shopOwnerID + "/" + s.RowKey;
				switch (s.LikeMember)
				{
					case "True":
						s.LikeMember = "HeartFilled.png";
						break;
					case "False":
						s.LikeMember = "HeartEmpty.png";
						break;
				}
                if (i++ < numOfShopContentPage)
                {
                    shopContents.Add(s);
                }
			}

            ShopContentsListView.IsRefreshing = false;
        }


		async void OnContentsSelection(object sender, SelectedItemChangedEventArgs e)
		{
			((ListView)sender).SelectedItem = null;
            if(!onProcessing)
            {
                onProcessing = true;
				Debug.WriteLine(((ListView)sender).SelectedItem);
				if (e.SelectedItem == null)
				{
                    onProcessing = false;
					return;
				}
				var item = (ContentsEntity)e.SelectedItem;
				int indexOfItem = shopContents.IndexOf(item);
				switch (shopContents[indexOfItem].LikeMember)
				{
					case "HeartFilled.png":
						shopContents[indexOfItem].LikeMember = "HeartEmpty.png";
						shopContents[indexOfItem].Like--;
						var blobNameOfFilled = shopContents[indexOfItem].RowKey.Split('/');
						await SyncData.UpdateLikeNum(shopOwner, blobNameOfFilled[blobNameOfFilled.Length - 1], App.userEmail.Split('@')[0], "down");
						break;
					case "HeartEmpty.png":
						shopContents[indexOfItem].LikeMember = "HeartFilled.png";
						shopContents[indexOfItem].Like++;
						var blobNameOfEmpty = shopContents[indexOfItem].RowKey.Split('/');
						await SyncData.UpdateLikeNum(shopOwner, blobNameOfEmpty[blobNameOfEmpty.Length - 1], App.userEmail.Split('@')[0], "up");
						break;
				}
                onProcessing = false;
            }

		}


		void ShopContentsSearch(object sender, TextChangedEventArgs e)
		{

			shopContentsComplete.IsVisible = true;
			shopContentsCancel.IsVisible = false;
			if (e.NewTextValue == "")
			{
				ShopContentsListView.ItemsSource = shopContents;
			}
			else
			{
				ObservableCollection<ContentsEntity> shopContentsSearchResult = new ObservableCollection<ContentsEntity>();
				ShopContentsListView.ItemsSource = shopContentsSearchResult;

				foreach (var r in shopContents)
				{
					if (r.Context.Contains(e.NewTextValue) || r.ShopName.Contains(e.NewTextValue))
					{
						shopContentsSearchResult.Add(r);
					}

				}
			}
		}

		void ShopContentsCancelClicked(object sender, EventArgs e)
		{
			shopContentsSearchEntry.Text = "";
			shopContentsCancel.IsVisible = false;
			shopContentsComplete.IsVisible = true;
		}

		void ShopContentsCompleteClicked(object sender, EventArgs e)
		{
            if(shopContentsSearchEntry.Text != "")
			{
				shopContentsCancel.IsVisible = true;
				shopContentsComplete.IsVisible = false;
            }
		}


		void OnSearchItemClicked(object sender, EventArgs args)
		{
			if (shopSearchWindow.IsVisible)
			{
				shopSearchWindow.IsVisible = false;
			}
			else
			{
				shopSearchWindow.IsVisible = true;
			}
		}


    }
}
