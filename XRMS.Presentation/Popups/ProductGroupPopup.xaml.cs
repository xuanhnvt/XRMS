using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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