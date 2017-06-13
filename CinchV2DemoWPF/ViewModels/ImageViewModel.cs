using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cinch;
using System.ComponentModel;

namespace CinchV2DemoWPF
{
    /// <summary>
    /// Represents a single Image ViewModel
    /// </summary>
    public class ImageViewModel : ViewModelBase
    {
        #region Data
        private string imagePath;
        private string fileName;
        private DateTime fileDate;
        private string fileExtension;
        private int fileSize;
        private int rating;
        #endregion

        #region Public Properties
        /// <summary>
        /// ImagePath
        /// </summary>
        static PropertyChangedEventArgs imagePathArgs =
            ObservableHelper.CreateArgs<ImageViewModel>(x => x.ImagePath);

        public string ImagePath
        {
            get { return imagePath; }
            set
            {
                imagePath = value;
                NotifyPropertyChanged(imagePathArgs);
            }
        }


        /// <summary>
        /// FileName
        /// </summary>
        static PropertyChangedEventArgs fileNameArgs =
            ObservableHelper.CreateArgs<ImageViewModel>(x => x.FileName);

        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                NotifyPropertyChanged(fileNameArgs);
            }
        }


        /// <summary>
        /// FileDate
        /// </summary>
        static PropertyChangedEventArgs fileDateArgs =
            ObservableHelper.CreateArgs<ImageViewModel>(x => x.FileDate);

        public DateTime FileDate
        {
            get { return fileDate; }
            set
            {
                fileDate = value;
                NotifyPropertyChanged(fileDateArgs);
            }
        }


        /// <summary>
        /// FileExtension
        /// </summary>
        static PropertyChangedEventArgs fileExtensionArgs =
            ObservableHelper.CreateArgs<ImageViewModel>(x => x.FileExtension);

        public string FileExtension
        {
            get { return fileExtension; }
            set
            {
                fileExtension = value;
                NotifyPropertyChanged(fileDateArgs);
            }
        }


        /// <summary>
        /// FileSize
        /// </summary>
        static PropertyChangedEventArgs fileSizeArgs =
            ObservableHelper.CreateArgs<ImageViewModel>(x => x.FileSize);

        public int FileSize
        {
            get { return fileSize; }
            set
            {
                fileSize = value;
                NotifyPropertyChanged(fileDateArgs);
            }
        }


        /// <summary>
        /// Rating
        /// </summary>
        static PropertyChangedEventArgs ratingArgs =
            ObservableHelper.CreateArgs<ImageViewModel>(x => x.Rating);

        public int Rating
        {
            get { return rating; }
            set
            {
                rating = value;
                NotifyPropertyChanged(ratingArgs);
            }
        }

        #endregion

        #region Overrides
     
        public override string ToString()
        {
            return String.Format("ImageViewModel {0}", this.FileName);
        }
        #endregion
    }
}
