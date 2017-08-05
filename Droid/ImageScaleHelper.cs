using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Android.Graphics;
using Java.IO;
using Java.Net;
using westgateproject.Droid;
using westgateproject.Helper;

[assembly: Xamarin.Forms.Dependency(typeof(ImageScaleHelper))]
namespace westgateproject.Droid
{
    public class ImageScaleHelper:IImageScaleHelper
    {
        public ImageScaleHelper()
        {
        }

        public async Task<byte[]> GetImageStream(string urlString)
        {

			//         System.Console.WriteLine("URL : " + urlString);
			//Bitmap image = null;
			//try
			//{
			//	URL url = new URL(urlString);
			//	HttpURLConnection connection = (HttpURLConnection)url.OpenConnection();
			//	connection.DoInput = true;
			//	connection.Connect();
			//	Stream inputSt = connection.InputStream;
			//             BitmapFactory.Options options = new BitmapFactory.Options()
			//             {
			//                 InSampleSize = 2
			//             };
			//             image = await BitmapFactory.DecodeStreamAsync(inputSt);
			//}
			//catch (Exception e)
			//{
			//             System.Console.WriteLine(e.StackTrace);
			//}
			//var ms = new MemoryStream();
			//// Converting Bitmap image to byte[] array
			//if (image != null)
			//{
			//    image.Compress(Bitmap.CompressFormat.Png, 0, ms);
			//    //var imageByteArray = ms.ToArray();
			//    System.Console.WriteLine("image isn't null");
			//}
			//else
			//{
			//    System.Console.WriteLine("image is null");
			//}
			//return ms;

			//ThreadPool.QueueUserWorkItem(o => SlowMethod());

			URL url = new URL(urlString);
			HttpURLConnection connection = (HttpURLConnection)url.OpenConnection();
			connection.DoInput = true;
			await Task.Run(() => connection.Connect());
			Stream inputSt = await Task.Run(() => connection.InputStream);
            //BitmapFactory.Options options = await GetBitmapOptionsOfImage(inputSt);
            //Bitmap bitmapToDisplay = await LoadScaledDownBitmapForDisplayAsync(inputSt, options, (int)App.ScreenWidth);
            //System.Console.WriteLine("hahahahaha");
            //await bitmapToDisplay.CompressAsync(Bitmap.CompressFormat.Png, 0, ms);
            //System.Console.WriteLine("NiceNiceNice");
            BitmapFactory.Options options = new BitmapFactory.Options()
            {
                InSampleSize = 4
			};
			Bitmap myBitmap = await BitmapFactory.DecodeStreamAsync(connection.InputStream, null, options);
            var ms = new MemoryStream();
			myBitmap.Compress(Bitmap.CompressFormat.Png, 0, ms);
            //return myBitmap;

			return ms.ToArray();

		}


		public static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth)
		{
			// Raw height and width of image
			float width = options.OutWidth;
			double inSampleSize = 1D;

			//if (height > reqHeight || width > reqWidth)
			//{
			//	int halfHeight = (int)(height / 2);
			//	int halfWidth = (int)(width / 2);

			//	// Calculate a inSampleSize that is a power of 2 - the decoder will use a value that is a power of two anyway.
			//	while ((halfHeight / inSampleSize) > reqHeight && (halfWidth / inSampleSize) > reqWidth)
			//	{
			//		inSampleSize *= 2;
			//	}

			//}

			if (width > reqWidth)
			{
				int halfWidth = (int)(width / 2);

				// Calculate a inSampleSize that is a power of 2 - the decoder will use a value that is a power of two anyway.
				while ((halfWidth / inSampleSize) > reqWidth)
				{
					inSampleSize *= 2;
				}

			}

			return (int)inSampleSize;
		}

		public async Task<Bitmap> LoadScaledDownBitmapForDisplayAsync(Stream inputStream, BitmapFactory.Options options, int reqWidth)
		{
			//BitmapFactory.Options options = new BitmapFactory.Options()
			//{
			//	InSampleSize = 2
			//};			

			// Calculate inSampleSize
			options.InSampleSize = CalculateInSampleSize(options, reqWidth);

			// Decode bitmap with inSampleSize set
			options.InJustDecodeBounds = false;

			return await BitmapFactory.DecodeStreamAsync(inputStream, new Rect(0, 0, 0, 0), options);
		}

        async Task<BitmapFactory.Options> GetBitmapOptionsOfImage(Stream inputStream)
        {
            BitmapFactory.Options options = new BitmapFactory.Options
                                            {
                                                InJustDecodeBounds = true
                                            };

            // The result will be null because InJustDecodeBounds == true.
            Bitmap result=  await BitmapFactory.DecodeStreamAsync(inputStream, new Rect(0, 0, 0, 0), options);


            //int imageHeight = options.OutHeight;
            //int imageWidth = options.OutWidth;

            //_originalDimensions.Text = string.Format("Original Size= {0}x{1}", imageWidth, imageHeight);

            return options;
        }
    }
}
