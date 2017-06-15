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
    /// we can use a Validating ViewModel, and also how
    /// to use the control focus from the ViewModel using the
    /// <c>TextBoxFocusBehavior</c>. 
    /// It also shows how to the <c>NumericTextBoxBehaviour</c> for
    /// the Rating TextBox.
    /// </summary>
    [PopupNameToViewLookupKeyMetadata("AddImageRatingPopup",typeof(AddImageRatingPopup))]
    public partial class AddImageRatingPopup : Window
    {
        #region Ctor
        public AddImageRatingPopup()
        {
            InitializeComponent();
        }
        #endregion

        #region Deconstructor
        /// <summary>
        /// Deconstructor : To see this being called just keep opening up 
        /// about popups and use the ok/cancel buttons, eventually
        /// when the .NET Garbage Collector sees fit it will collect.
        /// </summary>
        ~AddImageRatingPopup()
        {
            Console.WriteLine("AddImageRatingPopup deconstructor called");
        }
        #endregion
    }
}
