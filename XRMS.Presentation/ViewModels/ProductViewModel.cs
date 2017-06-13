using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

using Csla.Core;

using Cinch;
using XRMS.Libraries.BaseClasses;
using XRMS.Libraries.Helpers;
using XRMS.Business;
using XRMS.Business.Models;
using XRMS.Business.Services;

using XRMS.Libraries.MVVM;

namespace XRMS.Presentation.ViewModels
{
    public class ProductViewModel : ItemViewModelBase<Product>
    {
        #region Private Data Members

        //services
        private IProductGroupManager _groupManager = null;
        private List<ProductGroup> _groupList = null;

        private IUnitManager _unitManager = null;
        private List<Unit> _unitList = null;

        private IMaterialGroupManager _materialGroupManager = null;
        private List<MaterialGroup> _materialGroupList = null;

        private IMaterialManager _materialManager = null;
        private List<Material> _availableMaterialList = null;


        #endregion // Private Data Members

        #region Constructors

        public ProductViewModel(IMessageBoxService messageBoxService, IProductManager manager, IProductGroupManager groupManager, IUnitManager unitManager) : base(messageBoxService, manager)
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

                _materialGroupManager = new MaterialGroupManager();
                this.MaterialGroupList = _materialGroupManager.GetList();

                _materialManager = new MaterialManager();
                this.AvailableMaterialList = _materialManager.GetList();

                // initialize command
                this.SelectMaterialCommand = new CommandBase<Material>(o => this.ExecuteSelectMaterialCommand(o), o => this.CanExecuteSelectItemCommand(o));
                this.AddRecipeItemCommand = new CommandBase<Material>(o => this.ExecuteAddRecipeItemCommand(o), o => this.CanExecuteAddRecipeItemCommand(o));
                this.RemoveRecipeItemCommand = new CommandBase<RecipeItem>(o => this.ExecuteRemoveRecipeItemCommand(o), o => this.CanExecuteRemoveRecipeItemCommand(o));

                this.MarkDeletedCommand = new CommandBase<RecipeItem>(o => this.Item.Delete(), o => !this.Item.IsDeleted);

