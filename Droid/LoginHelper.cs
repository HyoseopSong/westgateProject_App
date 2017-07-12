using System;
using Android.Content;
using westgateproject.Droid;
using westgateproject.Helper;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(LoginHelper))]
namespace westgateproject.Droid
{
    public class LoginHelper : ILoginHelper
    {
        public LoginHelper()
        {
        }


        public void StartLogin()
        {
            var intent = new Intent(Forms.Context, typeof(SignInActivity));
            Forms.Context.StartActivity(intent);
        }
    }
}
