using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace westgateproject
{
	public class PinchToZoomContainer : ContentView
	{
		public double maxScale;
		public double minScale;
        public double anchorX;
        public double anchorY;
		double currentScale = 1;
		double startScale = 1;
		public double xOffset = 0;
		public double yOffset = 0;
		public double min_tx = 0;
        public double min_ty = 0;
		bool pinchCompleted;

		public PinchToZoomContainer()
		{
			var pinchGesture = new PinchGestureRecognizer();
			pinchGesture.PinchUpdated += OnPinchUpdated;
			var panGesture = new PanGestureRecognizer();
			panGesture.PanUpdated += OnPanUpdated;
            maxScale = 6;
            minScale = 1;
            anchorX = 0;
			anchorY = 0;
			GestureRecognizers.Add(pinchGesture);
			GestureRecognizers.Add(panGesture);
		}

		void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
		{
			if (e.Status == GestureStatus.Started)
			{
				startScale = Content.Scale;
                currentScale = startScale;
                Content.AnchorX = anchorX;
				Content.AnchorY = anchorY;
			}

			if (e.Status == GestureStatus.Running)
			{
				currentScale += (e.Scale - 1) * startScale;
                //switch(Device.RuntimePlatform)
                //{
                //    case "iOS":
                //        maxScale = 6;
                //        break;
                //    case "Android":
                //        maxScale = 6;
                //        break;
                //    default:
                //        break;
                        
                //}
				currentScale = Math.Min(Math.Max(minScale, currentScale), maxScale);
				//Content.TranslationX = xOffset - (Content.AnchorX - e.ScaleOrigin.X) * App.ScreenWidth;
				//Content.TranslationY = yOffset - (Content.AnchorY - e.ScaleOrigin.Y) * App.ScreenHeight;
				//Content.AnchorX = e.ScaleOrigin.X;
				//Content.AnchorY = e.ScaleOrigin.Y;

				//double renderedX = Content.X + xOffset;
				//double deltaX = renderedX / Width;
				//double deltaWidth = Width / (Content.Width * startScale);
				//double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

				//double renderedY = Content.Y + yOffset;
				//double deltaY = renderedY / Height;
				//double deltaHeight = Height / (Content.Height * startScale);
				//double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

				//double targetX = xOffset - (originX * Content.Width) * (currentScale - startScale);
				//double targetY = yOffset - (originY * Content.Height) * (currentScale - startScale);

				//Content.TranslationX = Math.Min(Math.Max(0, targetX), -Content.Width * (currentScale - 1));
				//Content.TranslationY = Math.Min(Math.Max(0, targetY), -Content.Height * (currentScale - 1));

				Content.Scale = currentScale;
				//xOffset = Content.TranslationX;
				//yOffset = Content.TranslationY;
			}
			if (e.Status == GestureStatus.Completed)
			{
				pinchCompleted = true;
                Debug.WriteLine("Content.Scale : "+Content.Scale);
			}
		}

		void OnPanUpdated(object sender, PanUpdatedEventArgs e)
		{
			if (e.StatusType == GestureStatus.Started)
			{
			}
			if (e.StatusType == GestureStatus.Running)
			{
				var tx = xOffset + e.TotalX;
				var ty = yOffset + e.TotalY;
				switch (Device.RuntimePlatform)
				{
					case Device.Android:
						if (!pinchCompleted)
						{
							Content.TranslationX = Math.Min(Math.Max(min_tx, tx), 0);
							Content.TranslationY = Math.Min(Math.Max(min_ty, ty), 0);
						}
						break;
					default:
						Content.TranslationX = Math.Min(Math.Max(min_tx, tx), 0);
						Content.TranslationY = Math.Min(Math.Max(min_ty, ty), 0);
						//Content.TranslationX = xOffset + e.TotalX;
						//Content.TranslationY = yOffset + e.TotalY;
						break;
				}
			}
			if (e.StatusType == GestureStatus.Completed)
			{
				xOffset = Content.TranslationX;
				yOffset = Content.TranslationY;
				pinchCompleted = false;
                Debug.WriteLine("TX : " + Content.TranslationX + ", TY : " + Content.TranslationY);
			}
		}
	}
}
