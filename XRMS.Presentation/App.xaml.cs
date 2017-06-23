using System;
using System.Collections.Generic;
using System.Windows;
using System.Reflection;

using Csla.Core.FieldManager;
using Csla.CustomFieldData;

using Cinch;

using XRMS.Business.Repositories;
using XRMS.Business.Services;

namespace XRMS.Presentation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Initialisation
        /// <summary>
        /// Initiliase Cinch using the CinchBootStrapper. 
        /// </summary>
        /*public App()
        {
            CinchBootStrapper.Initialise(new List<Assembly> { typeof(App).Assembly });
            ObjectMappingHelper.Setup();
            PropertyInfoFactory.Factory = new PropertyInformationFactory();
            InitializeComponent();
        }*/
        #endregion

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            CinchBootStrapper.Initialise(new List<Assembly> { typeof(App).Assembly });
            ObjectMappingHelper.Setup();
            PropertyInfoFactory.Factory = new PropertyInformationFactory();

            // Disable shutdown when the dialog closes
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            var loginVM = new ViewModels.LoginViewModel(new WPFMessageBoxService(), new UserManager());
            var login = new Views.LoginView();
            login.DataContext = loginVM;

            loginVM.CloseRequest += (sender, args) =>
            {
                try
                {
                    // Execute this statement, dialog will also be closed
                    login.DialogResult = args.Result;
                }
                catch (Exception)
                {
                    login.Close();
                }
            };

            // show login dialog, and check login result
            if (login.ShowDialog() == true)
            {
                Views.MainWindow main = new Views.MainWindow();
                //var context = new MainWindowViewModel(loginVm.CurrentUser);
                var context = new ViewModels.MainWindowViewModel(new Cinch.ViewAwareStatus(), new WPFMessageBoxService());
                main.DataContext = context;
                // Re-enable normal shutdown mode.
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                Current.MainWindow = main;
                main.Show();
            }
            else
            {
                // exit
                Current.Shutdown(-1);
            }
        }
    }
}
