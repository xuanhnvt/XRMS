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
using System.Windows.Threading;

using System.ComponentModel.Composition;
using System.Configuration;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;

using MEFedMVVM.ViewModelLocator;
using Cinch;
using MEFedMVVM.Common;

using XRMS.Business.Models;
using XRMS.Business.Services;

namespace XRMS.Presentation.ViewModels
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

        private DispatcherTimer _refreshingTimer;
        private int _clockCounter = 0;

        private IGlobalDataManager _globalDataManager = new GlobalDataManager();
        #endregion

        #region Ctor
        [ImportingConstructor]
        public MainWindowViewModel(IViewAwareStatus viewAwareStatusService, IMessageBoxService messageBoxService)
        {
            this.viewAwareStatusService = viewAwareStatusService;
            this.viewAwareStatusService.ViewLoaded += ViewAwareStatusService_ViewLoaded;
            this.messageBoxService = messageBoxService;
            this.DisplayName = "Main Window";

            // add view
            /*WorkspaceData workspace = new WorkspaceData(@"/CinchV2DemoWPF;component/Images/About.png",
                    "CashierOrdersView", null, "Orders Management", false);
            Views.Add(workspace);
            SetActiveWorkspace(workspace);*/

            // user login successfully
            //MainUser = GlobalObjects.SystemUser;

            _refreshingTimer = new DispatcherTimer();
            _refreshingTimer.Tick += new EventHandler(OnRefreshingData);
            _refreshingTimer.Interval = TimeSpan.FromMilliseconds(1000);
            //_refreshingTimer.Start();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Creates and returns the menu items
        /// </summary>
        private List<CinchMenuItem> CreateMenus()
        {
            /*var menu = new List<WPFMenuItem>();

            //create the File Menu
            var miFile = new WPFMenuItem("File");

            var miInfo = new WPFMenuItem("Restaurant Info")
            { IconUrl = @"..\Images\About.ico" };
            miInfo.Command = RestaurantInfoCommand;
            miFile.Children.Add(miInfo);

            var miExit = new WPFMenuItem("Exit")
            { IconUrl = @"..\Images\Exit.png" };
            miExit.Command = ExitApplicationCommand;
            miFile.Children.Add(miExit);

            menu.Add(miFile);

            //create the Managements Menu
            var miManagement = new WPFMenuItem("Managements");

            var miTables = new WPFMenuItem("Tables")
            { IconUrl = @"..\Images\Search.png" };
            miTables.Command = TablesManagementCommand;
            miManagement.Children.Add(miTables);

            var miProducts = new WPFMenuItem("Products")
            { IconUrl = @"..\Images\Search.png" };
            miProducts.Command = ProductsManagementCommand;
            miManagement.Children.Add(miProducts);

            var miKitchen = new WPFMenuItem("Kitchen")
            { IconUrl = @"..\Images\Search.png" };
            miKitchen.Command = KitchenManagementCommand;
            miManagement.Children.Add(miKitchen);

            var miMaterials = new WPFMenuItem("Materials")
            { IconUrl = @"..\Images\Search.png" };
            miMaterials.Command = MaterialsManagementCommand;
            miManagement.Children.Add(miMaterials);

            var miUsers = new WPFMenuItem("Users")
            { IconUrl = @"..\Images\Search.png" };
            miUsers.Command = UsersManagementCommand;
            miManagement.Children.Add(miUsers);

            menu.Add(miManagement);

            //create the Reports Menu
            var miReport = new WPFMenuItem("Reports");

            var miSellReport = new WPFMenuItem("Sell Report")
            { IconUrl = @"..\Images\Search.png" };
            miSellReport.Command = SellReportCommand;
            miReport.Children.Add(miSellReport);

            var miProductReport = new WPFMenuItem("Product Report")
            { IconUrl = @"..\Images\Search.png" };
            miProductReport.Command = ProductReportCommand;
            miReport.Children.Add(miProductReport);

            var miMaterialReport = new WPFMenuItem("Material Report")
            { IconUrl = @"..\Images\Search.png" };
            miMaterialReport.Command = MaterialReportCommand;
            miReport.Children.Add(miMaterialReport);

            menu.Add(miReport);

            return menu;*/
            List<CinchMenuItem> menu = new List<CinchMenuItem>();

            //create the File Menu
            var miFile = new CinchMenuItem("File");

            /*var miInfo = new CinchMenuItem("Restaurant Info")
            { IconUrl = @"..\Images\About.ico" };
            miInfo.Command = RestaurantInfoCommand;
            miFile.Children.Add(miInfo);*/

            var miExit = new CinchMenuItem("Exit") { IconUrl = @"/XRMS.Presentation;component/Images/Exit.png" };
            miExit.Command = new SimpleCommand<object, object>((x) =>
            {
                if (messageBoxService.ShowYesNo(
                "Would you like to exit application",
                CustomDialogIcons.Question) == CustomDialogResults.Yes)
                {
                    Application.Current.Shutdown(0);
                }
            });
            miFile.Children.Add(miExit);

            menu.Add(miFile);

            //create the Managements Menu
            var miManagement = new CinchMenuItem("Managements");

            CinchMenuItem miTables = new CinchMenuItem("Tables");
            miTables.Command = new SimpleCommand<object, object>((x) =>
            {
                WorkspaceData workspace = new WorkspaceData(@"/XRMS.Presentation;component/Images/Table.png",
                    "TablesManagementView", null, "Tables Management", true);

                WorkspaceData availableView = FindAvailableView(workspace);
                if (availableView == null)
                {
                    Views.Add(workspace);
                    SetActiveWorkspace(workspace);
                }
                else
                {
                    SetActiveWorkspace(availableView);
                }
            });
            miManagement.Children.Add(miTables);

            CinchMenuItem miProducts = new CinchMenuItem("Products");
            miProducts.Command = new SimpleCommand<object, object>((x) =>
            {
                WorkspaceData workspace = new WorkspaceData(@"/XRMS.Presentation;component/Images/Table.png",
                    "ProductsManagementView", null, "Products Management", true);
                WorkspaceData availableView = FindAvailableView(workspace);
                if (availableView == null)
                {
                    Views.Add(workspace);
                    SetActiveWorkspace(workspace);
                }
                else
                {
                    SetActiveWorkspace(availableView);
                }
            });
            miManagement.Children.Add(miProducts);

            CinchMenuItem miMaterials = new CinchMenuItem("Materials");
            miMaterials.Command = new SimpleCommand<object, object>((x) =>
            {
                WorkspaceData workspace = new WorkspaceData(@"/XRMS.Presentation;component/Images/Table.png",
                    "MaterialsManagementView", null, "Materials Management", true);
                WorkspaceData availableView = FindAvailableView(workspace);
                if (availableView == null)
                {
                    Views.Add(workspace);
                    SetActiveWorkspace(workspace);
                }
                else
                {
                    SetActiveWorkspace(availableView);
                }
            });
            miManagement.Children.Add(miMaterials);

            CinchMenuItem miCashierOrders = new CinchMenuItem("Cashier Orders");
            miCashierOrders.Command = new SimpleCommand<object, object>((x) =>
            {
                WorkspaceData workspace = new WorkspaceData(@"/CinchV2DemoWPF;component/Images/About.png",
                    "CashierOrdersView", null, "Orders Management", false);

                WorkspaceData availableView = FindAvailableView(workspace);
                if (availableView == null)
                {
                    Views.Add(workspace);
                    SetActiveWorkspace(workspace);
                }
                else
                {
                    //this.messageBoxService.ShowInformation("CashierOrdersView");
                    SetActiveWorkspace(availableView);
                }
            });
            miManagement.Children.Add(miCashierOrders);

            /*var miKitchen = new CinchMenuItem("Kitchen")
            { IconUrl = @"..\Images\Search.png" };
            miKitchen.Command = KitchenManagementCommand;
            miManagement.Children.Add(miKitchen);

            var miMaterials = new CinchMenuItem("Materials")
            { IconUrl = @"..\Images\Search.png" };
            miMaterials.Command = MaterialsManagementCommand;
            miManagement.Children.Add(miMaterials);

            var miUsers = new CinchMenuItem("Users")
            { IconUrl = @"..\Images\Search.png" };
            miUsers.Command = UsersManagementCommand;
            miManagement.Children.Add(miUsers);*/

            menu.Add(miManagement);

            /*CinchMenuItem menuImages = new CinchMenuItem("ImageLoaderView");
            menuImages.Command = new SimpleCommand<object, object>((x) =>
            {
                String imagePath = ConfigurationManager.AppSettings["YourImagePath"].ToString();

                WorkspaceData workspaceImages = new WorkspaceData(@"/CinchV2DemoWPF;component/Images/imageIcon.png",
                    "ImageLoaderView", imagePath, "Image View", true);
                workspaceImages.WorkspaceTabClosing += ImageWorkSpace_WorkspaceTabClosing;

                Views.Add(workspaceImages);
                ShowContextMenu = false;
            });
            menuActions.Children.Add(menuImages);*/

            return menu;
        }

        private void ViewAwareStatusService_ViewLoaded()
        {

            if (Designer.IsInDesignMode)
                return;

            ReadRestaurantInfo();
            _refreshingTimer.Start();

            WorkspaceData workspace = new WorkspaceData(@"/CinchV2DemoWPF;component/Images/About.png",
                    "CashierOrdersView", null, "Orders Management", false);
            Views.Add(workspace);
            SetActiveWorkspace(workspace);

            /*WorkspaceData workspace = new WorkspaceData(@"/XRMS.Presentation;component/Images/Table.png",
                    "TablesManagementView", null, "Tables Management", true);
            Views.Add(workspace);
            SetActiveWorkspace(workspace);*/
            //this.messageBoxService.ShowInformation("ViewAwareStatusService_ViewLoaded");
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

        private void OnRefreshingData(object sender, EventArgs e)
        {
            try
            {
                GlobalObjects.CurrentDateTime = _globalDataManager.GetDbCurrentDatetime();
                Clock = GlobalObjects.CurrentDateTime.ToString();
            }
            catch (Exception ex)
            {
                this.messageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }

            // refresh data every 5 seconds
            _clockCounter++;
            if (_clockCounter >= 5)
            {
                // Use the Mediator to send a Message to all opened pages refresh data
                Mediator.Instance.NotifyColleagues<Boolean>("MainWindowRequestRefreshingData", true);
                _clockCounter = 0;
            }
        }

        private void ReadRestaurantInfo()
        {
            try
            {
                RestaurantInfo = _globalDataManager.ReadRestaurantInfo();
            }
            catch (Exception ex)
            {
                this.messageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        private WorkspaceData FindAvailableView(WorkspaceData workspace)
        {

            if (workspace == null)
            {
                throw new ArgumentNullException("workspace");
            }

            WorkspaceData result = null;
            foreach (WorkspaceData view in Views)
            {
                if (view.ViewLookupKey == workspace.ViewLookupKey)
                {
                    result = view;
                    break;
                }
            }
            return result;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The restaurant
        /// </summary>
        public Restaurant RestaurantInfo
        {
            get { return GlobalObjects.RestaurantInfo; }
            set { GlobalObjects.RestaurantInfo = value; NotifyPropertyChanged("RestaurantInfo"); }
        }

        /// <summary>
        /// The user
        /// </summary>
        public User MainUser
        {
            get { return GlobalObjects.SystemUser; }
            set { GlobalObjects.SystemUser = value; NotifyPropertyChanged("MainUser"); }
        }

        /// <summary>
        /// The datetime clock
        /// </summary>
        public string Clock
        {
            get { return _clock; }
            set { _clock = value; NotifyPropertyChanged("Clock"); }
        }
        string _clock;

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
