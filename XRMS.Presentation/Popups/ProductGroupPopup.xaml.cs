using System.Windows;

using Cinch;

namespace XRMS.Presentation.Popups
{
    /// <summary>
    /// Interaction logic for ProductGroupPopup.xaml
    /// </summary>
    [PopupNameToViewLookupKeyMetadata("ProductGroupPopup", typeof(ProductGroupPopup))]
    public partial class ProductGroupPopup : Window
    {
        #region Construtors
        public ProductGroupPopup()
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
        ~ProductGroupPopup()
        {

        }
        #endregion // Deconstructor
    }
}