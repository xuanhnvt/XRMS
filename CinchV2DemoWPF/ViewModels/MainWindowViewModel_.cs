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
using System.Configuration;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;

using MEFedMVVM.ViewModelLocator;
using Cinch;
using MEFedMVVM.Common;

namespace CinchV2DemoWPF
{
    /// <summary>
    /// This ViewModel demonstrates how to use WorkSpaces and Menus. You will
    /// need to look in the MainWindow.xaml and also the AppStyles.xaml ResourceDictionary
    /// to see how the Styles are used to tie up with this ViewModel
    /// </summary>
    [ExportViewModel("MainWindowViewModel")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class MainWindowViewModel : ViewModelBase
    {
        #region Data
        private bool showContextMenu = false;
        private IViewAwareStatus viewAwareStatusService;
        private IMessageBoxService messageBoxService;
        #endregion

        #region Ctor
        [ImportingConstructor]
        public MainWindowViewModel(IViewAwareStatus viewAwareStatusService, IMessageBoxService messageBoxService)
        {
            this.viewAwareStatusService = viewAwareStatusService;
            this.viewAwareStatusService.ViewLoaded += ViewAwareStatusService_ViewLoaded;
            this.messageBoxService = messageBoxService;

            this.DisplayName = "Main Window";
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Creates and returns the menu items
        /// </summary>
        private List<CinchMenuItem> CreateMenus()
        {

            List<CinchMenuItem> menu = new List<CinchMenuItem>();

            CinchMenuItem menuActions = new CinchMenuItem("Actions");
            menu.Add(menuActions);

            CinchMenuItem menuAbout = new CinchMenuItem("About CinchV2");
            menuAbout.Command = new SimpleCommand<object, object>((x) =>
            {
                WorkspaceData workspace2 = new WorkspaceData(@"/CinchV2DemoWPF;component/Images/About.png",
                    "AboutView", null, "About Cinch V2", true);
                Views.Add(workspace2);
                ShowContextMenu = false;
            });
            menuActions.Children.Add(menuAbout);


            CinchMenuItem menuImages = new CinchMenuItem("ImageLoaderView");
            menuImages.Command = new SimpleCommand<object, object>((x) =>
            {
                String imagePath = ConfigurationManager.AppSettings["YourImagePath"].ToString();

                WorkspaceData workspaceImages = new WorkspaceData(@"/CinchV2DemoWPF;component/Images/imageIcon.png",
                    "ImageLoaderView", imagePath, "Image View", true);
                workspaceImages.WorkspaceTabClosing += ImageWorkSpace_WorkspaceTabClosing;

                Views.Add(workspaceImages);
                ShowContextMenu = false;
            });
            menuActions.Children.Add(menuImages);

            return menu;
        }

        private void ViewAwareStatusService_ViewLoaded()
        {

            if (Designer.IsInDesignMode)
                return;

            String imagePath = ConfigurationManager.AppSettings["YourImagePath"].ToString();


            WorkspaceData workspace1 = new WorkspaceData(@"/CinchV2DemoWPF;component/Images/imageIcon.png",
                "ImageLoaderView", imagePath, "Image View", true);
            workspace1.WorkspaceTabClosing += ImageWorkSpace_WorkspaceTabClosing;

            WorkspaceData workspace2 = new WorkspaceData(@"/CinchV2DemoWPF;component/Images/About.png",
                    "AboutView", null, "About Cinch V2", true);



            Views.Add(workspace1);
            Views.Add(workspace2);
            SetActiveWorkspace(workspace1);
        }

        private void ImageWorkSpace_WorkspaceTabClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
            CustomDialogResults result = 
                messageBoxService.ShowYesNo("Are you sure you want to close this tab?", 
                    CustomDialogIcons.Question);

            //if user did not want to cancel, keep workspace open
            if (result == CustomDialogResults.No)
            {
                e.Cancel = true;
            }
            //otherwise close workspace, and make sure to unhook WorkspaceTabClosing event
            //to prevent memory leak
            else
            {
                ((WorkspaceData)sender).WorkspaceTabClosing -= ImageWorkSpace_WorkspaceTabClosing;
            }
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Returns the bindbable Main Window options
        /// </summary>
        public List<CinchMenuItem> MainWindowOptions
        {
            get
            {
                return CreateMenus();
            }
        }


        /// <summary>
        /// ShowContextMenu
        /// </summary>
        static PropertyChangedEventArgs showContextMenuArgs =
            ObservableHelper.CreateArgs<MainWindowViewModel>(x => x.ShowContextMenu);

        public bool ShowContextMenu
        {
            get { return showContextMenu; }
            private set
            {
                showContextMenu = value;
                NotifyPropertyChanged(showContextMenuArgs);
            }
        }
        #endregion

    }

}
