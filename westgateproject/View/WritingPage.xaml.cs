using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;
using westgateproject.Helper;
using westgateproject.Models;
using Xamarin.Forms;

namespace westgateproject.View
{
    public partial class WritingPage : TabbedPage
	{
        private MediaFile photoStream;
        string _shopName;
        bool isInitial;
		int deleteCount;

        public WritingPage()
		{
			InitializeComponent();
            deleteCount = 0;

            myIdLabel.Text = "내 계정 : " + App.userEmail;
            myIdLabel.VerticalTextAlignment = TextAlignment.Center;

            CameraButton.WidthRequest = App.ScreenWidth / 2;
            PictureButton.WidthRequest = App.ScreenWidth / 2;
            UploadTextEditor.BackgroundColor = Color.Lime;
            switch (Device.RuntimePlatform)
            {
				case Device.iOS:
                    //shopName.HeightRequest = 30;
                    //shopLocation.HeightRequest = 30;
                    //phoneNumber.HeightRequest = 30;
                    myIdLabel.HeightRequest = 30;
                    break;
				case Device.Android:
					//shopName.HeightRequest = 40;
					//shopLocation.HeightRequest = 40;
					//phoneNumber.HeightRequest = 40;
					myIdLabel.HeightRequest = 40;
                    break;
            }

            isInitial = true;

            //syncLabel();
		}
        protected override async void OnAppearing()
		{
            if(!isInitial)
            {
                Debug.WriteLine("OnAppearing if");
                return;
            }
            else
			{
				Debug.WriteLine("OnAppearing else");
                isInitial = false;
            }
			List<UserInfoEntity> userInfo = new List<UserInfoEntity>();
			Dictionary<string, string> getParamUserInfo = new Dictionary<string, string>
			{
				{ "id", App.userEmail},
			};

            userInfo = await App.Client.InvokeApiAsync<List<UserInfoEntity>>("userInformation", System.Net.Http.HttpMethod.Get, getParamUserInfo);

			Picker shopPicker = new Picker
			{
				Title = "게시 할 매장",
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

            if (userInfo.Count > 0)
			{

                int paidCount = 0;
                int unpaidCount = 0;

				BoxView myBoxView = new BoxView()
				{
					HeightRequest = 10,
					BackgroundColor = Color.LightGray
				};
				MyInformation.Children.Add(myBoxView);
                foreach (var UserInfo in userInfo)
                {
                    if (UserInfo.Paid)
					{
                        paidCount++;
                        _shopName = UserInfo.ShopName;
						shopPicker.Items.Add(UserInfo.ShopName);

                        var temp = UserInfo.RowKey;
                        var shopInfo = temp.Split(':');

						switch (shopInfo[0])
						{
							case "Dongsan":
                                shopInfo[0] = "동산상가";
								break;
							case "FifthBuilding":
								shopInfo[0] = "5지구";
								break;
							case "SecondBuilding":
								shopInfo[0] = "2지구";
								break;
						}

                        Label shopName = new Label()
                        {
                            Text = "매장 이름 : " + UserInfo.ShopName,
                            VerticalTextAlignment = TextAlignment.Center
                        };
                        Label shopLocation = new Label()
                        {
                            Text = "위치 : " + shopInfo[0] + " " + shopInfo[1] + " " + shopInfo[2],
                            VerticalTextAlignment = TextAlignment.Center
                        };
                        Label phoneNumber = new Label()
                        {
                            Text = "전화 번호 : " + UserInfo.PhoneNumber,
                            VerticalTextAlignment = TextAlignment.Center
						};
						Label servicePeriod = new Label()
						{
							Text = "만료 날짜 : " + UserInfo.Period,
							VerticalTextAlignment = TextAlignment.Center
						};
						BoxView myBox = new BoxView()
						{
							HeightRequest = 10,
							BackgroundColor = Color.LightGray
						};

                        switch (Device.RuntimePlatform)
                        {
                            case Device.iOS:
                                shopName.HeightRequest = 30;
                                shopLocation.HeightRequest = 30;
								phoneNumber.HeightRequest = 30;
								servicePeriod.HeightRequest = 30;
                                break;
                            case Device.Android:
                                shopName.HeightRequest = 40;
                                shopLocation.HeightRequest = 40;
								phoneNumber.HeightRequest = 40;
								servicePeriod.HeightRequest = 30;
                                break;
                        }

                        MyInformation.Children.Add(shopName);
                        MyInformation.Children.Add(shopLocation);
						MyInformation.Children.Add(phoneNumber);
						MyInformation.Children.Add(servicePeriod);
                        MyInformation.Children.Add(myBox);
                    }
                    else
					{
                        unpaidCount++;
						var rawShopInfo = UserInfo.RowKey.Split(':');
						Label shopInfo = new Label()
						{
							Text = rawShopInfo[0] + " " + rawShopInfo[1] + " " + rawShopInfo[2] + " " + UserInfo.ShopName + " 등록 대기 중",
							VerticalTextAlignment = TextAlignment.Center
						};

						BoxView myBox = new BoxView()
						{
							HeightRequest = 10,
							BackgroundColor = Color.LightGray
						};

						MyInformation.Children.Insert(0, shopInfo);
                    }
                }
                if (paidCount > 1)
                {
					shopPicker.SelectedIndexChanged += (sender, args) =>
					{
						_shopName = shopPicker.Items[shopPicker.SelectedIndex];
				    };
                    MyInformation.Children.Add(shopPicker);
                }

                if (unpaidCount < 1)
                {
                    referenceBoxView.IsVisible = false;
                }
            }
            else
            {
				var myLabel = new Label()
				{
					Text = "내 매장 정보가 없습니다."
				};
				MyInformation.Children.Add(myLabel);
            }

			List<MyEntity> imageSource = new List<MyEntity>();
			try
			{
				Dictionary<string, string> getParam = new Dictionary<string, string>
				{
					{ "id", App.userEmail},
				};
				imageSource = await App.Client.InvokeApiAsync<List<MyEntity>>("upload", System.Net.Http.HttpMethod.Get, getParam);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.GetType() + "내 활동 내역이 없습니다.");

                statusLabel.IsVisible = true;

			}

            if (imageSource.Count > 0)
            {
                foreach (var temp in imageSource)
                {
                    string imageURL = "https://westgateproject.blob.core.windows.net/" + App.userEmail.Split('@')[0] + "/" + temp.PartitionKey;

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
                            myActivity.Children.Insert(0, myImage_Android);
                            break;
                        case Device.iOS:
                            var myImage_iOS = new Image { Aspect = Aspect.AspectFit, HeightRequest = App.ScreenWidth };
                            myImage_iOS.Source = ImageSource.FromUri(new Uri(imageURL));
                            myActivity.Children.Insert(0, myImage_iOS);
                            break;
                    }


                    var myLabel = new Label()
                    {
                        Text = temp.RowKey + " : " + temp.Text
                    };
                    myActivity.Children.Insert(1, myLabel);

                    var imageName = new Label()
                    {
                        Text = temp.PartitionKey,
                        IsVisible = false
                    };
                    myActivity.Children.Insert(2, imageName);

                    var myButton = new Button()
                    {
                        Text = "삭제"
                    };
                    myButton.Clicked += DeleteButton_Clicked;
                    myActivity.Children.Insert(3, myButton);

                    var myBoxView = new BoxView()
                    {
                        HeightRequest = 10,
                        BackgroundColor = Color.LightGray
                    };
                    myActivity.Children.Insert(4, myBoxView);
                }
            }
            else
            {
				Debug.WriteLine("내 활동 내역이 없습니다.");

                statusLabel.IsVisible = true;
            }

        }


