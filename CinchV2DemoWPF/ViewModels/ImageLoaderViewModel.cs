using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Data;
using System.IO;
using System.Linq;

using Cinch;
using MEFedMVVM.ViewModelLocator;
using MEFedMVVM.Services.Contracts;
using MEFedMVVM.Common;


namespace CinchV2DemoWPF
{
    /// <summary>
    /// This is a workspace type ViewModel, which is created by some code in the <c>MainWindowViewModel</c>
    /// and the DataTemplate in the <c>MainWindow.xaml</c>
    /// 
    /// This is the main ViewModel of the demo app, and this ViewModel makes use
    /// of pretty much all the Cinch services available.
    /// 
    /// This ViewModel is also able to operate in design mode by using the
    /// <c>IImageProvider</c> data service, which can either be a runtime service or a design time service
    /// 
    /// This ViewModel is also expecting some contextual data from the <c>IViewAwareStatus</c> service
    /// (as we are using a View 1st type of development in CinchV2). This contextual data is the
    /// image path that the <c>IImageProvider</c> data service uses to fetch images.
    /// 
    /// As such this ViewModel expects the <c>IViewAwareStatus</c> service which allows the ViewModel
    /// to examine this contextual view data
    /// 
    /// Anyway in this ViewModel you will find a demo of most of the services Cinch has to offer
    /// </summary>
    [ExportViewModel("ImageLoaderViewModel")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ImageLoaderViewModel : ViewModelBase
    {
        #region Data
        private string directoryName;
        private List<ImageViewModel> loadedImages;
        private ICollectionView loadedImagesCV;
        private IImageProvider imageProvider;
        private IImageDiskOperations imageDiskOperations;
        private IViewAwareStatus viewAwareStatusService;
        private IMessageBoxService messageBoxService;
        private IOpenFileService openFileService;
        private ISaveFileService saveFileService;
        private IUIVisualizerService uiVisualizerService;
        #endregion

        #region Ctor
        [ImportingConstructor]
        public ImageLoaderViewModel(
            IMessageBoxService messageBoxService,
            IOpenFileService openFileService,
            ISaveFileService saveFileService,
            IUIVisualizerService uiVisualizerService,
            IImageProvider imageProvider,
            IImageDiskOperations imageDiskOperations,
            IViewAwareStatus viewAwareStatusService)
        {
            //setup services
            this.messageBoxService = messageBoxService;
            this.openFileService = openFileService;
            this.saveFileService = saveFileService;
            this.uiVisualizerService = uiVisualizerService;
            this.imageProvider = imageProvider;
            this.imageDiskOperations = imageDiskOperations;
            this.viewAwareStatusService = viewAwareStatusService;
            this.viewAwareStatusService.ViewLoaded += ViewAwareStatusService_ViewLoaded;

            //commands, SimpleCommand<T1,T2> T1 is CanExecute parameter type, and T2 is Execute type
            AddImageRatingCommand = new SimpleCommand<Object, Object>(ExecuteAddImageRatingCommand);
            SaveToFileCommand = new SimpleCommand<Object, Object>(ExecuteSaveToFileCommand);
            OpenExistingFileCommand = new SimpleCommand<Object, Object>(ExecuteOpenExistingFileCommand);

            //EventToCommand triggered, see the View
            ShowActionsCommand = new SimpleCommand<Object, Object>(ExecuteShowActionsCommand);
            HideActionsCommand = new SimpleCommand<Object, Object>(ExecuteHideActionsCommand);

            //some reverse commands, that the VM fires, and the View uses as CompletedAwareCommandTriggers
            //to carry out some actions. In this case GoToStateActions are used in the View
            ShowActionsCommandReversed = new SimpleCommand<Object, Object>((input) => { });
            HideActionsCommandReversed = new SimpleCommand<Object, Object>((input) => { });

        }
        #endregion

        #region Public Properties

        //commands
        public SimpleCommand<Object, Object> HideActionsCommand { get; private set; }
        public SimpleCommand<Object, Object> ShowActionsCommand { get; private set; }
        public SimpleCommand<Object, Object> HideActionsCommandReversed { get; private set; }
        public SimpleCommand<Object, Object> ShowActionsCommandReversed { get; private set; }
        public SimpleCommand<Object, Object> AddImageRatingCommand { get; private set; }
        public SimpleCommand<Object, Object> SaveToFileCommand { get; private set; }
        public SimpleCommand<Object, Object> OpenExistingFileCommand { get; private set; }


        /// <summary>
        /// Loaded Images 
        /// </summary>
        static PropertyChangedEventArgs loadedImagesCVArgs =
            ObservableHelper.CreateArgs<ImageLoaderViewModel>(x => x.LoadedImagesCV);

        public ICollectionView LoadedImagesCV
        {
            get { return loadedImagesCV; }
            set
            {
                loadedImagesCV = value;
                NotifyPropertyChanged(loadedImagesCVArgs);
            }
        }

        /// <summary>
        /// Image directory name
        /// </summary>
        static PropertyChangedEventArgs directoryNameArgs =
            ObservableHelper.CreateArgs<ImageLoaderViewModel>(x => x.DirectoryName);

        public string DirectoryName
        {
            get { return directoryName; }
            set
            {
                directoryName = value;
                NotifyPropertyChanged(directoryNameArgs);
            }
        }
        #endregion

        #region Command Handlers

        /// <summary>
        /// Saves the  List<ImageViewModel> to a XML file using XLINQ
        /// </summary>
        private void ExecuteSaveToFileCommand(Object args)
        {
            saveFileService.InitialDirectory = @"C:\";
            saveFileService.OverwritePrompt = true;
            saveFileService.Filter = ".xml | XML Files";
    
            var result = saveFileService.ShowDialog(null);
            if (result.HasValue && result.Value == true)
            {
                try
                {
                    if (imageDiskOperations.Save(saveFileService.FileName, loadedImages.AsEnumerable()))
                    {
                        messageBoxService.ShowInformation(string.Format("Successfully saved images to file\r\n{0}",
                            saveFileService.FileName));
                    }
                }
                catch (Exception ex)
                {
                    messageBoxService.ShowError(
                        string.Format("An error occurred saving images to file\r\n{0}",ex.Message));
                }
            }
        }

        /// <summary>
        /// Create a new List<ImageViewModel> by reading a XML file using XLINQ
        /// </summary>
        private void ExecuteOpenExistingFileCommand(Object args)
        {
            openFileService.InitialDirectory = @"C:\";
            openFileService.Filter = ".xml | XML Files";

            var result = openFileService.ShowDialog(null);
            if (result.HasValue && result.Value == true)
            {
                try
                {
                    List<ImageViewModel> xmlReadViewModels = imageDiskOperations.Open(openFileService.FileName);
                    if (xmlReadViewModels != null)
                    {
                        loadedImages = xmlReadViewModels;
                        LoadedImagesCV = CollectionViewSource.GetDefaultView(loadedImages);
                        if (loadedImages != null)
                            LoadedImagesCV.MoveCurrentTo(loadedImages.First());

                        messageBoxService.ShowInformation(string.Format("Successfully retreived images from file\r\n{0}",
                            saveFileService.FileName));
                    }
                    else
                    {
                        messageBoxService.ShowError(string.Format("Couldn't load any images from file\r\n{0}",
                            saveFileService.FileName));
                    }
                }
                catch (Exception ex)
                {
                    messageBoxService.ShowError(
                        string.Format("An error occurred opening file\r\n{0}", ex.Message));
                }
            }
           
        }

        /// <summary>
        /// Goto "ShowActionsState", which use the VisualStateManagerService
        /// </summary>
        private void ExecuteShowActionsCommand(Object args)
        {
            ShowActionsCommandReversed.Execute(null);
        }

        /// <summary>
        /// Goto "HideActionsState", which use the VisualStateManagerService
        /// </summary>
        private void ExecuteHideActionsCommand(Object args)
        {
            HideActionsCommandReversed.Execute(null);
        }

        /// <summary>
        /// Show the AddImageRatingPopup using the IUIVisualizerService, passing
        /// it a ValidatingViewModel that should validate that a valid rating between
        /// 1-5 is entered by the user. If we get a valid rating then apply it to the
        /// currently selected ImageViewModel
        /// </summary>
        private void ExecuteAddImageRatingCommand(Object args)
        {
            ImageRatingViewModel imageRatingViewModel = new ImageRatingViewModel(messageBoxService);
            imageRatingViewModel.ImageRating.DataValue = ((ImageViewModel)loadedImagesCV.CurrentItem).Rating;


            bool? result = uiVisualizerService.ShowDialog("AddImageRatingPopup", imageRatingViewModel);
            if (result.HasValue && result.Value)
            {
                ((ImageViewModel)loadedImagesCV.CurrentItem).Rating = 
                    imageRatingViewModel.ImageRating.DataValue;
            }
        }
        #endregion

        #region Private Methods
        private void ViewAwareStatusService_ViewLoaded()
        {
            if (!Designer.IsInDesignMode)
            {
                var view = viewAwareStatusService.View;
                IWorkSpaceAware workspaceData = (IWorkSpaceAware)view;
                DirectoryName = (String)workspaceData.WorkSpaceContextualData.DataValue;
            }
            LoadImages(DirectoryName);
        }


        private void LoadImages(string imagePath)
        {
            imageProvider.FetchImages(imagePath, LoadImagesFromRetrievedData);
        }


        private void LoadImagesFromRetrievedData(List<ImageData> data)
        {
            loadedImages = GetImageViewModels(data);
            LoadedImagesCV = CollectionViewSource.GetDefaultView(loadedImages);
            if (loadedImages != null)
                LoadedImagesCV.MoveCurrentTo(loadedImages.First());
        }


        private List<ImageViewModel> GetImageViewModels(List<ImageData> data)
        {
            List<ImageViewModel> imageViewModels = new List<ImageViewModel>();
            foreach (ImageData imageData in data)
            {
                imageViewModels.Add(new ImageViewModel()
                {
                    ImagePath = imageData.ImagePath,
                    FileName = imageData.FileName,
                    FileDate = imageData.FileDate,
                    FileExtension = imageData.FileExtension,
                    FileSize = imageData.FileSize
                });
            }

            return imageViewModels;
        }


        #endregion
    }

}
