using System;
using System.Windows;
using System.Windows.Threading;
using System.ComponentModel.Composition;
using System.ComponentModel;
using System.Collections.Generic;

using MEFedMVVM.ViewModelLocator;
using MEFedMVVM.Common;

using Cinch;

using XRMS.Business.Models;
using XRMS.Business.Services;
using XRMS.Libraries.MVVM;

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

        private DispatcherTimer _refreshingTimer;
        private int _clockCounter = 0;

        private IRestaurantManager _restaurantManager = new RestaurantManager();
        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets message box service in order to show message to user
        /// </summary>
        public IMessageBoxService MessageBoxService { get; set; }

        /// <summary>
        /// Gets or sets UI service in order to call Popup Window
        /// </summary>
        public IUIVisualizerService UIVisualizerService { get; set; }

        #endregion

        #region Command Properties
        /// <summary>
        /// Gets or sets the login command.
        /// </summary>
        public CommandBase<User> ViewProfileCommand { get; set; }

        #endregion // ICommand Properties

        #region Ctor
        [ImportingConstructor]
        public MainWindowViewModel(IViewAwareStatus viewAwareStatusService, IMessageBoxService messageBoxService)
        {
            this.viewAwareStatusService = viewAwareStatusService;
            this.viewAwareStatusService.ViewLoaded += ViewAwareStatusService_ViewLoaded;
            this.MessageBoxService = messageBoxService;
            this.UIVisualizerService = ViewModelRepository.Instance.Resolver.Container.GetExport<IUIVisualizerService>().Value;
            this.DisplayName = "Main Window";


            this.ViewProfileCommand = new CommandBase<User>(o => this.ExecuteViewProfileCommand(o));

            if (Designer.IsInDesignMode)
                return;

            // add view
            WorkspaceData workspace = new WorkspaceData(@"/XRMS.Presentation;component/Images/Cashier.png",
                    "CashierOrdersView", null, "Orders Management", false);
            Views.Add(workspace);
            SetActiveWorkspace(workspace);

            _refreshingTimer = new DispatcherTimer();
            _refreshingTimer.Tick += new EventHandler(OnRefreshingData);
            _refreshingTimer.Interval = TimeSpan.FromMilliseconds(1000);
            _refreshingTimer.Start();

            ReadRestaurantInfo();
        }

        private void ExecuteViewProfileCommand(User item)
        {
            try
            {
                if (item == null)
                {
                    throw new ArgumentNullException("item");
                }
                UserViewModel userViewModel = new UserViewModel(this.MessageBoxService, item, (IUserManager)new UserManager(), new UserRoleManager());

                // open dialog and return result when it is closed
                bool? result = this.UIVisualizerService.ShowDialog("UserPopup", userViewModel);

                // code to check result
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Creates and returns the menu items
        /// </summary>
        private List<CinchMenuItem> CreateMenus()
        {
            List<CinchMenuItem> menu = new List<CinchMenuItem>();

            // create the File Menu
            var miFile = new CinchMenuItem("File");
            menu.Add(miFile);

            var miExit = new CinchMenuItem("Exit") { IconUrl = @"/XRMS.Presentation;component/Images/Exit.png" };
            miExit.Command = new SimpleCommand<object, object>((x) =>
            {
                if (MessageBoxService.ShowYesNo(
                "Would you like to exit application",
                CustomDialogIcons.Question) == CustomDialogResults.Yes)
                {
                    Application.Current.Shutdown(0);
                }
            });
            miFile.Children.Add(miExit);

            // create the Managements Menu
            var miManagement = new CinchMenuItem("Managements");
            menu.Add(miManagement);

            switch ((Role) MainUser.RoleId)
            {
                case Role.Manager:
                    {
                        CinchMenuItem miInfo = new CinchMenuItem("Restaurant Info") { IconUrl = @"/XRMS.Presentation;component/Images/RestaurantInfo.png" };
                        miInfo.Command = new SimpleCommand<object, object>((x) =>
                        {
                            try
                            {
                                if (RestaurantInfo == null)
                                    throw new ArgumentNullException("RestaurantInfo");

                                RestaurantInfoViewModel restaurantViewModel = new RestaurantInfoViewModel(this.MessageBoxService, RestaurantInfo, (IRestaurantManager)_restaurantManager);

                                // open dialog and return result when it is closed
                                bool? result = this.UIVisualizerService.ShowDialog("RestaurantInfoPopup", restaurantViewModel);

                                // code to check result
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        });
                        miManagement.Children.Add(miInfo);

                        CinchMenuItem miUsers = new CinchMenuItem("Users") { IconUrl = @"/XRMS.Presentation;component/Images/User.png" };
                        miUsers.Command = new SimpleCommand<object, object>((x) =>
                        {
                            WorkspaceData workspace = new WorkspaceData(@"/XRMS.Presentation;component/Images/User.png",
                                "UsersManagementView", null, "Users Management", true);

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
                        miManagement.Children.Add(miUsers);

                        CinchMenuItem miTables = new CinchMenuItem("Tables") { IconUrl = @"/XRMS.Presentation;component/Images/Table.png" };
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

                        CinchMenuItem miProducts = new CinchMenuItem("Products") { IconUrl = @"/XRMS.Presentation;component/Images/Product.png" };
                        miProducts.Command = new SimpleCommand<object, object>((x) =>
                        {
                            WorkspaceData workspace = new WorkspaceData(@"/XRMS.Presentation;component/Images/Product.png",
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

                        CinchMenuItem miMaterials = new CinchMenuItem("Materials") { IconUrl = @"/XRMS.Presentation;component/Images/Material.png" };
                        miMaterials.Command = new SimpleCommand<object, object>((x) =>
                        {
                            WorkspaceData workspace = new WorkspaceData(@"/XRMS.Presentation;component/Images/Material.png",
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

                        CinchMenuItem miCashierOrders = new CinchMenuItem("Cashier Orders") { IconUrl = @"/XRMS.Presentation;component/Images/Cashier.png" };
                        miCashierOrders.Command = new SimpleCommand<object, object>((x) =>
                        {
                            WorkspaceData workspace = new WorkspaceData(@"/XRMS.Presentation;component/Images/Cashier.png",
                                "CashierOrdersView", null, "Orders Management", false);

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
                        miManagement.Children.Add(miCashierOrders);

                        CinchMenuItem miKitchenOrders = new CinchMenuItem("Kitchen Orders") { IconUrl = @"/XRMS.Presentation;component/Images/Kitchen.png" };
                        miKitchenOrders.Command = new SimpleCommand<object, object>((x) =>
                        {
                            WorkspaceData workspace = new WorkspaceData(@"/XRMS.Presentation;component/Images/Kitchen.png",
                                "KitchenOrdersView", null, "Kitchen Management", true);

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
                        miManagement.Children.Add(miKitchenOrders);
                    }
                    break;
                case Role.Cashier:
                    {
                        CinchMenuItem miCashierOrders = new CinchMenuItem("Cashier Orders") { IconUrl = @"/XRMS.Presentation;component/Images/Cashier.png" };
                        miCashierOrders.Command = new SimpleCommand<object, object>((x) =>
                        {
                            WorkspaceData workspace = new WorkspaceData(@"/XRMS.Presentation;component/Images/Cashier.png",
                                "CashierOrdersView", null, "Orders Management", false);

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
                        miManagement.Children.Add(miCashierOrders);
                    }
                    break;
            }

            /*CinchMenuItem menuImages = new CinchMenuItem("ImageLoaderView");
            menuImages.Command = new SimpleCommand<object, object>((x) =>
            {
                String imagePath = ConfigurationManager.AppSettings["YourImagePath"].ToString();

                WorkspaceData workspaceImages = new WorkspaceData(@"/XRMS.Presentation;component/Images/imageIcon.png",
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

            /*_refreshingTimer.Start();
            ReadRestaurantInfo();

            WorkspaceData workspace = new WorkspaceData(@"/XRMS.Presentation;component/Images/Cashier.png",
                    "CashierOrdersView", null, "Orders Management", false);
            Views.Add(workspace);
            SetActiveWorkspace(workspace);*/
        }

        private void ImageWorkSpace_WorkspaceTabClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
            CustomDialogResults result =
                MessageBoxService.ShowYesNo("Are you sure you want to close this tab?",
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
                GlobalObjects.CurrentDateTime = _restaurantManager.GetDbCurrentDatetime();
                Clock = GlobalObjects.CurrentDateTime.ToString();
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
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
                RestaurantInfo = _restaurantManager.GetById(1);
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
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