                this.DisplayName = "Create Product";
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }

        }

        public ProductViewModel(IMessageBoxService messageBoxService, Product item, IProductManager manager, IProductGroupManager groupManager, IUnitManager unitManager) : base(messageBoxService, item, manager)
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

                _materialGroupManager = new MaterialGroupManager();
                this.MaterialGroupList = _materialGroupManager.GetList();

                _materialManager = new MaterialManager();
                this.AvailableMaterialList = _materialManager.GetList();

                // initialize command
                this.SelectMaterialCommand = new CommandBase<Material>(o => this.ExecuteSelectMaterialCommand(o), o => this.CanExecuteSelectItemCommand(o));
                this.AddRecipeItemCommand = new CommandBase<Material>(o => this.ExecuteAddRecipeItemCommand(o), o => this.CanExecuteAddRecipeItemCommand(o));
                this.RemoveRecipeItemCommand = new CommandBase<RecipeItem>(o => this.ExecuteRemoveRecipeItemCommand(o), o => this.CanExecuteRemoveRecipeItemCommand(o));

                this.MarkDeletedCommand = new CommandBase<RecipeItem>(o => this.Item.Delete(), o => !this.Item.IsDeleted);

                this.DisplayName = "Edit Product: " + this.Item.Name;
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
        public List<ProductGroup> GroupList
        {
            get { return _groupList; }
            set { _groupList = value; NotifyPropertyChanged(() => GroupList); }
        }

        /// <summary>
        /// The list of materials in the store for adding product recipe.
        /// </summary>
        public List<Material> AvailableMaterialList
        {
            get { return _availableMaterialList; }
            set { _availableMaterialList = value; NotifyPropertyChanged(() => AvailableMaterialList); }
        }

        /// <summary>
        /// The list of material groups, for filtering when user find material.
        /// </summary>
        public List<MaterialGroup> MaterialGroupList
        {
            get { return _materialGroupList; }
            set { _materialGroupList = value; NotifyPropertyChanged(() => MaterialGroupList); }
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
        /// <summary>
        /// Gets or sets the select material command.
        /// </summary>
        public CommandBase<Material> SelectMaterialCommand { get; set; }

        /// <summary>
        /// Gets or sets the add recipe item command.
        /// </summary>
        public CommandBase<Material> AddRecipeItemCommand { get; set; }

        /// <summary>
        /// Gets or sets the remove recipe item command.
        /// </summary>
        public CommandBase<RecipeItem> RemoveRecipeItemCommand { get; set; }

        /// <summary>
        /// Gets or sets the mark deleted command.
        /// </summary>
        public CommandBase<RecipeItem> MarkDeletedCommand { get; set; }

        #endregion // Command Properties


        #region  Private Method Members

        /// <summary>
        /// Execute SelectItemCommand
        /// </summary>
        /// <param name="item">Selected item to be processed.</param>
        private void ExecuteSelectMaterialCommand(Material item)
        {
            try
            {
                //this.MessageBoxService.ShowInformation("ExecuteSelectMaterialCommand");
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Execute AddRecipeItemCommand
        /// </summary>
        /// <param name="item">Selected item to be processed.</param>
        private void ExecuteAddRecipeItemCommand(Material material)
        {
            try
            {
                //this.MessageBoxService.ShowInformation("ExecuteAddRecipeItemCommand");
                if (material == null)
                    throw new ArgumentNullException("material");

                /*if (Item.Recipes == null)
                {
                    //Item.Recipes = new System.Collections.ObjectModel.ObservableCollection<RecipeItem>();
                    Item.Recipes = new RecipeItemList();
                }*/

                RecipeItem newRecipeItem = this.Item.Recipes.AddNew();
                //RecipeItem newRecipeItem = RecipeItem.NewObject();
                //newRecipeItem.MarkAsChild();
                if (this.Item.Recipes.Count == 0)
                    newRecipeItem.Sequence = 0;
                else
                    newRecipeItem.Sequence = (byte)(this.Item.Recipes.Max(o => o.Sequence) + 1);
                newRecipeItem.ProductId = this.Item.Id;
                newRecipeItem.MaterialId = material.Id;
                newRecipeItem.UsedAmount = 1;
                newRecipeItem.MaterialInfo = _materialManager.GetById(material.Id);
                //this.Item.Recipes.Add(newRecipeItem);
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Execute RemoveRecipeItemCommand
        /// </summary>
        /// <param name="item">Selected item to be processed.</param>
        private void ExecuteRemoveRecipeItemCommand(RecipeItem item)
        {
            try
            {
                if (item == null)
                    throw new ArgumentNullException("item");

                //this.Item.Recipes.Remove(item);
                if (this.Item.Recipes.Contains(item))
                {
                    this.Item.Recipes.Remove(item);

                    List<RecipeItem> list = (List<RecipeItem>) (this.Item.Recipes as IEditableCollection).GetDeletedList();

                    string message = "Deleted List: \n";
                    foreach (RecipeItem recipeItem in list)
                    {
                        message += recipeItem.MaterialInfo.Name + "\n";
                    }
                    this.MessageBoxService.ShowInformation(message);

                }
                else
                {
                    this.MessageBoxService.ShowInformation("This item is not in the list.");
                }
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Determines whether the SelectItemCommand can be executed.
        /// </summary>
        /// <param name="item">The item that this function check on.</param>
        /// <returns>
        ///     <c>true</c> if the SelectItemCommand can be executed; otherwise, <c>false</c>.
        /// </returns>
        private bool CanExecuteSelectItemCommand(Material item)
        {
            return item != null;
        }

        /// <summary>
        /// Determines whether the AddRecipeItemCommand can be executed.
        /// </summary>
        /// <param name="item">The item that this function check on.</param>
        /// <returns>
        ///     <c>true</c> if the AddRecipeItemCommand can be executed; otherwise, <c>false</c>.
        /// </returns>
        private bool CanExecuteAddRecipeItemCommand(Material item)
        {
            return item != null;
        }

        /// <summary>
        /// Determines whether the RemoveRecipeItemCommand can be executed.
        /// </summary>
        /// <param name="item">The item that this function check on.</param>
        /// <returns>
        ///     <c>true</c> if the RemoveRecipeItemCommand can be executed; otherwise, <c>false</c>.
        /// </returns>
        private bool CanExecuteRemoveRecipeItemCommand(RecipeItem item)
        {
            return item != null;
        }

        #endregion // Private Method Members


        #region Override Method Members

        /// <summary>
        /// Executes the cancel command.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected override void ExecuteCancelCommand()
        {
            try
            {
                (this.Item as ISupportUndo).CancelEdit();

                // for test undo many times
                (this.Item as ISupportUndo).BeginEdit();
                //NotifyObjectChanged();
                //CloseActivePopUpCommand.Execute(false);
            }
            catch (Exception ex)
            {
                this.MessageBoxService.ShowError(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }

        #endregion // Override Method Members
    }
}
