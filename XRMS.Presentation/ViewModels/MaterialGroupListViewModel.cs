using System;

using Cinch;

using XRMS.Business.Models;
using XRMS.Business.Services;
using XRMS.Libraries.MVVM;

namespace XRMS.Presentation.ViewModels
{
    public class MaterialGroupListViewModel : ListViewModelBase<MaterialGroup>
    {
        #region Private Data Members

        #endregion // Private Data Members


        #region Constructors
        public MaterialGroupListViewModel(IMessageBoxService messageBoxService, IUIVisualizerService uiVisualizerService, IMaterialGroupManager manager) : base(messageBoxService, uiVisualizerService, manager)
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
                MaterialGroupViewModel viewModel = new MaterialGroupViewModel(this.MessageBoxService, (IMaterialGroupManager)ModelManager);

                // open dialog and return result when it is closed
                bool? result = this.UIVisualizerService.ShowDialog("MaterialGroupPopup", viewModel);

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
        protected override void EditItem(MaterialGroup selectedItem)
        {
            try
            {
                if (selectedItem == null)
                    throw new ArgumentNullException("selectedItem");

                MaterialGroupViewModel viewModel = new MaterialGroupViewModel(this.MessageBoxService, selectedItem, (IMaterialGroupManager)ModelManager);

                // open dialog and return result when it is closed
                bool? result = this.UIVisualizerService.ShowDialog("MaterialGroupPopup", viewModel);

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
        protected override void DeleteItem(MaterialGroup selectedItem)
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
        protected override bool CanExecuteEditItemCommand(MaterialGroup selectedItem)
        {
            return selectedItem != null && selectedItem.Id != 0;
        }

        /// <summary>
        /// Logic to determine if DeleteCommand can execute
        /// </summary>
        protected override bool CanExecuteDeleteCommand(MaterialGroup selectedItem)
        {
            return selectedItem != null && selectedItem.Id != 0;
        }

        #endregion // Override Methods


        #region Private Method Members

        #endregion // Private Method Members
    }
}
