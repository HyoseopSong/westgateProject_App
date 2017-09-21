using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using westgateproject.Helper;
using westgateproject.Models;
using Xamarin.Forms;

namespace westgateproject.View
{
    public partial class ShopInfoPage : ContentPage
    {
        string _building;
        string _floor;
		string _location;
		string _shopOwnerID;
		string shopOwner;
		bool gotoRegister;
		bool backTouched;
		bool isInitial;
        int moreButtonCount;
        List<ContentsEntity> shopContents;
        //List<int> likeNumList = new List<int>();

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
			moreButtonCount = 0;
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
			Debug.WriteLine("shopLabel.Text = " + shopLabel.Text);

			moreButtonCount = 0;
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

			IDictionary<string, string> shopInfo = new Dictionary<string, string>();
            shopInfo = await App.Client.InvokeApiAsync<IDictionary<string, string>>("getShopInformation", System.Net.Http.HttpMethod.Get, getParam);
            

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

				Dictionary<string, string> getShopContentsParam = new Dictionary<string, string>
    			{
    				{ "shopOwner", _shopOwnerID},
    				{ "shopName", this.Title},
                    { "userID", App.userEmail.Split('@')[0] }
    			};
				
				shopContents = await App.Client.InvokeApiAsync<List<ContentsEntity>>("getShopContents", System.Net.Http.HttpMethod.Get, getShopContentsParam);

                if(shopContents.Count == 0)
                {
					var myLabel = new Label()
					{
						Text = "게시물이 없습니다."
					};
					myActivity.Children.Add(myLabel);
					return;
                }

                shopContents.Reverse();
				//foreach (var contentsOfShop in shopContents)

				for (int i = 0; i < 20 && i < shopContents.Count; i++)
                {
					var imageURL = "https://westgateproject.blob.core.windows.net/" + _shopOwnerID + "/" + shopContents[i].RowKey;
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
							myActivity.Children.Add(myImage_Android);
							break;
						case Device.iOS:
							var myImage_iOS = new Image { Aspect = Aspect.AspectFit, HeightRequest = App.ScreenWidth };
							myImage_iOS.Source = ImageSource.FromUri(new Uri(imageURL));
							myActivity.Children.Add(myImage_iOS);
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
						Text = shopContents[i].Like.ToString(),
                        VerticalTextAlignment = TextAlignment.Center
					};
                    //likeNumList.Add(shopContents[i].Like);
					var shopInfoLabel = new Label()
					{
						Text = "HeartEmpty",
						IsVisible = false
					};

                    switch (shopContents[i].LikeMember)
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
					heartTapGestureRecognizer.Tapped += async (s, e) => {
						var thisLayout = s as StackLayout;
						var indexOfThisLayout = myActivity.Children.IndexOf(thisLayout);
						int ShopContentsIndex = 0;
						if (moreButtonCount > 0)
						{
							ShopContentsIndex = ((indexOfThisLayout - 2) / 4) + (moreButtonCount * 20);
						}
						else
						{
							ShopContentsIndex = indexOfThisLayout / 4;
						}
						var heartEmpty = thisLayout.Children[0] as Image;
						var heartFilled = thisLayout.Children[1] as Image;
						var likeNum = thisLayout.Children[2] as Label;
						switch (shopContents[ShopContentsIndex].LikeMember)
						{
							case "True":
								heartEmpty.IsVisible = true;
								heartFilled.IsVisible = false;
								likeNum.Text = (--shopContents[ShopContentsIndex].Like).ToString();
								shopContents[ShopContentsIndex].LikeMember = "False";
								await SyncData.UpdateLikeNum(shopOwner, shopContents[ShopContentsIndex].RowKey, App.userEmail.Split('@')[0], "down");
								break;
							case "False":
								heartEmpty.IsVisible = false;
								heartFilled.IsVisible = true;
								likeNum.Text = (++shopContents[ShopContentsIndex].Like).ToString();
								shopContents[ShopContentsIndex].LikeMember = "True";
								await SyncData.UpdateLikeNum(shopOwner, shopContents[ShopContentsIndex].RowKey, App.userEmail.Split('@')[0], "up");
								break;

						}
					};
					layout.GestureRecognizers.Add(heartTapGestureRecognizer);

					myActivity.Children.Add(layout);





					var myLabel = new Label()
					{
						Text = shopContents[i].Context
					};
					myActivity.Children.Add(myLabel);

					var myBoxView = new BoxView()
					{
						HeightRequest = 10,
						BackgroundColor = Color.LightGray
					};
					myActivity.Children.Add(myBoxView);
                }

