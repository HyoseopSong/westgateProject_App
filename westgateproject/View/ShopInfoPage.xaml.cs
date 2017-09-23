using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
        //List<int> likeNumList = new List<int>();
        ObservableCollection<ContentsEntity> shopContents;

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
			//shopLabel.Text += _building + " " + _floor + " " + _location;
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





                //From here
				Dictionary<string, string> getShopContentsParam = new Dictionary<string, string>
    			{
    				{ "shopOwner", _shopOwnerID},
    				{ "shopName", this.Title},
                    { "userID", App.userEmail.Split('@')[0] }
    			};

				List<ContentsEntity> shopEntity = await App.Client.InvokeApiAsync<List<ContentsEntity>>("getShopContents", System.Net.Http.HttpMethod.Get, getShopContentsParam);

				shopEntity.Reverse();
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
                }

				shopContents = new ObservableCollection<ContentsEntity>(shopEntity);
				ShopContentsListView.ItemsSource = shopContents;

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

		async void OnContentsSelection(object sender, SelectedItemChangedEventArgs e)
		{
            Debug.WriteLine(((ListView)sender).SelectedItem);
			((ListView)sender).SelectedItem = null;
			if (e.SelectedItem == null)
			{
				return;
			}
			var item = (ContentsEntity)e.SelectedItem;
            int indexOfItem = shopContents.IndexOf(item);
            switch(shopContents[indexOfItem].LikeMember)
			{
				case "HeartFilled.png":
                    shopContents[indexOfItem].LikeMember = "HeartEmpty.png";
					shopContents[indexOfItem].Like--;
                    var blobNameOfFilled = shopContents[indexOfItem].RowKey.Split('/');
					await SyncData.UpdateLikeNum(shopOwner, blobNameOfFilled[blobNameOfFilled.Length-1], App.userEmail.Split('@')[0], "down");
					break;
				case "HeartEmpty.png":
					shopContents[indexOfItem].LikeMember = "HeartFilled.png";
                    shopContents[indexOfItem].Like++;
					var blobNameOfEmpty = shopContents[indexOfItem].RowKey.Split('/');
					await SyncData.UpdateLikeNum(shopOwner, blobNameOfEmpty[blobNameOfEmpty.Length - 1], App.userEmail.Split('@')[0], "up");
                    break;
            }
		}

    }
}
