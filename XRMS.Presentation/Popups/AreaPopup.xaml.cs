using System.Windows;

using Cinch;

namespace XRMS.Presentation.Popups
{
    /// <summary>
    /// Interaction logic for AreaPopup.xaml
    /// </summary>
    [PopupNameToViewLookupKeyMetadata("AreaPopup", typeof(AreaPopup))]
    public partial class AreaPopup : Window
    {
        #region Construtors
        public AreaPopup()
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
        ~AreaPopup()
        {

        }
        #endregion // Deconstructor
    }
}