				if (shopContents.Count > 20)
				{

					var labelButton = new Button()
					{
						Text = "이 매장의 예전 소식 더 보기"
					};
					labelButton.Clicked += DownMoreButton_Clicked;
					myActivity.Children.Add(labelButton);
				}

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


		private async void UpMoreButton_Clicked(object sender, EventArgs e)
		{
			myActivity.Children.Clear();

            if(--moreButtonCount > 0)
			{
				var UpLabelButton = new Button()
				{
					Text = "이 매장의 최근 소식 더 보기"
				};
				UpLabelButton.Clicked += UpMoreButton_Clicked;
				myActivity.Children.Add(UpLabelButton);

				var myUpBoxView = new BoxView()
				{
					HeightRequest = 10,
					BackgroundColor = Color.LightGray
				};
				myActivity.Children.Add(myUpBoxView);
                
            }


			int startIndex = moreButtonCount * 20;
			for (int i = startIndex; (i < startIndex + 20) && (i < shopContents.Count); i++)
			{
				var imageURL = "https://westgateproject.blob.core.windows.net/" + _shopOwnerID + "/" + shopContents[i].RowKey;
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
								tapGestureRecognizer.Tapped += (s, ee) =>
								{
									var img = s as Image;
									img.Rotation += 90;
								};
								myImage_Android.GestureRecognizers.Add(tapGestureRecognizer);
								break;
						}
						myActivity.Children.Add(myImage_Android);
						break;
					case Device.iOS:
						var myImage_iOS = new Image { Aspect = Aspect.AspectFit, HeightRequest = App.ScreenWidth };
						myImage_iOS.Source = ImageSource.FromUri(new Uri(imageURL));
						myActivity.Children.Add(myImage_iOS);
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
					Text = shopContents[i].Like.ToString(),
					VerticalTextAlignment = TextAlignment.Center
				};

				switch (shopContents[i].LikeMember)
				{
					case "True":
						heartFilledIcon.IsVisible = true;
						heartEmtpyIcon.IsVisible = false;
						break;
					case "False":
						heartFilledIcon.IsVisible = false;
						heartEmtpyIcon.IsVisible = true;
						break;

				}
				layout.Children.Add(heartEmtpyIcon);
				layout.Children.Add(heartFilledIcon);
				layout.Children.Add(likeNumber);
				var heartTapGestureRecognizer = new TapGestureRecognizer();
				heartTapGestureRecognizer.Tapped += async (s, ee) => {
					var thisLayout = s as StackLayout;
					var indexOfThisLayout = myActivity.Children.IndexOf(thisLayout);
                    int ShopContentsIndex = 0;
                    if(moreButtonCount > 0)
                    {
                        ShopContentsIndex = ((indexOfThisLayout - 2) / 4) + (moreButtonCount * 20);
                    }
                    else
                    {
                        ShopContentsIndex = indexOfThisLayout / 4;
                    }
					var heartEmpty = thisLayout.Children[0] as Image;
					var heartFilled = thisLayout.Children[1] as Image;
					var likeNum = thisLayout.Children[2] as Label;
					switch (shopContents[ShopContentsIndex].LikeMember)
					{
						case "True":
							heartEmpty.IsVisible = true;
							heartFilled.IsVisible = false;
							likeNum.Text = (--shopContents[ShopContentsIndex].Like).ToString();
							shopContents[ShopContentsIndex].LikeMember = "False";
							await SyncData.UpdateLikeNum(shopOwner, shopContents[ShopContentsIndex].RowKey, App.userEmail.Split('@')[0], "down");
							break;
						case "False":
							heartEmpty.IsVisible = false;
							heartFilled.IsVisible = true;
							likeNum.Text = (++shopContents[ShopContentsIndex].Like).ToString();
							shopContents[ShopContentsIndex].LikeMember = "True";
							await SyncData.UpdateLikeNum(shopOwner, shopContents[ShopContentsIndex].RowKey, App.userEmail.Split('@')[0], "up");
							break;

					}
				};
				layout.GestureRecognizers.Add(heartTapGestureRecognizer);

				myActivity.Children.Add(layout);





				var myLabel = new Label()
				{
					Text = shopContents[i].Context
				};
				myActivity.Children.Add(myLabel);