		//async void OnItemClicked(object sender, EventArgs e)
		//{
		//	var shopSync = await SyncData.SyncShopInfo();
		//	var buildingSync = await SyncData.SyncBuildingInfo();
		//	refresh.IsVisible = true;
		//	if (!shopSync || !buildingSync)
		//		refresh.Text = "서버에서 데이터를 가져올 수 없습니다. 마지막에 저장된 데이터를 사용합니다." + Environment.NewLine + System.DateTime.Now.ToString("G");
		//	else
		//		refresh.Text = "서버에서 지도 정보를 가져왔습니다." + Environment.NewLine + System.DateTime.Now.ToString("G");
		//}

		async void UploadButton_Clicked(object sender, EventArgs e)
		{
            if(_shopName == null)
            {
                await DisplayAlert("등록된 매장 없음", "내 매장을 등록하시면 게시 하실 수 있습니다.", "확인");
                return;
			}
			var senderButton = sender as Button;
			senderButton.IsEnabled = false;

            var result = await SyncData.UploadContents(photoStream, UploadTextEditor.Text, _shopName);
            switch(result)
            {
				case null:
					await DisplayAlert("빈 칸 있음", "사진과 글 모두 채워주세요.", "확인");
                    break;
				default:
                    statusLabel.IsVisible = false;
					result += ".jpg";
					var myImage = new Image { Aspect = Aspect.AspectFit, HeightRequest = App.ScreenWidth };
					myImage.Source = ImageSource.FromStream(photoStream.GetStream);
					myActivity.Children.Insert(0, myImage);

					var mySubLabel = new Label()
					{
						Text = _shopName + UploadTextEditor.Text
					};
					myActivity.Children.Insert(1, mySubLabel);


					var imageName = new Label()
					{
						Text = result,
						IsVisible = false
					};
					myActivity.Children.Insert(2, imageName);


					var myButton = new Button()
					{
						Text = "삭제"
					};
					myButton.Clicked += DeleteButton_Clicked;
					myActivity.Children.Insert(3, myButton);

					var myBoxView = new BoxView()
					{
						HeightRequest = 10,
						BackgroundColor = Color.LightGray
					};
					myActivity.Children.Insert(4, myBoxView);

					PhotoImage.IsVisible = false;
					UploadTextEditor.Text = "";
                    break;

			}
			senderButton.IsEnabled = true;

		}

