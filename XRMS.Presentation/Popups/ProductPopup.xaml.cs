using System.Windows;

using Cinch;

namespace XRMS.Presentation.Popups
{
    /// <summary>
    /// Interaction logic for ProductPopup.xaml
    /// </summary>
    [PopupNameToViewLookupKeyMetadata("ProductPopup", typeof(ProductPopup))]
    public partial class ProductPopup : Window
    {
        #region Construtors
        public ProductPopup()
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
        ~ProductPopup()
        {

        }
        #endregion // Deconstructor
    }
}
