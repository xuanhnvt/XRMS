using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Cinch;
using System.ComponentModel.Composition;

namespace CinchV2DemoWPF
{
    /// <summary>
    /// This example shows you you to show popups from
    /// Cinch V2, and also shows how you can use the View from your ViewModel
    /// using a specific interface to maybe carry out some very specific UI
    /// logic, such as navigating to a url in a control such as WebBrowser which
    /// is not really a WPF control (its more a thinly wrapped Win32 control), 
    /// and does not support Bindings.
    /// Sometimes code behind is the correct course of action
    /// </summary>
    [PopupNameToViewLookupKeyMetadata("AboutViewLinkRequestedPopup", typeof(AboutViewLinkRequestedPopup))]
    public partial class AboutViewLinkRequestedPopup : 
        Window, 
        IWebBrowserNavigatable //Show that sometimes code behind is the right thing to do
    {
        #region Ctor
        public AboutViewLinkRequestedPopup()
        {
            InitializeComponent();
        }
        #endregion

        #region Deconstructor
        /// <summary>
        /// Deconstructor : To see this being called just keep opening up 
        /// about popups and use close button (top right of popup) eventually
        /// when the .NET Garbage Collector sees fit it will collect.
        /// </summary>
        ~AboutViewLinkRequestedPopup()
        {
            Console.WriteLine("AboutViewLinkRequestedPopup deconstructor called");
        }
        #endregion

        #region IWebBrowserNavigatable Members

        /// <summary>
        /// Since the WebBrowser control in WPF is not a proper WPF control
        /// it does not support bindings and does not have DPs. So we could
        /// either use an attached DP to create a fake property that we could
        /// use with the WebBrowser or we could use a tiny bit of code behind
        /// and implement a specific interface that the ViewModel can use to 
        /// carry out this very UI specific bit of logic. 
        /// </summary>
        /// <param name="url">Navigate the WebBrowser to the supplied Url</param>
        public void NavigateTo(string url)
        {
            browser.Navigate(url);
        }

        #endregion

        #region Overrides
        /// <summary>
        /// Override on closing to make sure to Dispose() of wrapped HWnd based WebBrowser control
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            browser.Dispose();
        }
        #endregion
    }
}
