using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

using Cinch;
using XRMS.Libraries.BaseClasses;
using XRMS.Libraries.Helpers;
using XRMS.Business.Models;
using XRMS.Business.Services;
using XRMS.Libraries.MVVM;

namespace XRMS.Presentation.ViewModels
{
    public class TableListViewModel : ListViewModelBase<Table>
    {
        #region Private Data Members

        #endregion // Private Data Members


        #region Constructors

        public TableListViewModel(IMessageBoxService messageBoxService, IUIVisualizerService uiVisualizerService, ITableManager manager) : base(messageBoxService, uiVisualizerService, manager)
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
                TableViewModel tableViewModel = new TableViewModel(this.MessageBoxService, (ITableManager)ModelManager, new AreaManager());

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
        protected override void EditItem(Table selectedItem)
        {
            try
            {
                if (selectedItem == null)
                    throw new ArgumentNullException("selectedItem");

                TableViewModel tableViewModel = new TableViewModel(this.MessageBoxService, selectedItem, (ITableManager)ModelManager, new AreaManager());

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
        protected override void DeleteItem(Table selectedItem)
        {
            try
            {
                if (selectedItem == null)
                    throw new ArgumentNullException("selectedItem");

                // need warning for confirmation
                // ...
                if (this.MessageBoxService.ShowYesNo(
                "Would you like to delete table \"" + selectedItem.Name + "\" from list?",
                CustomDialogIcons.Question) == CustomDialogResults.Yes)
                {
                    bool result = ModelManager.Delete(selectedItem);
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

        /// <summary>
        /// Logic to determine if EditItemCommand can execute
        /// </summary>
        protected override bool CanExecuteEditItemCommand(Table selectedItem)
        {
            return selectedItem != null && selectedItem.Id != 0;
        }

        /// <summary>
        /// Logic to determine if DeleteCommand can execute
        /// </summary>
        protected override bool CanExecuteDeleteCommand(Table selectedItem)
        {
            return selectedItem != null && selectedItem.Id != 0;
        }

        #endregion // Override Methods


        #region Private Method Members

        #endregion // Private Method Members
    }
}
