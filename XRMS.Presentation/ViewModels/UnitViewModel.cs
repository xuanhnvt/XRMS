using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

using Cinch;
using XRMS.Libraries.BaseClasses;
using XRMS.Libraries.Helpers;
using XRMS.Business;
using XRMS.Business.Models;
using XRMS.Business.Services;

using XRMS.Libraries.MVVM;

namespace XRMS.Presentation.ViewModels
{
    public class UnitViewModel : ItemViewModelBase<Unit>
    {
        #region Private Data Members

        //services
        //private IUnitManager _manager = null;

        #endregion // Private Data Members

        #region Constructors

        public UnitViewModel(IMessageBoxService messageBoxService, IUnitManager manager) : base(messageBoxService, manager)
        {
            // do initialization
            try
            {
                this.DisplayName = "Create Unit";
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        public UnitViewModel(IMessageBoxService messageBoxService, Unit item, IUnitManager manager) : base(messageBoxService, item, manager)
        {
            // do initialization
            try
            {
                this.DisplayName = "Edit Unit: " + this.Item.Name;
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }
        #endregion // 


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
