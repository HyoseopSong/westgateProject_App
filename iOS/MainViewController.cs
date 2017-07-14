using Foundation;
using System;
using UIKit;

namespace westgateproject.iOS
{
    public partial class MainViewController : UIViewController
    {
        partial void UIButton10_TouchUpInside(UIButton sender)
        {
            labelComponent.Text = "Text was changed!";
        }

        public MainViewController (IntPtr handle) : base (handle)
        {
        }
    }
}