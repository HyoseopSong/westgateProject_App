using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using Google.SignIn;
using Newtonsoft.Json.Linq;
using UIKit;
using westgateproject;
using westgateproject.Helper;
using westgateproject.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer (typeof(InitialPage), typeof(InitialPageRenderer))]
namespace westgateproject.iOS
{
    public class InitialPageRenderer : PageRenderer, ISignInDelegate, ISignInUIDelegate
    {
        UILabel loginStatus;
        Button guestButton;
        SignInButton signInButton;
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			var height = (float)App.ScreenHeight;
			var width = (float)App.ScreenWidth;

            loginStatus = new UILabel(new CGRect(0 * width, 0.75 * height, 1 * width, 0.07 * height))
            {
                Text = "로그인 중 입니다...",
                TextAlignment = UITextAlignment.Center,
                TextColor = UIColor.Blue
			};
			View.AddSubview(loginStatus);

			signInButton = new SignInButton()
			{
				Frame = new System.Drawing.RectangleF(0.3f * width, 0.665f * height, 0.4f * width, 0.07f * height)
			};

			View.AddSubview(signInButton);
            signInButton.Hidden = true;


			//var signOutButton = UIButton.FromType(UIButtonType.System);
			//signOutButton.SetTitle("SignOut!", UIControlState.Normal);
			//signOutButton.Frame = new System.Drawing.RectangleF(0.3f * width, 0.765f * height, 0.4f * width, 0.07f * height);
			//View.AddSubview(signOutButton);

			//var disconButton = UIButton.FromType(UIButtonType.System);
			//disconButton.SetTitle("Disconnect!", UIControlState.Normal);
			//disconButton.Frame = new System.Drawing.RectangleF(0.6f * width, 0.765f * height, 0.2f * width, 0.07f * height);
			//View.AddSubview(disconButton);
   //         SignIn.SharedInstance.SignedIn +=  (sender, ei) =>
			//{
			//	// Perform any operations on signed in user here.
			//	var token = new JObject
			//	{
			//		{ "id_token", ei.User.Authentication.IdToken }
			//	};
			//	loginStatus.Text = "Login Completed!!";


			//	//Debug.WriteLine("IdToken : " + ei.User.Authentication.IdToken);
			//	//Debug.WriteLine("AccessToken : " + ei.User.Authentication.AccessToken);
			//	//Debug.WriteLine(ei.User.UserID + " " + ei.User.Profile.FamilyName + " " + ei.User.Profile.Email + " " + ei.User.Authentication.IdToken);
			//	//App.Client.CurrentUser = await App.Client.LoginAsync(Microsoft.WindowsAzure.MobileServices.MobileServiceAuthenticationProvider.Google, token);
			//	//App.userEmail = ei.User.Profile.Email;
			//	//if (ei.User != null && ei.Error == null)
			//	//{

			//	//	Debug.WriteLine("You are signed in");
			//	//	//statusText.Text = string.Format ("Signed in user: {0}", e.User.Profile.Name);
			//	//	//ToggleAuthUI();

			//	//	IDictionary<string, string> result = new Dictionary<string, string>();
			//	//	try
			//	//	{
			//	//		result = await App.Client.InvokeApiAsync<IDictionary<string, string>>("notice", System.Net.Http.HttpMethod.Get, null);
			//	//		foreach (var temp in result)
			//	//		{
			//	//			Debug.WriteLine("Key : " + temp.Key + ", Value : " + temp.Value);
			//	//		}
			//	//		loginStatus.Text = "로그인 완료 되었습니다.";
			//	//	}
			//	//	catch (Exception ex)
			//	//	{
			//	//		Debug.WriteLine(ex.GetType());
			//	//		Debug.WriteLine("서버에서 정보를 불러올 수 없습니다.");
			//	//		return;
			//	//	}
			//	//}

			//};

            signInButton.TouchUpInside += (sender, eo) =>
            {
                loginStatus.Text = "Logging in...";

                //if (SignIn.SharedInstance.CurrentUser != null)
                //{
                //    var token = new JObject
                //    {
	               //     { "id_token", SignIn.SharedInstance.CurrentUser.Authentication.IdToken }
	               // };
                //    App.Client.CurrentUser = await App.Client.LoginAsync(Microsoft.WindowsAzure.MobileServices.MobileServiceAuthenticationProvider.Google, token);
                //    App.userEmail = SignIn.SharedInstance.CurrentUser.Profile.Email;

                //    loginStatus.Text = "로그인 완료 되었습니다.";
                //}
            };

			//signOutButton.TouchUpInside += (sender, eo) =>
			//{
			//	loginStatus.Text = "SignOutButton is pressed!!";
			//	SignIn.SharedInstance.SignOutUser();
   //             App.userEmail = null;
			//	//ToggleAuthUI();
			//};

			//disconButton.TouchUpInside += (sender, ed) =>
			//{
			//	loginStatus.Text = "Disconnect Button Pressed!!";
			//	SignIn.SharedInstance.DisconnectUser();
			//};


			// Assign the SignIn Delegates to receive callbacks
			SignIn.SharedInstance.UIDelegate = this;
			SignIn.SharedInstance.Delegate = this;


			SignIn.SharedInstance.SignInUserSilently();

			
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null || Element == null)
			{
				return;
			}

            var currentView = e.NewElement as InitialPage;
            guestButton = currentView.getGuest();
            guestButton.IsVisible = false;

        }


        public async void DidSignIn(SignIn signIn, GoogleUser user, NSError error)
		{
            if (SignIn.SharedInstance.CurrentUser == null)
            {
				var height = (float)App.ScreenHeight;
				var width = (float)App.ScreenWidth;

				//SignIn.SharedInstance.SignInUser();
				
                loginStatus.Text = "Please touch Sign in Button";

                signInButton.Hidden = false;


                return;
            }

            signInButton.Hidden = true;
			var token = SignIn.SharedInstance.CurrentUser.Authentication.IdToken;
			var token1 = new JObject
				{
					{ "id_token", token }
				};
			App.Client.CurrentUser = await App.Client.LoginAsync(Microsoft.WindowsAzure.MobileServices.MobileServiceAuthenticationProvider.Google, token1);
		    App.userEmail = SignIn.SharedInstance.CurrentUser.Profile.Email;

			loginStatus.Text = "Successfully signed in!";

            guestButton.IsVisible = true;
        }

    }
}
