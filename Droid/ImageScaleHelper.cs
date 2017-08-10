using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Media;
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

            BitmapFactory.Options options = await GetBitmapOptionsOfImage(urlString);
            //         BitmapFactory.Options options = new BitmapFactory.Options()
            //         {
            //             InSampleSize = 4
            //};
			Bitmap myBitmap = await LoadScaledDownBitmapForDisplayAsync(urlString, options, (int)App.ScreenWidth);
			//Bitmap myBitmap = await BitmapFactory.DecodeStreamAsync(inputSt, null, options);
            var ms = new MemoryStream();
			myBitmap.Compress(Bitmap.CompressFormat.Png, 0, ms);
            //return myBitmap;
			return ms.ToArray();

		}


		int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth)
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

                if (inSampleSize != 1)
                {
                    inSampleSize /= 2;
                }

			}
			return (int)inSampleSize;
		}

		async Task<Bitmap> LoadScaledDownBitmapForDisplayAsync(string imageURL, BitmapFactory.Options options, int reqWidth)
		{
			// Calculate inSampleSize
			options.InSampleSize = CalculateInSampleSize(options, reqWidth);

			// Decode bitmap with inSampleSize set
			options.InJustDecodeBounds = false;


			URL url = new URL(imageURL);
			HttpURLConnection connection = (HttpURLConnection)url.OpenConnection();
			connection.DoInput = true;
			await Task.Run(() => connection.Connect());
            System.IO.Stream inputSt = await Task.Run(() => connection.InputStream);

			return await BitmapFactory.DecodeStreamAsync(inputSt, null, options);
		}

        async Task<BitmapFactory.Options> GetBitmapOptionsOfImage(string imageURL)
        {

			URL url = new URL(imageURL);
			HttpURLConnection connection = (HttpURLConnection)url.OpenConnection();
			connection.DoInput = true;
			await Task.Run(() => connection.Connect());
			System.IO.Stream inputSt = await Task.Run(() => connection.InputStream);
            BitmapFactory.Options options = new BitmapFactory.Options
                                            {
                                                InJustDecodeBounds = true
                                            };

            // The result will be null because InJustDecodeBounds == true.
            Bitmap result=  await BitmapFactory.DecodeStreamAsync(inputSt, null, options);

            //int imageHeight = options.OutHeight;
            //int imageWidth = options.OutWidth;

            //_originalDimensions.Text = string.Format("Original Size= {0}x{1}", imageWidth, imageHeight);

            return options;
        }

        public async Task<string> OrientationOfImage(string urlString)
		{
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.N)
            {
				URL url = new URL(urlString);
				HttpURLConnection connection = (HttpURLConnection)url.OpenConnection();
				connection.DoInput = true;
				await Task.Run(() => connection.Connect());
				System.IO.Stream inputSt = await Task.Run(() => connection.InputStream);
				ExifInterface exifInterface = new ExifInterface(inputSt);
                return exifInterface.GetAttribute(ExifInterface.TagOrientation);
            }
            //else if(Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat && Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.N)
			//{
				//Java.IO.File path = new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), DateTime.Now.ToFileTime() + ".jpg");
				////Java.IO.File path = new Java.IO.File(Android.App.Application.Context.FilesDir.AbsolutePath, DateTime.Now.ToString() + ".jpg");

				//if (!path.Exists())
				//{
				//	path.CreateNewFile();
				//}

				//OutputStream os = new FileOutputStream(path);
    //            os.Write(imageByte);
    //            os.Close();


				//ExifInterface exifInterface = new ExifInterface(path.AbsolutePath);

    //            Debug.WriteLine("saved file path : " + path);
    //            //path.Delete();
				//return exifInterface.GetAttribute(ExifInterface.TagOrientation);
                				
            //}
            else
            {
                return "0";
            }
		}
		
    }
}