				var myBoxView = new BoxView()
				{
					HeightRequest = 10,
					BackgroundColor = Color.LightGray
				};
				myActivity.Children.Add(myBoxView);
			}

			var labelButton = new Button()
			{
				Text = "이 매장의 예전 소식 더 보기"
			};
			labelButton.Clicked += DownMoreButton_Clicked;
			myActivity.Children.Add(labelButton);


			await ActivityScroll.ScrollToAsync(labelButton, ScrollToPosition.Start, false);
		}

		private async void DownMoreButton_Clicked(object sender, EventArgs e)
		{
            myActivity.Children.Clear();


			var UpLabelButton = new Button()
			{
				Text = "이 매장의 최근 소식 더 보기"
			};
			UpLabelButton.Clicked += UpMoreButton_Clicked;
			myActivity.Children.Add(UpLabelButton);

			var myUpBoxView = new BoxView()
			{
				HeightRequest = 10,
				BackgroundColor = Color.LightGray
			};
			myActivity.Children.Add(myUpBoxView);


			moreButtonCount++;
			int startIndex = moreButtonCount * 20;
			for (int i = startIndex; (i < startIndex + 20) && (i < shopContents.Count); i++)
			{
				var imageURL = "https://westgateproject.blob.core.windows.net/" + _shopOwnerID + "/" + shopContents[i].RowKey;
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
								tapGestureRecognizer.Tapped += (s, ee) =>
								{
									var img = s as Image;
									img.Rotation += 90;
								};
								myImage_Android.GestureRecognizers.Add(tapGestureRecognizer);
								break;
						}
						myActivity.Children.Add(myImage_Android);
						break;
					case Device.iOS:
						var myImage_iOS = new Image { Aspect = Aspect.AspectFit, HeightRequest = App.ScreenWidth };
						myImage_iOS.Source = ImageSource.FromUri(new Uri(imageURL));
						myActivity.Children.Add(myImage_iOS);
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
					Text = shopContents[i].Like.ToString(),
					VerticalTextAlignment = TextAlignment.Center
				};
				//likeNumList.Add(shopContents[i].Like);

				switch (shopContents[i].LikeMember)
				{
					case "True":
						heartFilledIcon.IsVisible = true;
						heartEmtpyIcon.IsVisible = false;
						break;
					case "False":
						heartFilledIcon.IsVisible = false;
						heartEmtpyIcon.IsVisible = true;
						break;

				}
				layout.Children.Add(heartEmtpyIcon);
				layout.Children.Add(heartFilledIcon);
				layout.Children.Add(likeNumber);
				var heartTapGestureRecognizer = new TapGestureRecognizer();
				heartTapGestureRecognizer.Tapped += async (s, ee) => {
					var thisLayout = s as StackLayout;
					var indexOfThisLayout = myActivity.Children.IndexOf(thisLayout);
					int ShopContentsIndex = 0;
					if (moreButtonCount > 0)
					{
						ShopContentsIndex = ((indexOfThisLayout - 2) / 4) + (moreButtonCount * 20);
					}
					else
					{
						ShopContentsIndex = indexOfThisLayout / 4;
					}
					var heartEmpty = thisLayout.Children[0] as Image;
					var heartFilled = thisLayout.Children[1] as Image;
					var likeNum = thisLayout.Children[2] as Label;
					switch (shopContents[ShopContentsIndex].LikeMember)
					{
						case "True":
							heartEmpty.IsVisible = true;
							heartFilled.IsVisible = false;
							likeNum.Text = (--shopContents[ShopContentsIndex].Like).ToString();
							shopContents[ShopContentsIndex].LikeMember = "False";
							await SyncData.UpdateLikeNum(shopOwner, shopContents[ShopContentsIndex].RowKey, App.userEmail.Split('@')[0], "down");
							break;
						case "False":
							heartEmpty.IsVisible = false;
							heartFilled.IsVisible = true;
							likeNum.Text = (++shopContents[ShopContentsIndex].Like).ToString();
							shopContents[ShopContentsIndex].LikeMember = "True";
							await SyncData.UpdateLikeNum(shopOwner, shopContents[ShopContentsIndex].RowKey, App.userEmail.Split('@')[0], "up");
							break;

					}
				};
				layout.GestureRecognizers.Add(heartTapGestureRecognizer);

				myActivity.Children.Add(layout);





				var myLabel = new Label()
				{
					Text = shopContents[i].Context
				};
				myActivity.Children.Add(myLabel);

				var myBoxView = new BoxView()
				{
					HeightRequest = 10,
					BackgroundColor = Color.LightGray
				};
				myActivity.Children.Add(myBoxView);
			}

			if (shopContents.Count > (moreButtonCount + 1) * 20)
			{
				var labelButton = new Button()
				{
					Text = "이 매장의 예전 소식 더 보기"
				};
				labelButton.Clicked += DownMoreButton_Clicked;
				myActivity.Children.Add(labelButton);
			}


			await ActivityScroll.ScrollToAsync(UpLabelButton, ScrollToPosition.Start, false);
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



    }
}
