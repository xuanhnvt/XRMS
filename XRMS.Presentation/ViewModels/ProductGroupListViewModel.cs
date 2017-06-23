using System;

using Cinch;

using XRMS.Business.Models;
using XRMS.Business.Services;
using XRMS.Libraries.MVVM;

namespace XRMS.Presentation.ViewModels
{
    public class ProductGroupListViewModel : ListViewModelBase<ProductGroup>
    {
        #region Private Data Members

        #endregion // Private Data Members


        #region Constructors
        public ProductGroupListViewModel(IMessageBoxService messageBoxService, IUIVisualizerService uiVisualizerService, IProductGroupManager manager) : base(messageBoxService, uiVisualizerService, manager)
        {
            // do initialization
            try
            {

            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }
        #endregion // Constructors


        #region Public Properties

        #endregion // Public Properties


        #region Command Properties

        #endregion // Command Properties


        #region Override Methods
        
        /// <summary>
        /// Create new item and add into database
        /// </summary>
        protected override void CreateNewItem()
        {
            try
            {
                ProductGroupViewModel viewModel = new ProductGroupViewModel(this.MessageBoxService, (IProductGroupManager) ModelManager);

                // open dialog and return result when it is closed
                bool? result = this.UIVisualizerService.ShowDialog("ProductGroupPopup", viewModel);

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
        protected override void EditItem(ProductGroup selectedItem)
        {
            try
            {
                if (selectedItem == null)
                    throw new ArgumentNullException("selectedItem");

                ProductGroupViewModel viewModel = new ProductGroupViewModel(this.MessageBoxService, selectedItem, (IProductGroupManager) ModelManager);

                // open dialog and return result when it is closed
                bool? result = this.UIVisualizerService.ShowDialog("ProductGroupPopup", viewModel);

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
        protected override void DeleteItem(ProductGroup selectedItem)
        {
            try
            {
                if (selectedItem == null)
                    throw new ArgumentNullException("selectedItem");

                // need warning for confirmation
                // ...
                if (this.MessageBoxService.ShowYesNo(
                "Would you like to delete group \"" + selectedItem.Name + "\" from list?",
                CustomDialogIcons.Question) == CustomDialogResults.Yes)
                {
                    bool result = ModelManager.Delete(selectedItem);
                    if (result)
                        this.MessageBoxService.ShowInformation("Successfully deleted group");

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
        /// Logic to determine if EditItemCommand can execute
        /// </summary>
        protected override bool CanExecuteEditItemCommand(ProductGroup selectedItem)
        {
            return selectedItem != null && selectedItem.Id != 0;
        }

        /// <summary>
        /// Logic to determine if DeleteCommand can execute
        /// </summary>
        protected override bool CanExecuteDeleteCommand(ProductGroup selectedItem)
        {
            return selectedItem != null && selectedItem.Id != 0;
        }

        #endregion // Override Methods


        #region Private Method Members

        #endregion // Private Method Members
    }
}
