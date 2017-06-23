using System.Windows;
using System.Windows.Input;

using Cinch;

namespace XRMS.Presentation.Popups
{
    /// <summary>
    /// Interaction logic for RestaurantInfoPopup.xaml
    /// </summary>
    [PopupNameToViewLookupKeyMetadata("RestaurantInfoPopup", typeof(RestaurantInfoPopup))]
    public partial class RestaurantInfoPopup : Window
    {
        #region Construtors
        public RestaurantInfoPopup()
        {
            InitializeComponent();
        }
        #endregion // Construtors

        #region Deconstrutor
        /// <summary>
        /// Deconstructor : To see this being called just keep opening up 
        /// about popups and use the ok/cancel buttons, eventually
        /// when the .NET Garbage Collector sees fit it will collect.
        /// </summary>
        ~RestaurantInfoPopup()
        {

        }
        #endregion // Deconstructor

        #region // Event Handlers
        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
