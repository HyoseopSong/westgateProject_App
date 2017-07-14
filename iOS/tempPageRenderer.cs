using System;
using westgateproject.iOS;
using westgateproject.View;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer(typeof(tempPage), typeof(tempPageRenderer))]
namespace westgateproject.iOS
{
    public class tempPageRenderer: PageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            var hostViewController = ViewController;

            var viewController = (MainViewController)AppDelegate.Storyboard.InstantiateViewController("MainViewController");

            hostViewController.AddChildViewController(viewController);
            hostViewController.View.Add(viewController.View);

            viewController.DidMoveToParentViewController(hostViewController);
        }
    }
}
