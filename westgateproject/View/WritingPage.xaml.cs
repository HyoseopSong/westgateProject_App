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
        bool backTouched;
		int deleteCount;
		Stream stream;
		int moreButtonCount;
        Picker shopPicker;

		Dictionary<string, string> _shopLocation = new Dictionary<string, string>();
		List<UserInfoEntity> userInfo = new List<UserInfoEntity>();
		List<string> blobNameList = new List<string>();
        List<ContentsEntity> imageSource = new List<ContentsEntity>();
        //List<int> likeNumList = new List<int>();


        public WritingPage(){}

        public WritingPage(List<UserInfoEntity> userInfoParam)
        {
            InitializeComponent();
            moreButtonCount = 0;
            deleteCount = 0;
            backTouched = false;
            _shopLocation = new Dictionary<string, string>();
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

            userInfo = userInfoParam;
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

            shopPicker = new Picker
            {
                Title = "게시 할 매장",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

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

                    var shopInfo = UserInfo.RowKey.Split(':');
                    _shopLocation.Add(UserInfo.ShopName, shopInfo[0] + ":" + shopInfo[1] + ":" + shopInfo[2]);

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
                    switch (rawShopInfo[0])
                    {
                        case "Dongsan":
                            rawShopInfo[0] = "동산상가";
                            break;
                        case "FifthBuilding":
                            rawShopInfo[0] = "5지구";
                            break;
                        case "SecondBuilding":
                            rawShopInfo[0] = "2지구";
                            break;
                    }
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
            shopPicker.SelectedIndex = -1;
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



            Dictionary<string, string> getParam = new Dictionary<string, string>
            {
                { "id", App.userEmail},
            };
            imageSource = await App.Client.InvokeApiAsync<List<ContentsEntity>>("upload", System.Net.Http.HttpMethod.Get, getParam);
            

            if (imageSource.Count > 0)
            {
                imageSource.Reverse();

                foreach(var temp in imageSource)
                {
                    blobNameList.Add(temp.RowKey);
                }

                for (int i = 0; i < 20 && i < imageSource.Count; i++)
                {
					string imageURL = "https://westgateproject.blob.core.windows.net/" + App.userEmail.Split('@')[0] + "/" + imageSource[i].RowKey;

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
						Text = imageSource[i].Like.ToString(),
						VerticalTextAlignment = TextAlignment.Center
					};
					//likeNumList.Add(imageSource[i].Like);
					//var shopInfoLabel = new Label()
					//{
					//	Text = "HeartEmpty",
					//	IsVisible = false
					//};

					switch (imageSource[i].LikeMember)
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
					heartTapGestureRecognizer.Tapped += async (s, e) => {
						var thisLayout = s as StackLayout;
						var indexOfThisLayout = myActivity.Children.IndexOf(thisLayout) / 5;
						var heartEmpty = thisLayout.Children[0] as Image;
						var heartFilled = thisLayout.Children[1] as Image;
						var likeNum = thisLayout.Children[2] as Label;
						switch (imageSource[i].LikeMember)
						{
							case "True":
								heartEmpty.IsVisible = true;
								heartFilled.IsVisible = false;
								likeNum.Text = (--imageSource[indexOfThisLayout].Like).ToString();
								imageSource[i].LikeMember = "False";
								await SyncData.UpdateLikeNum(imageSource[indexOfThisLayout].PartitionKey, imageSource[indexOfThisLayout].RowKey, App.userEmail.Split('@')[0], "down");
								break;
							case "False":
								heartEmpty.IsVisible = false;
								heartFilled.IsVisible = true;
								likeNum.Text = (++imageSource[indexOfThisLayout].Like).ToString();
								imageSource[i].LikeMember = "True";
								await SyncData.UpdateLikeNum(imageSource[indexOfThisLayout].PartitionKey, imageSource[indexOfThisLayout].RowKey, App.userEmail.Split('@')[0], "up");
								break;

						}
					};
					layout.GestureRecognizers.Add(heartTapGestureRecognizer);

					myActivity.Children.Add(layout);



					var myLabel = new Label()
					{
						Text = imageSource[i].ShopName + " : " + imageSource[i].Context
					};
					myActivity.Children.Add(myLabel);


					var myButton = new Button()
					{
						Text = "삭제"
					};
					myButton.Clicked += DeleteButton_Clicked;
					myActivity.Children.Add(myButton);

					var myInsideBoxView = new BoxView()
					{
						HeightRequest = 10,
						BackgroundColor = Color.LightGray
					};
					myActivity.Children.Add(myInsideBoxView);
                }
				if (imageSource.Count > 20)
				{

					var labelButton = new Button()
					{
						Text = "이전 내 소식 더 보기"
					};
					labelButton.Clicked += DownMoreButton_Clicked;
					myActivity.Children.Add(labelButton);
				}

            }
            else
            {
                var myLabel = new Label()
                {
                    Text = "게시물이 없습니다."
                };
                myActivity.Children.Add(myLabel);
            }

        }
		private async void UpMoreButton_Clicked(object sender, EventArgs e)
		{
			myActivity.Children.Clear();


			if (--moreButtonCount > 0)
			{
				var UpLabelButton = new Button()
				{
					Text = "최근 내 소식 더 보기"
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
			for (int i = startIndex; (i < startIndex + 20) && (i < imageSource.Count); i++)
			{
				string imageURL = "https://westgateproject.blob.core.windows.net/" + App.userEmail.Split('@')[0] + "/" + imageSource[i].RowKey;

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
					Text = imageSource[i].Like.ToString(),
					VerticalTextAlignment = TextAlignment.Center
				};

				switch (imageSource[i].LikeMember)
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
					var indexOfThisLayout = myActivity.Children.IndexOf(thisLayout) / 5;
					var heartEmpty = thisLayout.Children[0] as Image;
					var heartFilled = thisLayout.Children[1] as Image;
					var likeNum = thisLayout.Children[2] as Label;
					switch (imageSource[i].LikeMember)
					{
						case "True":
							heartEmpty.IsVisible = true;
							heartFilled.IsVisible = false;
							likeNum.Text = (--imageSource[indexOfThisLayout].Like).ToString();
							imageSource[i].LikeMember = "False";
							await SyncData.UpdateLikeNum(imageSource[indexOfThisLayout].PartitionKey, imageSource[indexOfThisLayout].RowKey, App.userEmail.Split('@')[0], "down");
							break;
						case "False":
							heartEmpty.IsVisible = false;
							heartFilled.IsVisible = true;
							likeNum.Text = (++imageSource[indexOfThisLayout].Like).ToString();
							imageSource[i].LikeMember = "True";
							await SyncData.UpdateLikeNum(imageSource[indexOfThisLayout].PartitionKey, imageSource[indexOfThisLayout].RowKey, App.userEmail.Split('@')[0], "up");
							break;

					}
				};
				layout.GestureRecognizers.Add(heartTapGestureRecognizer);

				myActivity.Children.Add(layout);



				var myLabel = new Label()
				{
					Text = imageSource[i].ShopName + " : " + imageSource[i].Context
				};
				myActivity.Children.Add(myLabel);

				blobNameList.Add(imageSource[i].RowKey);

				var myButton = new Button()
				{
					Text = "삭제"
				};
				myButton.Clicked += DeleteButton_Clicked;
				myActivity.Children.Add(myButton);

				var myInsideBoxView = new BoxView()
				{
					HeightRequest = 10,
					BackgroundColor = Color.LightGray
				};
				myActivity.Children.Add(myInsideBoxView);
			}

			var labelButton = new Button()
			{
				Text = "이전 내 소식 더 보기"
			};
			labelButton.Clicked += DownMoreButton_Clicked;
			myActivity.Children.Add(labelButton);

            await ActivityScroll.ScrollToAsync(labelButton, ScrollToPosition.End, false);

		}

		private async void DownMoreButton_Clicked(object sender, EventArgs e)
		{
			myActivity.Children.Clear();

			var UpLabelButton = new Button()
			{
				Text = "최근 내 소식 더 보기"
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
			for (int i = startIndex; (i < startIndex + 20) && (i < imageSource.Count); i++)
			{
				string imageURL = "https://westgateproject.blob.core.windows.net/" + App.userEmail.Split('@')[0] + "/" + imageSource[i].RowKey;

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
					Text = imageSource[i].Like.ToString(),
					VerticalTextAlignment = TextAlignment.Center
				};

				switch (imageSource[i].LikeMember)
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
					var indexOfThisLayout = myActivity.Children.IndexOf(thisLayout) / 5;
					var heartEmpty = thisLayout.Children[0] as Image;
					var heartFilled = thisLayout.Children[1] as Image;
					var likeNum = thisLayout.Children[2] as Label;
					switch (imageSource[i].LikeMember)
					{
						case "True":
							heartEmpty.IsVisible = true;
							heartFilled.IsVisible = false;
							likeNum.Text = (--imageSource[indexOfThisLayout].Like).ToString();
							imageSource[i].LikeMember = "False";
							await SyncData.UpdateLikeNum(imageSource[indexOfThisLayout].PartitionKey, imageSource[indexOfThisLayout].RowKey, App.userEmail.Split('@')[0], "down");
							break;
						case "False":
							heartEmpty.IsVisible = false;
							heartFilled.IsVisible = true;
							likeNum.Text = (++imageSource[indexOfThisLayout].Like).ToString();
							imageSource[i].LikeMember = "True";
							await SyncData.UpdateLikeNum(imageSource[indexOfThisLayout].PartitionKey, imageSource[indexOfThisLayout].RowKey, App.userEmail.Split('@')[0], "up");
							break;

					}
				};
				layout.GestureRecognizers.Add(heartTapGestureRecognizer);

				myActivity.Children.Add(layout);



				var myLabel = new Label()
				{
					Text = imageSource[i].ShopName + " : " + imageSource[i].Context
				};
				myActivity.Children.Add(myLabel);

				blobNameList.Add(imageSource[i].RowKey);

				var myButton = new Button()
				{
					Text = "삭제"
				};
				myButton.Clicked += DeleteButton_Clicked;
				myActivity.Children.Add(myButton);

				var myInsideBoxView = new BoxView()
				{
					HeightRequest = 10,
					BackgroundColor = Color.LightGray
				};
				myActivity.Children.Add(myInsideBoxView);
			}

			if (imageSource.Count > (moreButtonCount + 1) * 20)
			{
				var labelButton = new Button()
				{
					Text = "예전 내 소식 더 보기"
				};
				labelButton.Clicked += DownMoreButton_Clicked;
				myActivity.Children.Add(labelButton);
			}

			await ActivityScroll.ScrollToAsync(0, 0, false);
		}


        async void UploadButton_Clicked(object sender, EventArgs e)
        {
            if(myActivity.Children.Count == 1)
            {
                myActivity.Children.Clear();
            }


            var senderButton = sender as Button;
            senderButton.IsEnabled = false;
            string result = "";

            if(PhotoImage.Source != null && UploadTextEditor.Text != null && shopPicker.SelectedIndex > -1)
            {

                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                        result = await SyncData.UploadContents(photoStream, UploadTextEditor.Text, _shopName, _shopLocation[_shopName]);
                        break;
                    case Device.iOS:
                        result = await SyncData.UploadByteArrayContents(stream, UploadTextEditor.Text, _shopName, _shopLocation[_shopName]);
                        break;
                }
				result += ".jpg";
				ContentsEntity tempEntity = new ContentsEntity(App.userEmail, result, _shopName, UploadTextEditor.Text);
                imageSource.Insert(0, tempEntity);


                if(moreButtonCount == 0)
                {
					var myImage = new Image { Aspect = Aspect.AspectFit, HeightRequest = App.ScreenWidth };
					myImage.Source = ImageSource.FromStream(photoStream.GetStream);
					myActivity.Children.Insert(0, myImage);

					var layout = new StackLayout()
					{
						Orientation = StackOrientation.Horizontal
					};
					var heartEmtpyIcon = new Image { Source = "HeartEmpty.png" };
					var heartFilledIcon = new Image { Source = "HeartFilled.png" };
					var likeNumber = new Label
					{
						Text = "0",
						VerticalTextAlignment = TextAlignment.Center
					};
					var shopInfoLabel = new Label()
					{
						Text = "HeartEmpty",
						IsVisible = false
					};

					heartFilledIcon.IsVisible = false;
					heartEmtpyIcon.IsVisible = true;
					shopInfoLabel.Text = "HeartEmpty";

					layout.Children.Add(heartEmtpyIcon);
					layout.Children.Add(heartFilledIcon);
					layout.Children.Add(likeNumber);
					layout.Children.Add(shopInfoLabel);
					var heartTapGestureRecognizer = new TapGestureRecognizer();
					heartTapGestureRecognizer.Tapped += async (s, es) => {
						var thisLayout = s as StackLayout;
						var heartEmpty = thisLayout.Children[0] as Image;
						var heartFilled = thisLayout.Children[1] as Image;
						var likeNum = thisLayout.Children[2] as Label;
						var imgSource = thisLayout.Children[3] as Label;
						switch (imgSource.Text)
						{
							case "HeartFilled":
								heartEmpty.IsVisible = true;
								heartFilled.IsVisible = false;
								likeNum.Text = "0";
								imgSource.Text = "HeartEmpty";
								await SyncData.UpdateLikeNum(App.userEmail, result, App.userEmail.Split('@')[0], "down");
								break;
							default:
								heartEmpty.IsVisible = false;
								heartFilled.IsVisible = true;
								likeNum.Text = "1";
								imgSource.Text = "HeartFilled";
								await SyncData.UpdateLikeNum(App.userEmail, result, App.userEmail.Split('@')[0], "up");
								break;

						}
					};
					layout.GestureRecognizers.Add(heartTapGestureRecognizer);

					myActivity.Children.Insert(1, layout);


					var mySubLabel = new Label()
					{
						Text = _shopName + " : " + UploadTextEditor.Text
					};
					myActivity.Children.Insert(2, mySubLabel);

					blobNameList.Insert(0, result);


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

                PhotoImage.IsVisible = false;
                PhotoImage.Source = null;
                UploadTextEditor.Text = null;
            }
            else
            {
                await DisplayAlert("빈 칸 있음", "매장과 사진, 글 중 하나 이상이 비어있습니다.", "확인");
            }

            senderButton.IsEnabled = true;

        }

        private async void CameraButton_Clicked(object sender, EventArgs e)
        {
            photoStream = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {

                PhotoSize = PhotoSize.Custom,
                CustomPhotoSize = 25,
                CompressionQuality = 50,
                RotateImage = true

            });


            if (photoStream != null)
			{
				PhotoImage.IsVisible = true;
                PhotoImage.HeightRequest = App.ScreenHeight * 0.7;

                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                        PhotoImage.Source = ImageSource.FromStream(photoStream.GetStream);
                        break;
                    case Device.iOS:
                        byte[] resizedImageByteArray = DependencyService.Get<IImageResizeHelper>().ResizeImageIOS(photoStream.Path);
                        stream = new MemoryStream(resizedImageByteArray);
                        var thisStream = new MemoryStream(resizedImageByteArray);
                        PhotoImage.Source = ImageSource.FromStream(() => thisStream);

                        break;
                }
            }


        }


        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            deleteCount++;
			int senderIndex = myActivity.Children.IndexOf(sender as Button);
            var targetIndex = 0;
            if(moreButtonCount > 0)
            {
                targetIndex = ((senderIndex - 2) / 5) + (moreButtonCount * 20);
            }
            else
            {
                targetIndex = senderIndex / 5;
            }
            var result = await SyncData.DeleteContents(blobNameList[targetIndex]);
			blobNameList.RemoveAt(targetIndex);
            int indexNumber = senderIndex - 3;
            for (int ii = 0; ii < 5; ii++)
            {
                myActivity.Children.RemoveAt(indexNumber);
            }
            if(deleteCount == myActivity.Children.Count / 5)
            {
                var myLabel = new Label()
                {
                    Text = "게시물이 없습니다."
                };
                myActivity.Children.Add(myLabel);
            }
        }


        private async void PicturePicker_Clicked(object sender, EventArgs e)
        {
            photoStream = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.Custom,
                CustomPhotoSize = 25,
                CompressionQuality = 50
            });

            PhotoImage.IsVisible = true;

            if (photoStream != null)
            {
                PhotoImage.HeightRequest = App.ScreenHeight * 0.7;

                switch(Device.RuntimePlatform)
                {
                    case Device.Android:
                        PhotoImage.Source = ImageSource.FromStream(photoStream.GetStream);
                        break;
                    case Device.iOS:
                        byte[] resizedImageByteArray = DependencyService.Get<IImageResizeHelper>().ResizeImageIOS(photoStream.Path);
                        stream = new MemoryStream(resizedImageByteArray);
                        var thisStream = new MemoryStream(resizedImageByteArray);
                        PhotoImage.Source = ImageSource.FromStream(() => thisStream);

                        break;
                }
            }
        }


        protected override bool OnBackButtonPressed()
        {
            if(!backTouched)
            {
                backTouched = true;
                Navigation.PopAsync();
            }
            return true;
        }

    }
}
