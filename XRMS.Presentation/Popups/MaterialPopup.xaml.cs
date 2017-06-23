﻿using System.Windows;

using Cinch;

namespace XRMS.Presentation.Popups
{
    /// <summary>
    /// Interaction logic for MaterialPopup.xaml
    /// </summary>
    [PopupNameToViewLookupKeyMetadata("MaterialPopup", typeof(MaterialPopup))]
    public partial class MaterialPopup : Window
    {
        #region Construtors
        public MaterialPopup()
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
        ~MaterialPopup()
        {

        }
        #endregion // Deconstructor
    }
}
