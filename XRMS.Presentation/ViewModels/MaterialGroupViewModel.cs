﻿using System;

using Cinch;

using XRMS.Business.Models;
using XRMS.Business.Services;
using XRMS.Libraries.MVVM;

namespace XRMS.Presentation.ViewModels
{
    public class MaterialGroupViewModel : ItemViewModelBase<MaterialGroup>
    {
        #region Private Data Members

        //services
        //private IMaterialGroupManager _manager = null;

        #endregion // Private Data Members

        #region Constructors

        public MaterialGroupViewModel(IMessageBoxService messageBoxService, IMaterialGroupManager manager) : base(messageBoxService, manager)
        {
            // do initialization
            try
            {
                this.DisplayName = "Create Material Group";
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        public MaterialGroupViewModel(IMessageBoxService messageBoxService, MaterialGroup item, IMaterialGroupManager manager) : base(messageBoxService, item, manager)
        {
            // do initialization
            try
            {
                this.DisplayName = "Edit Material Group: " + this.Item.Name;
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }
        #endregion // Constructors

        #region Public Properties

        #endregion // Public Properties

        #region IDataErrorInfo Members

        /*string IDataErrorInfo.Error
        {
            get { return (this.Item as IDataErrorInfo).Error; }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                string error = null;
                error = (this.Item as IDataErrorInfo)[propertyName];

                // Dirty the commands registered with CommandManager,
                // such as our Save command, so that they are queried
                // to see if they can execute now.
                CommandManager.InvalidateRequerySuggested();

                return error;
            }
        }*/

        #endregion // IDataErrorInfo Members

        #region Command Properties

        #endregion // Command Properties


        #region  Private Method Members

        #endregion // Private Method Members


        #region Override Method Members

        #endregion // Override Method Members
    }
}
