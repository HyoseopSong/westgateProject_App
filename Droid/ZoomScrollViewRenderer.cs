using System;
using Android.Views;
using Android.Views.Animations;
using westgateproject.Droid;
using westgateproject.View;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.Views.ScaleGestureDetector;

[assembly: ExportRenderer(typeof(ScrollView), typeof(ZoomScrollViewRenderer))]
namespace westgateproject.Droid
{
    public class ZoomScrollViewRenderer : ScrollViewRenderer, IOnScaleGestureListener
    {
		//private float mScale = 1f;
		private ScaleGestureDetector mScaleDetector;

		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{

			base.OnElementChanged(e);
			mScaleDetector = new ScaleGestureDetector(Context, this);

		}


		public override bool DispatchTouchEvent(MotionEvent e)
		{
			base.DispatchTouchEvent(e);
			return mScaleDetector.OnTouchEvent(e);
		}

		public bool OnScale(ScaleGestureDetector detector)
		{
			FloorMap.scale = detector.ScaleFactor;
            MessagingCenter.Send<object>(this, "PinchRunning");

			//float scale = 1 - detector.ScaleFactor;

			//float prevScale = mScale;
			//mScale += scale;

			//if (mScale < 0.5f) // Minimum scale condition:
			//	mScale = 0.5f;

			//if (mScale > 1f) // Maximum scale condition:
				//mScale = 1f;
            //ScaleAnimation scaleAnimation = new ScaleAnimation(1f / prevScale, 1f / mScale, 1f / prevScale, 1f / mScale, detector.FocusX, detector.FocusY)
            //{
            //    Duration = 0,
            //    FillAfter = true
            //};
            //StartAnimation(scaleAnimation);
			return true;

		}

		public bool OnScaleBegin(ScaleGestureDetector detector)
		{
            MessagingCenter.Send<object>(this, "PinchStart");
			return true;
		}

		public void OnScaleEnd(ScaleGestureDetector detector)
		{
            MessagingCenter.Send<object>(this, "PinchCompleted");
		}
    }
}
