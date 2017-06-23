using System;
using System.ComponentModel.Composition;

using MEFedMVVM.ViewModelLocator;

using Cinch;

using XRMS.Business.Models;
using XRMS.Business.Services;
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
    [ExportViewModel("TablesManagementViewModel")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class TablesManagementViewModel : ListViewModelBase<Table>
    {
        #region Private Data Members

        private IAreaManager _areaManager;
        private AreaListViewModel _areaListViewModel;

        #endregion // Private Data Members


        #region Constructors
        [ImportingConstructor]
        public TablesManagementViewModel(IMessageBoxService messageBoxService, IUIVisualizerService uiVisualizerService,
                                        ITableManager tableManager, IAreaManager areaManager) : base(messageBoxService, uiVisualizerService, tableManager)
        {
            // do initialization
            try
            {
                if (areaManager == null)
                {
                    throw new ArgumentNullException("areaManager");
                }
                _areaManager = areaManager;

                // get list
                this.DisplayName = "Tables Management";
                this.Items = new System.Collections.ObjectModel.ObservableCollection<Table>(GetItems());
                AreaListViewModel = new AreaListViewModel(this.MessageBoxService, this.UIVisualizerService, _areaManager);
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }
        #endregion // Constructors


        #region Public Properties

        /// <summary>
        /// A area list.
        /// </summary>
        public AreaListViewModel AreaListViewModel
        {
            get { return _areaListViewModel; }
            set { _areaListViewModel = value; NotifyPropertyChanged(() => AreaListViewModel); }
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
                TableViewModel tableViewModel = new TableViewModel(this.MessageBoxService, (ITableManager) ModelManager, _areaManager);

                // open dialog and return result when it is closed
                bool? result = this.UIVisualizerService.ShowDialog("TablePopup", tableViewModel);

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
        protected override void EditItem(Table item)
        {
            try
            {
                TableViewModel tableViewModel = new TableViewModel(this.MessageBoxService, item, (ITableManager) ModelManager, _areaManager);
                
                // open dialog and return result when it is closed
                bool? result = this.UIVisualizerService.ShowDialog("TablePopup", tableViewModel);

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
        protected override void DeleteItem(Table item)
        {
            try
            {
                // need warning for confirmation
                // ...
                if (this.MessageBoxService.ShowYesNo(
                "Would you like to remove table \"" + item.Name + "\" from list?",
                CustomDialogIcons.Question) == CustomDialogResults.Yes)
                {
                    bool result = ModelManager.Delete(item);
                    if (result)
                        this.MessageBoxService.ShowInformation("Successfully deleted table");

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