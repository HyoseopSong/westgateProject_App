using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using westgateproject.Models;

namespace westgateproject.Droid
{
	[Activity(Label = "LoginActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
	[IntentFilter(
		new[] { Intent.ActionView },
		Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
		DataSchemes = new[] { "com.googleusercontent.apps.62511156556-0nn8sfrn74vcf7ivbgh659q78go0vpdn" },
		DataPath = "/oauth2redirect")]
    public class LoginActivity : Activity
    {
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Convert Android.Net.Url to Uri
			var uri = new Uri(Intent.Data.ToString());

			// Load redirectUrl page
			AuthenticationState.Authenticator.OnPageLoading(uri);

			Finish();
		}
    }
}
