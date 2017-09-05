﻿using System;
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
        bool gotoRegister;
        public ShopInfoPage()
        {
            InitializeComponent();
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


        }

		protected override async void OnAppearing()
		{
            if(gotoRegister)
            {
                await Navigation.PopAsync();
                return;
            }
			IDictionary<string, string> imageSource = new Dictionary<string, string>();
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

            imageSource = await App.Client.InvokeApiAsync<IDictionary<string, string>>("getShopInformation", System.Net.Http.HttpMethod.Get, getParam);
            

            if (imageSource != null)
            {

				NavigationPage.SetHasBackButton(this, true);
                if(imageSource.Count == 3)
				{
					this.Title = imageSource["shopName"];
                    shopPhoneNumber.Text = imageSource["phoneNumber"];
					var myLabel = new Label()
					{
						Text = "게시물이 없습니다."
					};
					myActivity.Children.Add(myLabel);
                    return;
                }
                string shopOwner = "";
                foreach (var temp in imageSource)
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
							break;
						case "phoneNumber":
							Debug.WriteLine("temp.Value : " + temp.Value);
							shopPhoneNumber.Text = temp.Value;
							break;
                        case "notOnService":
                            if (App.userEmail == shopOwner)
                            {
                                await Navigation.PushAsync(new Register(_building, _floor, _location));
                                Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                                break;
                            }
                            else
                            {
                                await DisplayAlert("등록 진행 중", "이미 등록 신청 된 매장입니다.", "확인");
                                await Navigation.PopAsync(true);
                                return;
                            }
						default:
							var imageURL = "https://westgateproject.blob.core.windows.net/" + shopOwner.Split('@')[0] + "/" + temp.Key;
							switch (Device.RuntimePlatform)
							{
								case Device.Android:
									var myImage_Android = new Image { Aspect = Aspect.AspectFit, HeightRequest = App.ScreenWidth };
									var imageByte = await DependencyService.Get<IImageScaleHelper>().GetImageStream(imageURL);
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
									myActivity.Children.Insert(3, myImage_Android);
									break;
								case Device.iOS:
									var myImage_iOS = new Image { Aspect = Aspect.AspectFit, HeightRequest = App.ScreenWidth };
									myImage_iOS.Source = ImageSource.FromUri(new Uri(imageURL));
									myActivity.Children.Insert(3, myImage_iOS);
									break;
							}


							var myLabel = new Label()
							{
								Text = temp.Value
							};
							myActivity.Children.Insert(4, myLabel);

							var myBoxView = new BoxView()
							{
								HeightRequest = 10,
								BackgroundColor = Color.LightGray
							};
							myActivity.Children.Insert(5, myBoxView);
                            break;
    			    }

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
