using System;
using Xamarin.Forms;

namespace westgateproject.View.PageForEachFloor.office
{
    public partial class shopUnion : ContentPage
	{
		public bool onProcessing;
		bool backTouched;
		double currentScale = 1;
		double startScale = 1;
		double maxScaleValue;
		double minScaleValue;

		protected override bool OnBackButtonPressed()
		{
			if (!backTouched)
			{
				backTouched = true;
				Navigation.PopAsync();
			}
			return true;
		}
        public shopUnion()
        {
			InitializeComponent();
            backTouched = false;
			absL.AnchorX = 0;
			absL.AnchorY = 0;

			switch (Device.RuntimePlatform)
			{
				case Device.Android:
					maxScaleValue = (App.ScreenHeight - 110) / 189;
					break;
				default:
					maxScaleValue = (App.ScreenHeight - 70) / 189;
					break;
			}
			minScaleValue = App.ScreenWidth / 444;

			sliderBar.Maximum = maxScaleValue;
			sliderBar.Minimum = minScaleValue;

			var boundaryBox = new BoxView { Color = Color.Red };
			AbsoluteLayout.SetLayoutBounds(boundaryBox, new Rectangle(444 * maxScaleValue, 190 * maxScaleValue, 0, 1));
			absL.Children.Add(boundaryBox);
			onProcessing = false;

			switch (Device.RuntimePlatform)
			{
				case Device.iOS:
					sliderBar.IsVisible = false;
					var pinchGesture = new PinchGestureRecognizer();
					pinchGesture.PinchUpdated += OnPinchUpdated;
					absL.GestureRecognizers.Add(pinchGesture);
					break;
			}
		}

		async void OnTapped(object sender, EventArgs args)
		{
			if (!onProcessing)
			{
				onProcessing = true;
				await Navigation.PushAsync(new unionParking());
				onProcessing = false;
			}
		}
		//async void goBack(object sender, EventArgs args)
		//{
		//    await Navigation.PopAsync();
		//}
		void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
		{
			if (e.Status == GestureStatus.Started)
			{
				System.Diagnostics.Debug.WriteLine("OnPichUpdated Started");
				startScale = absL.Scale;
				absL.AnchorX = 0;
				absL.AnchorY = 0;
			}
			if (e.Status == GestureStatus.Running)
			{
				// Calculate the scale factor to be applied.
				currentScale += (e.Scale - 1) * startScale;
				currentScale = Math.Max(minScaleValue, currentScale);
				currentScale = Math.Min(currentScale, maxScaleValue);

				absL.Scale = currentScale;
			}
			if (e.Status == GestureStatus.Completed)
			{
				//int indexOfBoundaryBox = absL.Children.IndexOf(boundaryBox);
				//var temp = absL.Children[indexOfBoundaryBox].Bounds;
				//absL.Children[indexOfBoundaryBox].Layout(new Rectangle(width * currentScale, height * currentScale, 1, 1));
			}
		}

		void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
		{
			absL.AnchorX = 0;
			absL.AnchorY = 0;
			absL.Scale = args.NewValue;
		}
    }
}
