using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.ComponentModel.Composition.Hosting;


using Cinch;
using System.Reflection;
using MEFedMVVM.ViewModelLocator;

namespace CinchV2DemoWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Initialisation
        /// <summary>
        /// Initiliase Cinch using the CinchBootStrapper. 
        /// </summary>
        public App()
        {
            CinchBootStrapper.Initialise(new List<Assembly> { typeof(App).Assembly });
            InitializeComponent();
        }
        #endregion
    }

}
