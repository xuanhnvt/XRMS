using System;
using System.Collections.Generic;

using Cinch;

using XRMS.Business.Models;
using XRMS.Libraries.Helpers;
using XRMS.Libraries.MVVM;
using XRMS.Business.Services;

namespace XRMS.Presentation.ViewModels
{
    public class UserViewModel : ItemViewModelBase<User>
    {
        #region Private Data Members

        //services
        private IUserRoleManager _userRoleManager = null;
        private List<UserRole> _userRoleList = null;

        #endregion // Private Data Members

        #region Constructors

        public UserViewModel(IMessageBoxService messageBoxService, IUserManager manager, IUserRoleManager userRoleManager) : base(messageBoxService, manager)
        {
            // do initialization
            try
            {
                if (userRoleManager == null)
                {
                    throw new ArgumentNullException("userRoleManager");
                }
                _userRoleManager = userRoleManager;
                // populate the list of userRoles
                this.UserRoleList = _userRoleManager.GetList();

                this.DisplayName = "Create User";
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        public UserViewModel(IMessageBoxService messageBoxService, User table, IUserManager manager, IUserRoleManager userRoleManager) : base(messageBoxService, table, manager)
        {
            // do initialization
            try
            {
                if (userRoleManager == null)
                {
                    throw new ArgumentNullException("userRoleManager");
                }
                _userRoleManager = userRoleManager;
                // populate the list of userRoles
                this.UserRoleList = _userRoleManager.GetList();

                this.DisplayName = "Edit User: " + this.Item.Name;
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }
        #endregion // Constructors

        #region Public Properties

        /// <summary>
        /// The list of available userRole that table can be located in.
        /// </summary>
        public List<UserRole> UserRoleList
        {
            get { return _userRoleList; }
            set { _userRoleList = value; NotifyPropertyChanged("UserRoleList"); }
        }

        #endregion // Public Properties


        #region IDataErrorInfo Members

        /*string IDataErrorInfo.Error
        {
            get { return (this.Item as IDataErrorInfo).Error; }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                string error = null;
                    error = (this.Item as IDataErrorInfo)[propertyName];

                // Dirty the commands registered with CommandManager,
                // such as our Save command, so that they are queried
                // to see if they can execute now.
                CommandManager.InvalidateRequerySuggested();

                return error;
            }
        }
        */

        #endregion // IDataErrorInfo Members

        #region Command Properties

        #endregion // Command Properties


        #region Private Method Members

        #endregion  // Private Method Members


        #region Override Method Members

        /// <summary>
        /// ExecuteSaveCommand
        /// </summary>
        protected override void ExecuteSaveCommand()
        {
            // encrypt password
            Item.Password = SecurityHelper.GetMd5Hash("123456");
            base.ExecuteSaveCommand();
                    
        }

        #endregion // Override Method Members
    }
}

