using System;
using System.Diagnostics;
using Foundation;
using Google.SignIn;
using UIKit;
using westgateproject.Helper;
using westgateproject.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(LoginHelper))]
namespace westgateproject.iOS
{
    public class LoginHelper:UIViewController, ILoginHelper
    {
        public LoginHelper() : base("LoginHelper", null)
		{
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        public void StartLogin()
        {

			Console.WriteLine("Hello");
            PerformSegue("SignInViewController", this);

        }

        public void StartLogout()
        {
            SignIn.SharedInstance.SignOutUser();
        }
    }
}