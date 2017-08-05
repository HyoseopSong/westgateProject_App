using System;
using System.IO;
using System.Threading.Tasks;

namespace westgateproject.Helper
{
    public interface IImageScaleHelper
    {
        Task<byte[]> GetImageStream(string urlString);
    }
}
