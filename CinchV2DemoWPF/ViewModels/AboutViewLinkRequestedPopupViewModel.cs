using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cinch;
using System.ComponentModel;
using System.ComponentModel.Composition;

namespace CinchV2DemoWPF
{
    ///NOTE : As this is a popup we should not be manually setting the Views 
    ///ViewModel to a pre populated ViewModel, by using the 
    ///<c>WPFUIVisualizerService</c> service, as we would typically pass the
    ///popup and object to alter the state with
    public class AboutViewLinkRequestedPopupViewModel : ViewModelBase, IViewStatusAwareInjectionAware
    {
        #region Data
        private string navigateTo;
        #endregion

        #region Public Properties

        private IViewAwareStatus ViewAwareStatusService { get; set; }


        /// <summary>
        /// NavigateTo
        /// </summary>
        static PropertyChangedEventArgs navigateToArgs =
            ObservableHelper.CreateArgs<AboutViewLinkRequestedPopupViewModel>(x => x.NavigateTo);

        public string NavigateTo
        {
            get { return navigateTo; }
            set
            {
                navigateTo = value;
                NotifyPropertyChanged(navigateToArgs);
            }
        }
        #endregion

        #region IViewStatusAwareInjectionAware Members

        public void InitialiseViewAwareService(IViewAwareStatus viewAwareStatusService)
        {
            this.ViewAwareStatusService = viewAwareStatusService;
            this.ViewAwareStatusService.ViewLoaded += ViewAwareStatusService_ViewLoaded;
        }
        #endregion

        #region Private Methods

        private void ViewAwareStatusService_ViewLoaded()
        {
            //Get the View from the ViewAwareStatusService as a specific interface
            //and ask it to navigate its internal WebBrowser to the requested Url
            //Sometimes a tiny bit of code behind in the view is the correct thing
            //to do, we could abstract ourselves to insanity, but the thing is, if
            //it truly is a UI type operation and is not really something that requires
            //a lot of testing, I see nothing wrong with a tiny bit of code behind in the
            //view and that is what this is showing you
            IWebBrowserNavigatable webBrowserNavigatable = 
                this.ViewAwareStatusService.View as IWebBrowserNavigatable;
            if (webBrowserNavigatable != null)
            {
                ((IWebBrowserNavigatable)webBrowserNavigatable).NavigateTo(NavigateTo);
            }
        }

        #endregion
    }
}
