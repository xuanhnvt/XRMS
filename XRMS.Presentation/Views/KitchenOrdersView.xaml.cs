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
using System.Windows.Navigation;
using System.Windows.Shapes;

using Cinch;

namespace XRMS.Presentation.Views
{
    /// <summary>
    /// This View is workspace aware, so if expected to be used in a workspace (such as a TabControl).
    /// This View is NOT expected any contextual data. The ViewModel is able to create all the data
    /// without any contextual data
    /// </summary>
    [ViewnameToViewLookupKeyMetadata("KitchenOrdersView", typeof(KitchenOrdersView))]
    public partial class KitchenOrdersView : UserControl, IWorkSpaceAware
    {
        #region Constructors
        public KitchenOrdersView()
        {
            InitializeComponent();
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
        ~KitchenOrdersView()
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
            DependencyProperty.Register("WorkSpaceContextualData", typeof(object), typeof(KitchenOrdersView),
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
    }
}

