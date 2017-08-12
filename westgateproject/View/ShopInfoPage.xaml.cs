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

        }

		protected override async void OnAppearing()
		{
			IDictionary<string, string> imageSource = new Dictionary<string, string>();

			Dictionary<string, string> getParam = new Dictionary<string, string>
			{
				{ "building", _building},
				{ "floor", _floor},
				{ "location", _location},
			};
			imageSource = await App.Client.InvokeApiAsync<IDictionary<string, string>>("getShopInformation", System.Net.Http.HttpMethod.Get, getParam);

            if (imageSource != null)
            {
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
						default:
							var imageURL = "https://westgateproject.blob.core.windows.net/" + shopOwner + "/" + temp.Key;
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
											break;
										case "3":
											break;
										case "4":
											break;
										case "5":
											break;
										case "6":
											myImage_Android.Rotation = 90;
											break;
										case "7":
											break;
										case "8":
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
								Text = temp.Value
							};
							myActivity.Children.Insert(1, myLabel);

							var myBoxView = new BoxView()
							{
								HeightRequest = 10,
								BackgroundColor = Color.LightGray
							};
							myActivity.Children.Insert(2, myBoxView);
                            break;
                    }

                }
            }
            else
            {
                await DisplayAlert("No Shop Info", "This position is empty", "OK");
                await Navigation.PopAsync(true);
            }
		}
    }
}
