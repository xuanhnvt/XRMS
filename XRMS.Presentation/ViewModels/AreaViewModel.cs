using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

//using DeepEqual;
//using DeepEqual.Syntax;
using Cinch;
using XRMS.Libraries.BaseClasses;
using XRMS.Libraries.Helpers;
using XRMS.Business;
using XRMS.Business.Models;
using XRMS.Business.Services;

using XRMS.Libraries.MVVM;

namespace XRMS.Presentation.ViewModels
{
    public class AreaViewModel : ItemViewModelBase<Area>
    {
        #region Private Data Members

        #endregion // Private Data Members

        #region Constructors

        public AreaViewModel(IMessageBoxService messageBoxService, IAreaManager manager) : base(messageBoxService, manager)
        {
            // do initialization
            try
            {
                this.DisplayName = "Create Area";
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }

        }

        public AreaViewModel(IMessageBoxService messageBoxService, Area area, IAreaManager manager) : base(messageBoxService, area, manager)
        {
            // do initialization
            try
            {
                this.DisplayName = "Edit Area: " + this.Item.Name;
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
