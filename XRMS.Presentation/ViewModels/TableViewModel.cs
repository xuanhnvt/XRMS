using System;
using System.Collections.Generic;

using Cinch;

using XRMS.Business.Models;
using XRMS.Libraries.MVVM;
using XRMS.Business.Services;

namespace XRMS.Presentation.ViewModels
{
    public class TableViewModel : ItemViewModelBase<Table>
    {
        #region Private Data Members

        //services
        private IAreaManager _areaManager = null;
        private List<Area> _areaList = null;

        #endregion // Private Data Members

        #region Constructors

        public TableViewModel(IMessageBoxService messageBoxService, ITableManager manager, IAreaManager areaManager) : base(messageBoxService, manager)
        {
            // do initialization
            try
            {
                if (areaManager == null)
                {
                    throw new ArgumentNullException("areaManager");
                }
                _areaManager = areaManager;
                // populate the list of areas
                this.AreaList = _areaManager.GetList();

                this.DisplayName = "Create Table";
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        public TableViewModel(IMessageBoxService messageBoxService, Table table, ITableManager manager, IAreaManager areaManager) : base(messageBoxService, table, manager)
        {
            // do initialization
            try
            {
                if (areaManager == null)
                {
                    throw new ArgumentNullException("areaManager");
                }
                _areaManager = areaManager;
                // populate the list of areas
                this.AreaList = _areaManager.GetList();

                this.DisplayName = "Edit Table: " + this.Item.Name;
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }
        #endregion // Constructors

        #region Public Properties

        /// <summary>
        /// The list of available area that table can be located in.
        /// </summary>
        public List<Area> AreaList
        {
            get { return _areaList; }
            set { _areaList = value; NotifyPropertyChanged("AreaList"); }
        }

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
        }
        */

        #endregion // IDataErrorInfo Members

        #region Command Properties

        #endregion // Command Properties


        #region Private Method Members

        #endregion  // Private Method Members


        #region Override Method Members

        #endregion // Override Method Members
    }
}

