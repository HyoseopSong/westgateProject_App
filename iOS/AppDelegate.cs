using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Foundation;
using UIKit;

namespace westgateproject.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// Initialize Azure Mobile Apps
			Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

			// Initialize Xamarin Forms
			global::Xamarin.Forms.Forms.Init ();
			global::Xamarin.FormsMaps.Init();

			App.ScreenWidth = UIScreen.MainScreen.Bounds.Width;
			App.ScreenHeight = UIScreen.MainScreen.Bounds.Height;

			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}
	}
}

