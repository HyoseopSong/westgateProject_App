using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace westgateproject.Helper
{
    public interface IImageScaleHelper
    {
        Task<byte[]> GetImageStream(string urlString);
		Task<string> OrientationOfImage(string urlString);
    }
}
