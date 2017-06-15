using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.IO;

using MEFedMVVM.ViewModelLocator;
using Cinch;

namespace CinchV2DemoWPF
{

    /// <summary>
    /// Runtime implementation of the 
    /// Data service used by the <c>ImageLoaderViewModel</c> to obtain data
    /// </summary>
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [ExportService(ServiceType.Runtime, typeof(IImageProvider))]
    public class RunTimeImageProvider : IImageProvider
    {
        #region Data
        private BackgroundTaskManager<string, List<ImageData>> bgWorker = 
            new BackgroundTaskManager<string, List<ImageData>>();
        #endregion



        #region Public Methods/Properties

        public void FetchImages(string imagePath, Action<List<ImageData>> callback)
        {
            bgWorker.TaskFunc = (argument) =>
                {
                    return FetchImagesInternal(argument);
                };

            bgWorker.CompletionAction = (result) =>
                {
                    callback(result);
                };

            bgWorker.WorkerArgument = imagePath;
            bgWorker.RunBackgroundTask();

        }


        /// <summary>
        /// To allow this class to be unit tested stand alone
        /// See CinchV1 articles about Unit Testing for this
        /// Or comments in Cinch BackgroundTaskManager<T> class
        /// </summary>
        public BackgroundTaskManager<string,List<ImageData>> BgWorker
        {
            get { return bgWorker; }
        }

        #endregion

        #region Private Methods
        private List<ImageData> FetchImagesInternal(string imagePath)
        {
            List<string> imageFiles = new List<string>();

            string strFilter = "*.jpg;*.png;*.gif";
            string[] filters = strFilter.Split(';');
            foreach (string filter in filters)
            {
                imageFiles.AddRange(Directory.GetFiles(imagePath, filter));
            }

            List<ImageData> images = new List<ImageData>();

            if (imageFiles.Count > 0)
            {
                int maxImages = imageFiles.Count > 20 ? 20 : imageFiles.Count;

                for (int i = 0; i < maxImages; i++)
                {
                    FileInfo fi = new FileInfo(imageFiles[i]);
                    ImageData id = new ImageData();
                    id.ImagePath = imageFiles[i];
                    id.FileDate = fi.LastWriteTime;
                    id.FileExtension = fi.Extension;
                    id.FileName = fi.Name;
                    id.FileSize = (int)fi.Length / 1024;
                    images.Add(id);
                }
            }

            return images;

        }
        #endregion

    }
}
