using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinchV2DemoWPF
{
    /// <summary>
    /// Data service used by the <c>ImageLoaderViewModel</c> to obtain data
    /// </summary>
    public interface IImageProvider
    {
        void FetchImages(string imagePath, Action<List<ImageData>> callback);
    }

    /// <summary>
    /// Data class used by <c>IImageProvider</c>
    /// </summary>
    public class ImageData
    {
        public string ImagePath { get; set; }
        public string FileName { get; set; }
        public DateTime FileDate { get; set; }
        public string FileExtension { get; set; }
        public int FileSize { get; set; }
    }

}
