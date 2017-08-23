using System;
using System.Diagnostics;
using System.Drawing;
using CoreGraphics;
using Foundation;
using UIKit;
using westgateproject.Helper;
using westgateproject.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(ImageResizeHelper))]
namespace westgateproject.iOS
{
    public class ImageResizeHelper : IImageResizeHelper
    {
        public ImageResizeHelper()
        {
        }

		public byte[] ResizeImageIOS(string imagePath)
		{
            UIImage sourceImage = new UIImage(imagePath);
			//using (NSData imageData = image.AsPNG())
			//{
			//	Byte[] myByteArray = new Byte[imageData.Length];
			//	System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, myByteArray, 0, Convert.ToInt32(imageData.Length));
			//}
			//UIImage originalImage = ImageFromByteArray(imageData);
			UIImageOrientation orientation = sourceImage.Orientation;

			var sourceSize = sourceImage.Size;
			var maxResizeFactor = Math.Max(700 / sourceSize.Width, 700 / sourceSize.Height);
			if (maxResizeFactor > 1) return sourceImage.AsJPEG().ToArray();
			var width = maxResizeFactor * sourceSize.Width;
			var height = maxResizeFactor * sourceSize.Height;
			UIGraphics.BeginImageContext(new CGSize(width, height));
			sourceImage.Draw(new CGRect(0, 0, width, height));
			Debug.WriteLine("width : " + width);
			Debug.WriteLine("height : " + height);
			var resultImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			return resultImage.AsJPEG((float)0.5).ToArray();

		}

		//private UIKit.UIImage ImageFromByteArray(byte[] data)
		//{
		//	if (data == null)
		//	{
		//		return null;
		//	}

		//	UIKit.UIImage image;
		//	try
		//	{
		//		image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
		//	}
		//	catch (Exception e)
		//	{
		//		Console.WriteLine("Image load failed: " + e.Message);
		//		return null;
		//	}
		//	return image;
		//}

		// resize the image (without trying to maintain aspect ratio)
		//public UIImage ResizeImage(this UIImage sourceImage, float width, float height)
		//{
		//	UIGraphics.BeginImageContext(new SizeF(width, height));
		//	sourceImage.Draw(new RectangleF(0, 0, width, height));
		//	var resultImage = UIGraphics.GetImageFromCurrentImageContext();
		//	UIGraphics.EndImageContext();
		//	return resultImage;
		//}

		//// crop the image, without resizing
		//public UIImage CropImage(this UIImage sourceImage, int crop_x, int crop_y, int width, int height)
		//{
		//	var imgSize = sourceImage.Size;
		//	UIGraphics.BeginImageContext(new SizeF(width, height));
		//	var context = UIGraphics.GetCurrentContext();
		//	var clippedRect = new RectangleF(0, 0, width, height);
		//	context.ClipToRect(clippedRect);
		//	var drawRect = new CGRect(-crop_x, -crop_y, imgSize.Width, imgSize.Height);
		//	sourceImage.Draw(drawRect);
		//	var modifiedImage = UIGraphics.GetImageFromCurrentImageContext();
		//	UIGraphics.EndImageContext();
		//	return modifiedImage;
		//}
    }
}
