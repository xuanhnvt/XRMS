using System;
using System.Collections.Generic;

using Cinch;

using XRMS.Business.Models;
using XRMS.Business.Services;
using XRMS.Libraries.MVVM;

namespace XRMS.Presentation.ViewModels
{
    public class MaterialViewModel : ItemViewModelBase<Material>
    {
        #region Private Data Members

        //services
        private IMaterialGroupManager _groupManager = null;
        private List<MaterialGroup> _groupList = null;

        private IUnitManager _unitManager = null;
        private List<Unit> _unitList = null;

        #endregion // Private Data Members

        #region Constructors

        public MaterialViewModel(IMessageBoxService messageBoxService, IMaterialManager manager, IMaterialGroupManager groupManager, IUnitManager unitManager) : base(messageBoxService, manager)
        {
            // do initialization
            try
            {
                if (groupManager == null)
                {
                    throw new ArgumentNullException("groupManager");
                }
                _groupManager = groupManager;
                // populate the list of groups
                this.GroupList = _groupManager.GetList();

                if (unitManager == null)
                {
                    throw new ArgumentNullException("unitManager");
                }
                _unitManager = unitManager;
                // populate the list of units
                this.UnitList = _unitManager.GetList();

                this.DisplayName = "Create Material";
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }

        }

        public MaterialViewModel(IMessageBoxService messageBoxService, Material item, IMaterialManager manager, IMaterialGroupManager groupManager, IUnitManager unitManager) : base(messageBoxService, item, manager)
        {
            // do initialization
            try
            {
                if (groupManager == null)
                {
                    throw new ArgumentNullException("groupManager");
                }
                _groupManager = groupManager;
                // populate the list of groups
                this.GroupList = _groupManager.GetList();

                if (unitManager == null)
                {
                    throw new ArgumentNullException("unitManager");
                }
                _unitManager = unitManager;
                // populate the list of units
                this.UnitList = _unitManager.GetList();

                this.DisplayName = "Edit Material: " + this.Item.Name;
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }
        #endregion // Constructors

        #region Public Properties

        /// <summary>
        /// The list of available group that item can be located in.
        /// </summary>
        public List<MaterialGroup> GroupList
        {
            get { return _groupList; }
            set { _groupList = value; NotifyPropertyChanged(() => GroupList); }
        }

        /// <summary>
        /// The list of available unit that item can use.
        /// </summary>
        public List<Unit> UnitList
        {
            get { return _unitList; }
            set { _unitList = value; NotifyPropertyChanged(() => UnitList); }
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
