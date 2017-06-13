using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

using System.ComponentModel.Composition;
using MEFedMVVM.ViewModelLocator;

//using DeepEqual;
//using DeepEqual.Syntax;

using Cinch;
using XRMS.Business;
using XRMS.Business.Models;
using XRMS.Business.Services;
using XRMS.Libraries.BaseClasses;
using XRMS.Libraries.Helpers;
using XRMS.Libraries.MVVM;


namespace XRMS.Presentation.ViewModels
{
    /// This is a workspace type ViewModel, which is created by some code in the <c>MainWindowViewModel</c>
    /// and the DataTemplate in the <c>MainWindow.xaml</c>
    /// 
    /// This ViewModel is also able to operate in design mode by using this ViewModel at runtime or by using
    /// data provided by a Design time ViewModel that inherits from this one.
    /// 
    /// This ViewModel also demonstrates EventToCommand where the XAML fires an Command in this ViewModel
    /// based on some RoutedEvent in the XAML.
    [ExportViewModel("MaterialsManagementViewModel")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class MaterialsManagementViewModel : ListViewModelBase<Material>
    {
        #region Private Data Members

        private IMaterialGroupManager _materialGroupManager;
        private IUnitManager _unitManager;

        private MaterialGroupListViewModel _materialGroupListViewModel;
        private UnitListViewModel _unitListViewModel;

        #endregion // Private Data Members

        #region Constructors
        [ImportingConstructor]
        public MaterialsManagementViewModel(IMessageBoxService messageBoxService, IUIVisualizerService uiVisualizerService,
                                        IMaterialManager materialManager, IMaterialGroupManager materialGroupManager, IUnitManager unitManager) : base(messageBoxService, uiVisualizerService, materialManager)
        {
            // do initialization
            try
            {
                if (materialGroupManager == null)
                {
                    throw new ArgumentNullException("materialGroupManager");
                }
                _materialGroupManager = materialGroupManager;

                if (unitManager == null)
                {
                    throw new ArgumentNullException("unitManager");
                }
                _unitManager = unitManager;

                this.DisplayName = "Materials Management";
                MaterialGroupListViewModel = new MaterialGroupListViewModel(this.MessageBoxService, this.UIVisualizerService, _materialGroupManager);
                UnitListViewModel = new UnitListViewModel(this.MessageBoxService, this.UIVisualizerService, _unitManager);
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }
        #endregion // Constructors


        #region Public Properties

        /// <summary>
        /// A group list.
        /// </summary>
        public MaterialGroupListViewModel MaterialGroupListViewModel
        {
            get { return _materialGroupListViewModel; }
            set { _materialGroupListViewModel = value; NotifyPropertyChanged(() => MaterialGroupListViewModel); }
        }

        /// <summary>
        /// A group list.
        /// </summary>
        public UnitListViewModel UnitListViewModel
        {
            get { return _unitListViewModel; }
            set { _unitListViewModel = value; NotifyPropertyChanged(() => UnitListViewModel); }
        }

        #endregion // Public Properties


        #region Command Properties

        #endregion // Command Properties

        #region Override Methods

        /// <summary>
        /// Create new item and add into database
        /// </summa
        protected override void CreateNewItem()
        {
            try
            {
                MaterialViewModel viewModel = new MaterialViewModel(this.MessageBoxService, (IMaterialManager) ModelManager, _materialGroupManager, _unitManager);

                // open dialog and return result when it is closed
                bool? result = this.UIVisualizerService.ShowDialog("MaterialPopup", viewModel);

                // code to check result
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Edit an existing item and update in database
        /// </summary>
        protected override void EditItem(Material item)
        {
            try
            {
                MaterialViewModel viewModel = new MaterialViewModel(this.MessageBoxService, item, (IMaterialManager) ModelManager, _materialGroupManager, _unitManager);

                // open dialog and return result when it is closed
                bool? result = this.UIVisualizerService.ShowDialog("MaterialPopup", viewModel);

                // code to check result
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete item from database
        /// </summary>
        protected override void DeleteItem(Material item)
        {
            try
            {
                // need warning for confirmation
                // ...
                if (this.MessageBoxService.ShowYesNo(
                "Would you like to remove material \"" + item.Name + "\" from list?",
                CustomDialogIcons.Question) == CustomDialogResults.Yes)
                {
                    bool result = ModelManager.Delete(item);
                    if (result)
                        this.MessageBoxService.ShowInformation("Successfully deleted material");

                    // refresh item list
                    this.ExecuteRefreshCommand();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion // Override Methods


        #region Private Method Members

        #endregion // Private Method Members
    }
}