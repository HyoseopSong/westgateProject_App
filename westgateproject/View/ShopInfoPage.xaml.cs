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
        public ShopInfoPage(ShopInformation info)
        {
            InitializeComponent();
        }

		protected override async void OnAppearing()
		{
			Dictionary<string, string> imageSource = new Dictionary<string, string>();

			try
			{
				Dictionary<string, string> getParam = new Dictionary<string, string>
				{
					{ "id", App.userEmail},
				};
				imageSource = await App.Client.InvokeApiAsync<Dictionary<string, string>>("upload", System.Net.Http.HttpMethod.Get, getParam);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.GetType());
				return;
			}

			foreach (var temp in imageSource)
			{
				var imageURL = "https://westgateproject.blob.core.windows.net/" + App.userEmail.Split('@')[0] + "/" + temp.Key;
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
						var myImage_iOS = new Image { Aspect = Aspect.AspectFit };
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
			}
		}
    }
}
