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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Cinch;
using System.Diagnostics;

namespace CinchV2DemoWPF
{
    /// <summary>
    /// This View is workspace aware, so if expected to be used in a workspace (such as a TabControl).
    /// This View is expecting a image path for the contextual data, this is supplied in the
    /// <c>MainWindowViewModel</c>, and is then passed to this View via the DataTemplate in the
    /// <c>MainWindow.xaml</c>, and from here is is passed to the <c>ImageLoaderViewModel</c> by the Cinch
    /// framework. This is done because the <c>ImageLoaderViewModel</c> implements 
    /// <c>IViewStatusAwareInjectionAware</c> so it needs to know about the <c>IViewAwareStatus</c> service
    /// for some contextual data
    /// </summary>
    [ViewnameToViewLookupKeyMetadata("ImageLoaderView", typeof(ImageLoaderView))]
    public partial class ImageLoaderView : UserControl, IWorkSpaceAware
    {
        #region Ctor
        public ImageLoaderView()
        {
            InitializeComponent();
        }
        #endregion

        #region Deconstructor
        /// <summary>
        /// When the GC sees fit to collect this view, make sure the ViewModel cleans up
        /// by calling ICinchDisposable.Dispose() method. The Cinch.ViewModelBase class
        /// unhook mediator messages in the ICinchDisposable.Dispose() method. And although the
        /// mediator uses Weak events, and auto unhooks dead actions, so there is no real problem, 
        /// it is still a good idea to clean up where possible
        /// </summary>
        ~ImageLoaderView()
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
            DependencyProperty.Register("WorkSpaceContextualData", typeof(object), typeof(ImageLoaderView),
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
