using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Plugin.Permissions;
using westgateproject.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace westgateproject.Droid
{
    [Activity (Label = "서문시장",	Icon = "@drawable/icon", MainLauncher = true,
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
	           Theme = "@android:style/Theme.Holo.Light",
                //ScreenOrientation = ScreenOrientation.User),
               ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{

        protected override void OnCreate (Bundle savedInstanceState)
		{

			FormsAppCompatActivity.ToolbarResource = Resource.Layout.Toolbar;
			FormsAppCompatActivity.TabLayoutResource = Resource.Layout.Tabbar;
			base.OnCreate (savedInstanceState);

			// Initialize Azure Mobile Apps
			// Initialize Xamarin Forms
			global::Xamarin.Forms.Forms.Init (this, savedInstanceState);
			global::Xamarin.FormsMaps.Init (this, savedInstanceState);

			var width = Resources.DisplayMetrics.WidthPixels;
			var height = Resources.DisplayMetrics.HeightPixels;
			var density = Resources.DisplayMetrics.Density;

			App.ScreenWidth = (width - 0.5f) / density;
			App.ScreenHeight = (height - 0.5f) / density;

			// Load the main application
			LoadApplication (new App ());


		}

		//public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		//{
		//	PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		//}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);

			System.Diagnostics.Debug.WriteLine("OnActivityResult of MainActivity");
			System.Diagnostics.Debug.WriteLine("requestCode : " + requestCode);
			System.Diagnostics.Debug.WriteLine("resultCode : " + resultCode);
            if (resultCode == Result.Ok)
            {
				MessagingCenter.Send<object>(this, "OK");
            }
            else if(resultCode == Result.Canceled)
            {
                MessagingCenter.Send<object>(this, "Canceled");
            }

		}
	}
}