  //      private void uploadEditorFocused(object sender, EventArgs e)
  //      {
  //          var editor = (Editor)sender;
  //          Debug.WriteLine("editor.Height : " + editor.Height);

  //      }

  //      private void uploadEditorUnfocused(object sender, EventArgs e)
		//{

		//	var editor = (Editor)sender;
		//	Debug.WriteLine("Unfocuseditor.Height : " + editor.Height);
		//}

		private async void CameraButton_Clicked(object sender, EventArgs e)
		{
            photoStream = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
			{
                PhotoSize = PhotoSize.Small,
                RotateImage = true

			});

            PhotoImage.IsVisible = true;

            if (photoStream != null)
            {
                PhotoImage.Source = ImageSource.FromStream(photoStream.GetStream);
                PhotoImage.HeightRequest = App.ScreenHeight * 0.7;
            }
		}

		//async protected void syncLabel()
		//{
		//	IDictionary<string, string> result = new Dictionary<string, string>();
		//	try
		//	{
		//		result = await App.Client.InvokeApiAsync<IDictionary<string, string>>("notice", System.Net.Http.HttpMethod.Get, null);
		//	}
		//	catch (Exception ex)
		//	{
		//		Debug.WriteLine(ex.GetType());
		//		appState.Text = "서버에서 정보를 불러올 수 없습니다.";
		//		return;
		//	}
		//	string[] state = result["개발현황"].Split(':');
		//	appState.Text = "";
		//	for (int i = 0; i < state.Length; i++)
		//	{
		//		appState.Text += state[i] + Environment.NewLine;
		//	}

		//}

		void EditorTextChanged(object sender, TextChangedEventArgs e)
		{
			//var oldText = e.OldTextValue;
			//var newText = e.NewTextValue;

			//Debug.WriteLine("newText : " + newText);
			//Debug.WriteLine("oldText : " + oldText);


   //         if(newText.Length > oldText.Length)
   //         {
			//	if (newText.EndsWith(System.Environment.NewLine))
			//		UploadTextEditor.HeightRequest += 10;
                
   //         }
   //         else
			//{
				//if (oldText.EndsWith(System.Environment.NewLine))
					//UploadTextEditor.HeightRequest -= 10;
                
            //}
		}

        private async void DeleteButton_Clicked(object sender, EventArgs e)
		{
			var senderButton = sender as Button;
			senderButton.IsEnabled = false;
            deleteCount++;
            int senderIndex = myActivity.Children.IndexOf(sender as Button);
			var imageName = myActivity.Children[senderIndex - 1] as Label;
			var result = await SyncData.DeleteContents(imageName.Text);

			int indexNumber = senderIndex - 3;
			for (int ii = indexNumber; ii < indexNumber + 5; ii++)
			{
				myActivity.Children[ii].IsVisible = false;
			}
            if(deleteCount == myActivity.Children.Count / 5)
            {
                statusLabel.IsVisible = true;
			}
			senderButton.IsEnabled = true;
        }


        private async void PicturePicker_Clicked(object sender, EventArgs e)
        {
            photoStream = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
            {
                PhotoSize = PhotoSize.Small
            });

			PhotoImage.IsVisible = true;

			if (photoStream != null)
			{
				PhotoImage.Source = ImageSource.FromStream(photoStream.GetStream);
				PhotoImage.HeightRequest = App.ScreenHeight * 0.7;
			}
        }

    }
}
