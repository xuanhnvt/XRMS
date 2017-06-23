using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using Cinch;

namespace XRMS.Presentation.Views
{
    /// <summary>
    /// This View is workspace aware, so if expected to be used in a workspace (such as a TabControl).
    /// This View is NOT expected any contextual data. The ViewModel is able to create all the data
    /// without any contextual data
    /// </summary>
    [ViewnameToViewLookupKeyMetadata("CashierOrdersView", typeof(CashierOrdersView))]
    public partial class CashierOrdersView : UserControl, IWorkSpaceAware
    {
        #region Constructors
        public CashierOrdersView()
        {
            InitializeComponent();
            FilterTextBox.FilterTextBox.TextChanged += new TextChangedEventHandler(FilterTextBox_TextChanged);
        }
        #endregion // Constructors

        #region Deconstructor
        /// <summary>
        /// When the GC sees fit to collect this view, make sure the ViewModel cleans up
        /// by calling ICinchDisposable.Dispose() method. The Cinch.ViewModelBase class
        /// unhook mediator messages in the ICinchDisposable.Dispose() method. And although the
        /// mediator uses Weak events, and auto unhooks dead actions, so there is no real problem, 
        /// it is still a good idea to clean up where possible
        /// </summary>
        ~CashierOrdersView()
        {
            this.Dispatcher.InvokeIfRequired(() =>
            {
                ((ICinchDisposable)this.DataContext).Dispose();
            });
        }
        #endregion

        #region IWorkSpaceAware Members

        /// <summary>
        /// WorkSpaceContextualData Dependency Property
        /// </summary>
        public static readonly DependencyProperty WorkSpaceContextualDataProperty =
            DependencyProperty.Register("WorkSpaceContextualData", typeof(object), typeof(CashierOrdersView),
                new FrameworkPropertyMetadata((WorkspaceData)null));

        /// <summary>
        /// Gets or sets the WorkSpaceContextualData property.  
        /// </summary>
        public WorkspaceData WorkSpaceContextualData
        {
            get { return (WorkspaceData)GetValue(WorkSpaceContextualDataProperty); }
            set { SetValue(WorkSpaceContextualDataProperty, value); }
        }

        #endregion


        #region Private Methods
        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(ListViewOfItems.ItemsSource).Filter = o => String.IsNullOrEmpty(FilterTextBox.Text) || ((XRMS.Business.Models.Order)o).Table.Name.ToLower().Contains(FilterTextBox.Text.ToLower());
        }
        #endregion // Private Methods
    }
}
