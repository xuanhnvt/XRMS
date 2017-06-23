using System;
using System.ComponentModel.Composition;

using MEFedMVVM.ViewModelLocator;

using Cinch;

using XRMS.Business.Models;
using XRMS.Business.Services;
using XRMS.Libraries.MVVM;

namespace XRMS.Presentation.ViewModels
{
    /// This is a workspace type ViewModel, which is created by some code in the <c>MainWindowViewModel</c>
    /// and the DataTemplate in the <c>MainWindow.xaml</c>
    /// 
    /// This ViewModel is also able to operate in design mode by using this ViewModel at runtime or by using
    /// data provided by a Design time ViewModel that inherits from this one.
    /// 
    /// This ViewModel also demonstrates EventToCommand where the XAML fires an Command in this ViewModel
    /// based on some RoutedEvent in the XAML.
    [ExportViewModel("UsersManagementViewModel")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class UsersManagementViewModel : ListViewModelBase<User>
    {
        #region Private Data Members

        private IUserRoleManager _userRoleManager;
        //private UserRoleListViewModel _userRoleListViewModel;

        #endregion // Private Data Members


        #region Constructors
        [ImportingConstructor]
        public UsersManagementViewModel(IMessageBoxService messageBoxService, IUIVisualizerService uiVisualizerService,
                                        IUserManager userManager, IUserRoleManager userRoleManager) : base(messageBoxService, uiVisualizerService, userManager)
        {
            // do initialization
            try
            {
                if (userRoleManager == null)
                {
                    throw new ArgumentNullException("userRoleManager");
                }
                _userRoleManager = userRoleManager;

                // get list
                this.DisplayName = "Users Management";
                this.Items = new System.Collections.ObjectModel.ObservableCollection<User>(GetItems());
                //UserRoleListViewModel = new UserRoleListViewModel(this.MessageBoxService, this.UIVisualizerService, _userRoleManager);
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }
        #endregion // Constructors


        #region Public Properties

        /// <summary>
        /// A UserRole list.
        /// </summary>
        /*public UserRoleListViewModel UserRoleListViewModel
        {
            get { return _userRoleListViewModel; }
            set { _userRoleListViewModel = value; NotifyPropertyChanged(() => UserRoleListViewModel); }
        }*/

        #endregion // Public Properties


        #region Command Properties

        #endregion // Command Properties

        #region Override Methods

        /// <summary>
        /// Create new item and add into database
        /// </summa
        protected override void CreateNewItem()
        {
            try
            {
                UserViewModel userViewModel = new UserViewModel(this.MessageBoxService, (IUserManager)ModelManager, _userRoleManager);

                // open dialog and return result when it is closed
                bool? result = this.UIVisualizerService.ShowDialog("UserPopup", userViewModel);

                // code to check result
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Edit an existing item and update in database
        /// </summary>
        protected override void EditItem(User item)
        {
            try
            {
                UserViewModel userViewModel = new UserViewModel(this.MessageBoxService, item, (IUserManager)ModelManager, _userRoleManager);

                // open dialog and return result when it is closed
                bool? result = this.UIVisualizerService.ShowDialog("UserPopup", userViewModel);

                // code to check result
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete item from database
        /// </summary>
        protected override void DeleteItem(User item)
        {
            try
            {
                // need warning for confirmation
                // ...
                if (this.MessageBoxService.ShowYesNo(
                "Would you like to remove user \"" + item.Name + "\" from list?",
                CustomDialogIcons.Question) == CustomDialogResults.Yes)
                {
                    bool result = ModelManager.Delete(item);
                    if (result)
                        this.MessageBoxService.ShowInformation("Successfully deleted user");

                    // refresh item list
                    this.ExecuteRefreshCommand();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion // Override Methods

        #region Private Method Members

        #endregion // Private Method Members
    }
}