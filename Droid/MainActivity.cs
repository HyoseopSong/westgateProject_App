using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using westgateproject.Helper;
using Xamarin.Forms.Platform.Android;

namespace westgateproject.Droid
{
    [Activity (Label = "서문시장",
		Icon = "@drawable/icon",
		MainLauncher = true,
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
	           Theme = "@android:style/Theme.Holo.Light", ScreenOrientation = ScreenOrientation.Portrait)]
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
	}
}

