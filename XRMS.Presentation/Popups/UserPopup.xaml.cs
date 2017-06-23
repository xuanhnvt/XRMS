using System.Windows;

using Cinch;

namespace XRMS.Presentation.Popups
{
    /// <summary>
    /// Interaction logic for UserPopup.xaml
    /// </summary>
    [PopupNameToViewLookupKeyMetadata("UserPopup", typeof(UserPopup))]
    public partial class UserPopup : Window
    {
        #region Construtors
        public UserPopup()
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
        ~UserPopup()
        {

        }
        #endregion // Deconstructor
    }
}
