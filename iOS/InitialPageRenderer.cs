using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null || Element == null)
			{
				return;
			}

			var signInButton = new SignInButton()
			{
			    Frame = new CGRect(20, 100, 150, 44)
			};
			View.AddSubview(signInButton);




			//var signInButton = UIButton.FromType(UIButtonType.System);
			//signInButton.SetTitle("SignIn!", UIControlState.Normal);
			//signInButton.Frame = new System.Drawing.RectangleF(20, 100, 150, 44);
			//View.AddSubview(signInButton);

			var signOutButton = UIButton.FromType(UIButtonType.System);
			signOutButton.SetTitle("SignOut!", UIControlState.Normal);
			signOutButton.Frame = new System.Drawing.RectangleF(20, 150, 150, 44);
			View.AddSubview(signOutButton);

			var disconButton = UIButton.FromType(UIButtonType.System);
			disconButton.SetTitle("Disconnect!", UIControlState.Normal);
			disconButton.Frame = new System.Drawing.RectangleF(20, 200, 150, 44);
			View.AddSubview(disconButton);

			// Assign the SignIn Delegates to receive callbacks
			SignIn.SharedInstance.UIDelegate = this;
			SignIn.SharedInstance.Delegate = this;

            // Sign the user in automatically
            if(SignIn.SharedInstance.CurrentUser == null)
			{
				SignIn.SharedInstance.SignInUserSilently();
				ToggleAuthUI();
				Debug.WriteLine("after initial Toggle");
			}


			SignIn.SharedInstance.SignedIn += async (sender, ei) =>
			{
				// Perform any operations on signed in user here.
				var token = new JObject();
				token.Add("id_token", ei.User.Authentication.IdToken);
				Debug.WriteLine("IdToken : " + ei.User.Authentication.IdToken);
				Debug.WriteLine("AccessToken : " + ei.User.Authentication.AccessToken);
				Debug.WriteLine(ei.User.UserID + " " + ei.User.Profile.FamilyName + " " + ei.User.Profile.Email + " " + ei.User.Authentication.IdToken);
				App.Client.CurrentUser = await App.Client.LoginAsync(Microsoft.WindowsAzure.MobileServices.MobileServiceAuthenticationProvider.Google, token);

				if (ei.User != null && ei.Error == null)
				{

					Debug.WriteLine("You are signed in");
					//statusText.Text = string.Format ("Signed in user: {0}", e.User.Profile.Name);
					ToggleAuthUI();

					IDictionary<string, string> result = new Dictionary<string, string>();
					try
					{
						result = await App.Client.InvokeApiAsync<IDictionary<string, string>>("notice", System.Net.Http.HttpMethod.Get, null);
						Debug.WriteLine(result);
					}
					catch (Exception ex)
					{
						Debug.WriteLine(ex.GetType());
						Debug.WriteLine("서버에서 정보를 불러올 수 없습니다.");
						return;
					}
				}

			};


			//signInButton.TouchUpInside += (sender, eo) =>
			//{
			//	SignIn.SharedInstance.SignInUser();
			//	ToggleAuthUI();
			//};

			signOutButton.TouchUpInside += (sender, eo) =>
			{
				SignIn.SharedInstance.SignOutUser();
				ToggleAuthUI();
			};

			disconButton.TouchUpInside += (sender, ed) =>
			{
				SignIn.SharedInstance.DisconnectUser();
			};

			SignIn.SharedInstance.Disconnected += (sender, ed) =>
			{
				// Perform any operations when the user disconnects from app here.
				ToggleAuthUI();
			};

			void ToggleAuthUI()
			{
				if (SignIn.SharedInstance.CurrentUser == null || SignIn.SharedInstance.CurrentUser.Authentication == null)
				{
					Debug.WriteLine("Toggle Signed out");
					// Not signed in
					signInButton.Hidden = false;
					signOutButton.Hidden = true;
					disconButton.Hidden = true;
				}
				else
				{
					Debug.WriteLine("Toggle Signed in");
					// Signed in
					signInButton.Hidden = true;
					signOutButton.Hidden = false;
					disconButton.Hidden = false;
				}
			}
			//base.OnElementChanged(e);

			//if (e.OldElement != null || Element == null)
			//{
			//	return;
			//}


			//var signInButton = new SignInButton()
			//{
			//    Frame = new CGRect(100, 300, 150, 44)
			//};
			//View.AddSubview(signInButton);

			////var signInButton = UIButton.FromType(UIButtonType.System);
			////signInButton.SetTitle("SignIn!", UIControlState.Normal);
			////signInButton.Frame = new System.Drawing.RectangleF(20, 100, 150, 44);
			////View.AddSubview(signInButton);



			//// Assign the SignIn Delegates to receive callbacks
			//SignIn.SharedInstance.UIDelegate = this;
			//SignIn.SharedInstance.Delegate = this;

			////signInButton.TouchUpInside += (sender, eo) =>
			////{
			////	SignIn.SharedInstance.SignInUser();
			////	Debug.WriteLine("after signInButton TouchUpInside!!");
			////};

			//SignIn.SharedInstance.SignedIn += async (sender, ei) =>
			//{
			//	Debug.WriteLine("SignedIn!!");
   //             // Perform any operations on signed in user here.
   //             var token = new JObject
   //             {
   //                 { "id_token", ei.User.Authentication.IdToken }
   //             };
   //             App.Client.CurrentUser = await App.Client.LoginAsync(Microsoft.WindowsAzure.MobileServices.MobileServiceAuthenticationProvider.Google, token);

			//	if (ei.User != null && ei.Error == null)
			//	{

			//		IDictionary<string, string> result = new Dictionary<string, string>();
			//		try
			//		{
			//			result = await App.Client.InvokeApiAsync<IDictionary<string, string>>("notice", System.Net.Http.HttpMethod.Get, null);
			//			Debug.WriteLine(result);
			//		}
			//		catch (Exception ex)
			//		{
			//			return;
			//		}
			//	}

			//};
        }


        public void DidSignIn(SignIn signIn, GoogleUser user, NSError error)
        {
            
        }

    }
}
