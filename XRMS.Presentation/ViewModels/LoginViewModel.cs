using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cinch;
using XRMS.Libraries.BaseClasses;
using XRMS.Libraries.Helpers;
using XRMS.Libraries.Constants;
using XRMS.Business;
using XRMS.Business.Models;
using XRMS.Business.Services;

using XRMS.Libraries.MVVM;

namespace XRMS.Presentation.ViewModels
{
    public class LoginViewModel : ItemViewModelBase<User>
    {
        #region Private Data Members

        private string _password;

        #endregion // Data Members

        #region Constructors
        public LoginViewModel(IMessageBoxService messageBoxService, IUserManager modelManager) : base(messageBoxService, modelManager)
        {
            Item.Name = "manager";
            Password = "123456";

            this.LoginCommand = new CommandBase<object>(o => Login());
        }
        #endregion // Constructors

        #region Public Properties

        /// <summary>
        /// Password to login.
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; NotifyPropertyChanged(() => Password); }
        }

        #endregion // Public Properties

        #region Command Properties
        /// <summary>
        /// Gets or sets the login command.
        /// </summary>
        public CommandBase<object> LoginCommand { get; set; }

        #endregion // ICommand Properties

        #region Private Method Members

        private void Login()
        {
            bool errorFlag = false;
            String errorText = "";
            if (String.IsNullOrEmpty(Item.Name))
            {
                errorText = MessageList.UserNotValid;
                errorFlag = true;
            }
            else if (String.IsNullOrEmpty(Password))
            {
                errorText = MessageList.PasswordNotValid;
                errorFlag = true;
            }
            else if (!(Password.Length >= 3))
            {
                errorText = MessageList.PasswordNotValid;
                errorFlag = true;
            }
            if (errorFlag == false)
            {
                try
                {
                    Item.Password = SecurityHelper.GetMd5Hash(Password);
                    CheckUser(Item);
                }
                catch (Exception ex)
                {
                    MessageService.ShowError(MessageList.UnhandleException + ex.Message);
                }
            }
            else
            {
                MessageService.ShowError(errorText);
            }
        }

        private void CheckUser(User loginUser)
        {
            // Default UserName and Password for Config
            // UserName = Config and Password = ConfigPass
            if (loginUser.Name == "Config" && loginUser.Password == "096E62FD294491F0D2E9C007012C9E94")
            {
                /*
                // Show SQL Connection Config Form
                ConnectionConfigWindow objConfigWindow = new ConnectionConfigWindow();
                this.Hide();
                objConfigWindow.Owner = this;
                bool dg = (bool)objConfigWindow.ShowDialog();
                if (dg == true)
                {
                    this.DialogResult = false;
                }
                else
                {
                    txbUserName.Focus();
                }
                objConfigWindow = null;*/
            }
            else
            {
                try
                {
                    User searchUser = (ModelManager as IUserManager).GetList().FirstOrDefault(x => x.Name == loginUser.Name);

                    if (searchUser != null)
                    {
                        // waiter is not allowed to login into this client software
                        if (searchUser.Role.Id == (byte)Role.Waiter)
                        {
                            MessageService.ShowWarning(MessageList.UserNotAllowed);
                        }
                        else
                        {
                            if (searchUser.Password == loginUser.Password)
                            {
                                // get all information of user
                                try
                                {
                                    // get current user that logged in
                                    GlobalObjects.SystemUser = (ModelManager as IUserManager).GetById(searchUser.Id);
                                    // request close the view
                                    this.RaiseCloseRequest(true);
                                }
                                catch (Exception ex)
                                {
                                    MessageBoxService.ShowError("Error while read information of user");
                                }
                            }
                            else
                            {
                                MessageService.ShowWarning(MessageList.PasswordNotValid);
                            }
                        }
                    }
                    else
                    {
                        MessageService.ShowWarning(MessageList.UserNotValid);
                    }
                }
                catch (Exception ex)
                {
                    MessageService.ShowError(MessageList.UnhandleException + ex.Message);
                }
            }
        }

        #endregion // Private Method Members
    }
}
