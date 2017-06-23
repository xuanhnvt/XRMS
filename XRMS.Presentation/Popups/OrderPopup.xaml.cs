using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using Cinch;

using XRMS.Business.Models;

namespace XRMS.Presentation.Popups
{
    /// <summary>
    /// Interaction logic for OrderPopup.xaml
    /// </summary>
    [PopupNameToViewLookupKeyMetadata("OrderPopup", typeof(OrderPopup))]
    public partial class OrderPopup : Window
    {
        #region Construtors
        public OrderPopup()
        {
            InitializeComponent();
            FilterTextBox.FilterTextBox.TextChanged += new TextChangedEventHandler(FilterTextBox_TextChanged);
        }
        #endregion // Construtors

        #region Deconstrutor
        /// <summary>
        /// Deconstructor : To see this being called just keep opening up 
        /// about popups and use the ok/cancel buttons, eventually
        /// when the .NET Garbage Collector sees fit it will collect.
        /// </summary>
        ~OrderPopup()
        {

        }
        #endregion // Deconstructor


        #region Private Methods
        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(ListViewOfAvailableProducts.ItemsSource).Filter = o => String.IsNullOrEmpty(FilterTextBox.Text) || ((Product)o).Name.ToLower().Contains(FilterTextBox.Text.ToLower());
        }
        #endregion // Private Methods
    }
}
