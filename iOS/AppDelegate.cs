using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Foundation;
using Google.SignIn;
using UIKit;

namespace westgateproject.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
        public static UIStoryboard Storyboard = UIStoryboard.FromName("MainStoryboard", null);
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// Initialize Azure Mobile Apps
			Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

			// Initialize Xamarin Forms
			global::Xamarin.Forms.Forms.Init ();
			global::Xamarin.FormsMaps.Init();

			App.ScreenWidth = UIScreen.MainScreen.Bounds.Width;
			App.ScreenHeight = UIScreen.MainScreen.Bounds.Height;

			var googleServiceDictionary = NSDictionary.FromFile("GoogleService-Info.plist");
			SignIn.SharedInstance.ClientID = googleServiceDictionary["CLIENT_ID"].ToString();
			SignIn.SharedInstance.ServerClientID = @"62511156556-eb28nq2m58mqe2n4vba2j09gjqdfliu7.apps.googleusercontent.com";



			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            var openUrlOptions = new UIApplicationOpenUrlOptions(options);
            return SignIn.SharedInstance.HandleUrl(url, openUrlOptions.SourceApplication, openUrlOptions.Annotation);
        }

	}
}

