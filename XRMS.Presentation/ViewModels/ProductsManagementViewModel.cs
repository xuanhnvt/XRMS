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
    [ExportViewModel("ProductsManagementViewModel")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ProductsManagementViewModel : ListViewModelBase<Product>
    {
        #region Private Data Members

        private IProductGroupManager _productGroupManager;
        private IUnitManager _unitManager;

        private ProductGroupListViewModel _productGroupListViewModel;
        private UnitListViewModel _unitListViewModel;

        #endregion // Private Data Members

        #region Constructors
        [ImportingConstructor]
        public ProductsManagementViewModel(IMessageBoxService messageBoxService, IUIVisualizerService uiVisualizerService,
                                        IProductManager productManager, IProductGroupManager productGroupManager, IUnitManager unitManager) : base(messageBoxService, uiVisualizerService, productManager)
        {
            // do initialization
            try
            {
                if (productGroupManager == null)
                {
                    throw new ArgumentNullException("productGroupManager");
                }
                _productGroupManager = productGroupManager;

                if (unitManager == null)
                {
                    throw new ArgumentNullException("unitManager");
                }
                _unitManager = unitManager;

                this.DisplayName = "Products Management";
                ProductGroupListViewModel = new ProductGroupListViewModel(this.MessageBoxService, this.UIVisualizerService, _productGroupManager);
                UnitListViewModel = new UnitListViewModel(this.MessageBoxService, this.UIVisualizerService, _unitManager);

                //MessageBoxService.ShowInformation(SelectedItem.Name);
                (ModelManager as IProductManager).FetchProductRecipes(SelectedItem);
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
        public ProductGroupListViewModel ProductGroupListViewModel
        {
            get { return _productGroupListViewModel; }
            set { _productGroupListViewModel = value; NotifyPropertyChanged(() => ProductGroupListViewModel); }
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
                ProductViewModel viewModel = new ProductViewModel(this.MessageBoxService, (IProductManager) ModelManager, _productGroupManager, _unitManager);

                // open dialog and return result when it is closed
                bool? result = this.UIVisualizerService.ShowDialog("ProductPopup", viewModel);

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
        protected override void EditItem(Product item)
        {
            try
            {
                ProductViewModel viewModel = new ProductViewModel(this.MessageBoxService, item, (IProductManager)ModelManager, _productGroupManager, _unitManager);

                // open dialog and return result when it is closed
                bool? result = this.UIVisualizerService.ShowDialog("ProductPopup", viewModel);

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
        protected override void DeleteItem(Product item)
        {
            try
            {
                // need warning for confirmation
                // ...
                if (this.MessageBoxService.ShowYesNo(
                "Would you like to remove product \"" + item.Name + "\" from list?",
                CustomDialogIcons.Question) == CustomDialogResults.Yes)
                {
                    bool result = ModelManager.Delete(item);
                    if (result)
                        this.MessageBoxService.ShowInformation("Successfully deleted product");

                    // refresh item list
                    this.ExecuteRefreshCommand();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Process seleted item after user change selection.
        /// </summary>
        protected override void ProcessSelectedItem(Product selectedItem)
        {
            try
            {
                (ModelManager as IProductManager).FetchProductRecipes(selectedItem);
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