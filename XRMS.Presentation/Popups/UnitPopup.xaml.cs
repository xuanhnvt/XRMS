using System.Windows;

using Cinch;

namespace XRMS.Presentation.Popups
{
    /// <summary>
    /// Interaction logic for UnitPopup.xaml
    /// </summary>
    [PopupNameToViewLookupKeyMetadata("UnitPopup", typeof(UnitPopup))]
    public partial class UnitPopup : Window
    {
        #region Construtors
        public UnitPopup()
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
        ~UnitPopup()
        {

        }
        #endregion // Deconstructor
    }
}
