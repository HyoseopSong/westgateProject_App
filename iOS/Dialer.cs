using System;
using Foundation;
using UIKit;
using westgateproject.Helper;
using westgateproject.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(Dialer))]
namespace westgateproject.iOS
{
    public class Dialer : IDialer
    {
		public bool Dial(string number)
		{
			return UIApplication.SharedApplication.OpenUrl(
				new NSUrl("tel:" + number));
		}
    }
}
