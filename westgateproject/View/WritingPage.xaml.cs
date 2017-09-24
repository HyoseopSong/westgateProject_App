using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        bool onProcessing;
        bool dataLoading;
        int myPageNumber;
        const int numOfMyPage = 10;
		Stream stream;
        Picker shopPicker;

		Dictionary<string, string> _shopLocation = new Dictionary<string, string>();
		List<UserInfoEntity> userInfo = new List<UserInfoEntity>();
		List<string> blobNameList = new List<string>();
        List<ContentsEntity> MyContentsSource = new List<ContentsEntity>();
        ObservableCollection<ContentsEntity> myContents = new ObservableCollection<ContentsEntity>();


        public WritingPage(){}

        public WritingPage(List<UserInfoEntity> userInfoParam)
        {
            InitializeComponent();
            dataLoading = false;
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
                onProcessing = false;
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
            MyContentsSource = await App.Client.InvokeApiAsync<List<ContentsEntity>>("upload", System.Net.Http.HttpMethod.Get, getParam);

            MyContentsSource.Reverse();
			MyListView.ItemsSource = myContents;

            int i = 0;
            foreach (var t in MyContentsSource)
            {
                t.RowKey = "https://westgateproject.blob.core.windows.net/" + App.userEmail.Split('@')[0] + "/" + t.RowKey;
                switch (t.LikeMember)
                {
                    case "True":
                        t.LikeMember = "HeartFilled.png";
                        break;
                    case "False":
                        t.LikeMember = "HeartEmpty.png";
                        break;
                }

                if (i++ < numOfMyPage)
                {
                    myContents.Add(t);
                }
            }
            MyListView.ItemAppearing += (object sender, ItemVisibilityEventArgs e) =>
            {
                var item = e.Item as ContentsEntity;
                int index = myContents.IndexOf(item);
                if (myContents.Count - 2 <= index)
                {
                    if (!dataLoading)
                    {
                        dataLoading = true;

                        myPageNumber++;
                        for (i = myPageNumber * numOfMyPage; i < (myPageNumber + 1) * numOfMyPage && i < MyContentsSource.Count; i++)
                        {
                            myContents.Add(MyContentsSource[i]);
                        }

                        dataLoading = false;
                    }
                }
            };

        }

        async void UploadButton_Clicked(object sender, EventArgs e)
        {

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

				tempEntity.RowKey = "https://westgateproject.blob.core.windows.net/" + App.userEmail.Split('@')[0] + "/" + tempEntity.RowKey;
                tempEntity.LikeMember = "HeartEmpty.png";
                MyContentsSource.Insert(0, tempEntity);

                myContents.Insert(0, tempEntity);

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

		void OnSearchItemClicked(object sender, EventArgs args)
		{
			if (mySearchWindow.IsVisible)
			{
				mySearchWindow.IsVisible = false;
			}
			else
			{
				mySearchWindow.IsVisible = true;
			}
		}

        async void OnMyContentSelection(object sender, SelectedItemChangedEventArgs e)
        {
            Debug.WriteLine("onProcessing : " + onProcessing);
            if (!onProcessing)
            {
                onProcessing = true;
				if (e.SelectedItem == null)
				{
					onProcessing = false;
					return;
				}

				var item = (ContentsEntity)e.SelectedItem;

				var answer = await DisplayAlert("게시물 삭제", "이 게시물을 삭제하시겠습니까?", "삭제", "무시");
				if (answer)
				{
					myContents.Remove(item);

					var splitedRowKey = item.RowKey.Split('/');
					var result = await SyncData.DeleteContents(splitedRowKey[splitedRowKey.Length - 1]);
					onProcessing = false;
				}
                else
                {
                    onProcessing = false;
                }
            }

			((ListView)sender).SelectedItem = null;
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
		void MySearch(object sender, TextChangedEventArgs e)
		{

			myComplete.IsVisible = true;
			myCancel.IsVisible = false;
			if (e.NewTextValue == "")
			{
				MyListView.ItemsSource = myContents;
			}
			else
			{
				ObservableCollection<ContentsEntity> mySearchResult = new ObservableCollection<ContentsEntity>();
				MyListView.ItemsSource = mySearchResult;

				foreach (var r in MyContentsSource)
				{
					if (r.Context.Contains(e.NewTextValue) || r.ShopName.Contains(e.NewTextValue))
					{
						mySearchResult.Add(r);
					}

				}
			}
		}

		void MyCancelClicked(object sender, EventArgs e)
		{
			mySearchEntry.Text = "";
			myCancel.IsVisible = false;
			myComplete.IsVisible = true;
		}

		void MyCompleteClicked(object sender, EventArgs e)
		{
			if (mySearchEntry.Text != "")
			{
				myCancel.IsVisible = true;
				myComplete.IsVisible = false;
			}
		}
    }
}
